﻿using EventManagmentSystem.Enums;
using EventManagmentSystem.Models.ViewModel;
using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class SellerController : ValidationController
    {
        private readonly UserService _userService;
        private readonly EventService _eventService;

        public SellerController(UserService userService, EventService eventService)
        {
            _userService = userService;
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> Seller()
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

        public IActionResult Seller_EventOverview()
        {
            return View();
         }

        //Konto löschen seller
        [HttpPost]
        public async Task<IActionResult> DeleteKonto()
        {
            // Benutzer-ID 
            var userIdString = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIdString, out var userId))
            {
                // Wenn die Benutzer-ID nicht vorhanden ist, wird der Benutzer auf die Startseite umgeleitet
                return NotFound();
            }

            // Benutzerdaten asynchron mit der Benutzer-ID abrufen
            var user = await _userService.GetUserAsync(userId);
            if (user == null)
            {
                // Benutzer nicht gefunden
                //ToDo : Fehlermeldung
                return NotFound();
            }

            // Benutzer löschen - abhängig davon, ob es sich um einen Verkäufer handelt oder nicht
            bool success;
            if (user.Role == UserRole.Seller)
            {
                success = await _userService.DeleteSellerAccountAsync(userId);
            }
            else
            {
                success = await _userService.DeleteUserAsync(userId);
            }

            if (!success)
            {
                // Benutzer konnte nicht gelöscht werden
                SetErrorMessage("Benutzer konnte nicht gelöscht werden");
                return RedirectToAction("Index", "Home");
            }

            // Benutzer abmelden
            HttpContext.Session.Clear();
            SetSuccessMessage("Konto ist erfolgreich gelöscht");
            return RedirectToAction("Index", "Home");
        }


        // Methode zum Updaten des Benutzerkontos
        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            // Benutzer-ID 
            var userIdString = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIdString, out var userId))
            {
                // Wenn die Benutzer-ID nicht vorhanden ist, wird der Benutzer auf die Startseite umgeleitet
                return NotFound();
            }

            // Ich rufe hier die Methode GetUserAsync() aus dem UserService auf
            // Damit ich die Daten des Benutzers abrufen kann
            var user = await _userService.GetUserAsync(userId);
            if (user == null)
            {
                // Benutzer nicht gefunden
                return NotFound();
            }

            // Model für die View vorbereiten
            var model = new UserViewModel
            {
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserViewModel model)
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

            // Benutzerdaten aktualisieren
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            // Benutzerdaten speichern
            var success = await _userService.UpdateUserAsync(user);
            if (!success)
            {
                SetErrorMessage("Benutzer konnte nicht aktualisiert werden");
            }
            else
            {
                SetSuccessMessage("Benutzerdaten erfolgreich aktualisiert");
            }

            return RedirectToAction("Seller");
        }

        /// <summary>
        /// meine Events anzeigen
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> MyEvents()
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

            // Events des Benutzers abrufen
            var events = await _eventService.GetAllSellerEventsAsync(userId);
            return View(events);
        }   
    }
}
