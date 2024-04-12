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
            // Benutzer-ID aus den Claims des aktuell angemeldeten Benutzers extrahieren
            var userIdString = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIdString, out var userId))
            {
                // Fehlgeschlagene Konvertierung oder Benutzer nicht gefunden; entsprechend handeln
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

    }
}
