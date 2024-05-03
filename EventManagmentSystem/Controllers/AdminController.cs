using EventManagmentSystem.Models.DbModel;
using EventManagmentSystem.Models.ViewModel;
using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class AdminController : ValidationController
    {
        private readonly UserService _userService;

        public AdminController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            //check ob der user admin ist
            //ich verwende hier die Methode IsAdmin() aus dem UserService
            if (!_userService.IsAdmin())
            {
                TempData["ErrorMessage"] = "Sie haben nicht die erforderlichen Berechtigungen";
                return RedirectToAction("Index", "Home");
            }
            //wenn der User Admin ist, dann alle User anzeigen
            //die methode GetAllUsersAsync() ist asynchron und ist in UserService definiert
            var users = await _userService.GetAllUsersAsync();
            if (users == null)//
            {
                // wenn keine Benutzer abgerufen werden können
                users = new List<User>();
            }
            var model = new AdminViewModel
                {
                    Users = users
                };
                return View(model);
        }
        //Methode zum löschen eines Benutzers
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            // ist angemeldete Benutzer der Administrator ist
            if (!_userService.IsAdmin())
            {
                SetErrorMessage("Sie haben nicht die erforderlichen Berechtigungen");
                return RedirectToAction("Index", "Home");
            }

            // ob der Administrator versucht, sich selbst zu löschen
            var loggedInUserIdString = HttpContext.Session.GetString("UserID");
            if (int.TryParse(loggedInUserIdString, out var loggedInUserId) && loggedInUserId == userId)
            {
                SetErrorMessage("Sie können sich nicht selbst löschen");
                return RedirectToAction("Admin","Admin");
            }

            // Löschen des Benutzers
            var success = await _userService.DeleteUserAsync(userId);
            if (!success)
            {
                SetErrorMessage("Benutzer konnte nicht gelöscht werden");
            }
            return RedirectToAction("Admin", "Admin");
        }


        public IActionResult Admin_EventOverview()
        {
            return View();
        }

        public IActionResult Admin_UserOverview()
        {
            return View();
        }
    }
}