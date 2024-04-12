﻿using EventManagmentSystem.DAL;
using EventManagmentSystem.Models.DbModel;
using EventManagmentSystem.Models.ViewModel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EventManagmentSystem.Services
{
    public class UserService 
    {
        //IHttContextAccessor wird benötigt um auf die Userdaten zuzugreifen
        private readonly IHttpContextAccessor _httpContextAccessor;
        //DbContext wird benötigt um auf die Datenbank zuzugreifen
        private readonly EventDbContext _context;

        public UserService(EventDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        //Methode um User zu speichern
        public bool TryRegisterUser(RegistrationViewModel model, out string errorMessage)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                errorMessage = "Email ist schon vorhanden";
                return false;
            }

            //Generieren von Salt damit das Passwort nicht im Klartext in der Db gespeichert wird
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            //passwort und salt hashen.
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Password = hashedPassword,
                Role = model.Role, 
                IsAdmin = model.IsAdmin,
                Salt = Convert.ToBase64String(salt),
            };
            _context.Add(user);
            _context.SaveChanges();

            errorMessage = null;
            return true;
        }

        //Login Methode
        public (bool Success, string errorMessage, User user) TryLoginUser(LoginViewModel model)
        {
            //check ob User vorhanden ist
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser == null)
            {

                return (false, "Email oder Passwort ist falsch", null);
            }
            //passwort und salt hashen. damit wir das Passwort mit dem in der Db vergleichen können
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.Password,
                salt: Convert.FromBase64String(existingUser.Salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            //check ob das Passwort stimmt

            if (existingUser.Password != hashedPassword)
            {
                return (false, "Email oder Passwort ist falsch", null);
            }

            return (true, null, existingUser);
        }

        //Methode zum zeigen alle users für den Admin
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        //Methode zum zeigen die daten eines users für sein Profil (EditProfile)
        //ich verwende hier async und await damit die Methode asynchron ausgeführt wird
        //im Controller so aufgerufen: var user = await _userService.GetUserAsync(userId);
        public async Task<User> GetUserAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }
        public bool IsAdmin()
        {
            var isAdmin = _httpContextAccessor.HttpContext.Session.GetString("Admin");
            if (isAdmin != null)
            {
                if (bool.TryParse(isAdmin, out bool isAdminBool))
                {
                    return isAdminBool;
                }
            }

            // 
            var user = _httpContextAccessor.HttpContext.User;
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userIdInt))
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.UserId == userIdInt);
                return userInDb != null && userInDb.IsAdmin;
            }

            return false;
        }



    }
}


