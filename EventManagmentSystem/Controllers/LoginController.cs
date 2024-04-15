using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;
using EventManagmentSystem.Models.ViewModel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using EventManagmentSystem.Models.DbModel;
using EventManagmentSystem.Enums;

namespace EventManagmentSystem.Controllers
{
    public class LoginController : ValidationController
    {
        private readonly UserService _userService;

        public LoginController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                SetErrorMessage("Email oder Passwort sit falsch");
                return View(model);
            }

            var (Success, ErrorMessage, User) = _userService.TryLoginUser(model);
            if (Success)
            {
                HttpContext.Session.SetString("UserID", User.UserId.ToString());
                HttpContext.Session.SetString("UserEmail", User.Email);
                HttpContext.Session.SetString("Admin", User.IsAdmin.ToString());
                //Userrolle anzeigen ob seller oder normaluser
                HttpContext.Session.SetString("UserRole", User.Role.ToString());

                SetSuccessMessage("Sie haben sich erfolgreich angemeldet.");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                SetErrorMessage(ErrorMessage);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear(); // Das Sessionobjekt wird geleert
            SetSuccessMessage("Sie haben sich erfolgreich abemeldet.");
            return RedirectToAction("Index", "Home"); // Zurück zur Startseite
        }


    }
}
