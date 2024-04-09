using EventManagmentSystem.DAL;
using EventManagmentSystem.Models;
using EventManagmentSystem.Models.DbModel;
using EventManagmentSystem.Models.ViewModel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
namespace EventManagmentSystem.Services
{
    public class UserService 
    {
        private readonly EventDbContext _context;
        public UserService(EventDbContext context)
        {
            _context = context;
        }

        //To do Get User Email

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

        //Login Todo
        public (bool Success, string errorMessage, User user) TryLoginUser(LoginViewModel model)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser == null)
            {

                return (false, "Email oder Passwort ist falsch", null);
            }
            //passwort und salt hashen.
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.Password,
                salt: Convert.FromBase64String(existingUser.Salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (existingUser.Password != hashedPassword)
            {
                return (false, "Email oder Passwort ist falsch", null);
            }

            return (true, null, existingUser);
        }

    }
}


