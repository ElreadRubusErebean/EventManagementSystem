using EventManagmentSystem.DAL;
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
            return false;
        }

        //Methode zum löschen des Benutzerkontos (NormalUser)
        public async Task<bool> DeleteUserAsync(int userId)
        {
            /// <summary>
            /// To Do : wenn User noch offene Buchungen hat, dann werden diese einfach gelöscht
            /// wenn User sein Konto löscht
            /// </summary>            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return false;
            }
            //hier wird geprüft, ob der User noch offene Buchungen hat
            var bookings = await _context.Bookings.Where(b => b.UserId == userId).ToListAsync();
            foreach (var booking in bookings)
            {
                var ev = await _context.Events.FirstOrDefaultAsync(e => e.EventId == booking.EventId);
                if (ev != null)
                {
                    //Anzahl der verfügbaren Tickets aktualisierens
                    ev.AmountOfTickets += booking.AmountOfTickets;
                }
                _context.Bookings.Remove(booking);
            }
            await _context.SaveChangesAsync();

            // Benutzer löschen
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
        /// <summary>
        /// ich habe hier eine neue Methode hinzugefügt, um das Benutzerkonto eines Verkäufers zu löschen
        /// weil mit dem käufer das löschen des Kontos anders ist als mit dem Verkäufer
        /// </summary>
        public async Task<bool> DeleteSellerAccountAsync(int sellerId)
        {
            //zuerst die userId des Verkäufers holen und checken ob das wirklich ein Verkäufer ist
            var seller = await _context.Users.FirstOrDefaultAsync(u => u.UserId == sellerId);
            if (seller == null)
            {
                return false; // Verkäufer nicht gefunden
            }

            // Überprüfen, ob der Verkäufer Events hat
            //und ob die events von dem Verkäufer sind
            //wenn ja, dann werden alle Events und Buchungen für das Event gelöscht
            var sellerEvents = await _context.Events.Where(e => e.UserId == sellerId).ToListAsync();
            foreach (var ev in sellerEvents)
            {
                //hier werden geschaut ob das Event noch Buchungen hat
                //wenn ja dann werden die Buchungen gelöscht
                //ansonsten springt der code weiter und löscht das Event
                // Alle Buchungen für das Event löschen
                var bookings = await _context.Bookings.Where(b => b.EventId == ev.EventId).ToListAsync();
                foreach (var booking in bookings)
                {
                    _context.Bookings.Remove(booking);
                }

                // Event löschen
                _context.Events.Remove(ev);
            }
            //wenn alle Abhängigkeiten gelöscht sind, dann wird das Benutzerkonto gelöscht
            // Benutzerkonto des Verkäufers löschen
            _context.Users.Remove(seller);

            await _context.SaveChangesAsync();

            return true;
        }
        //Methode zum updaten des Benutzerkontos
        public async Task<bool> UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
            await _context.SaveChangesAsync();
            return true;
        }


    }
}


