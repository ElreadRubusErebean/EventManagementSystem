using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;
using EventManagmentSystem.Models.ViewModel;
using System.Security.Claims;

namespace EventManagmentSystem.Controllers
{
    public class BuyerController : Controller
    {
        private readonly UserService _userService;

        public BuyerController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Benutzer-ID 
            var userIdString = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIdString, out var userId))
            {
                //wenn die Benutzer-ID nicht vorhanden ist, wird der Benutzer auf die Startseite umgeleitet
                return NotFound();
            }

            // Benutzerdaten asynchron mit der Benutzer-ID abrufen
            var user = await _userService.GetUserAsync(userId);
            if (user == null)
            {
                // Benutzer nicht gefunden
                return NotFound();
            }

            // Modell für die View vorbereiten
            var model = new UserViewModel
            {
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return View(model);
        }

        //Methode zum löschen des Benutzerkontos
        //Ich verwende hier die Mthode DeleteUserAsync() aus dem UserService
        //vorher muss aber festgestellt dass der Benutzer der angemeldet ist, der Benutzer ist, der sein Konto löschen möchte
        [HttpPost]
        public async Task<IActionResult> DeleteKonto()
        {
            // Benutzer-ID 
            var userIdString = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIdString, out var userId))
            {
                //wenn die Benutzer-ID nicht vorhanden ist, wird der Benutzer auf die Startseite umgeleitet
                return NotFound();
            }

            // Benutzerdaten asynchron mit der Benutzer-ID abrufen
            var user = await _userService.GetUserAsync(userId);
            if (user == null)
            {
                // Benutzer nicht gefunden
                return NotFound();
            }

            // Benutzer löschen
            var success = await _userService.DeleteUserAsync(userId);
            if (!success)
            {
                // Benutzer konnte nicht gelöscht werden
                TempData["ErrorMessage"] = "Benutzer konnte nicht gelöscht werden";
            }

            // Benutzer abmelden
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
