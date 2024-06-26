﻿using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;
using EventManagmentSystem.Models.ViewModel;
using System.Security.Claims;

namespace EventManagmentSystem.Controllers
{
    public class BuyerController : ValidationController
    {
        private readonly UserService _userService;
        private readonly BookingService _bookingService;

        public BuyerController(UserService userService, BookingService bookingService)
        {
            _userService = userService;
            _bookingService = bookingService;
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
        //vorher muss aber festgestellt werden, dass der Benutzer der angemeldet ist auch der Benutzer ist, der sein Konto löschen möchte
        [HttpPost]
        public async Task<IActionResult> DeleteKonto()
        {
            //check ob user Booking hat oder nicht
            //wenn ja rufe cancelBooking() aus dem UserService auf
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
                SetErrorMessage("Benutzer konnte nicht gelöscht werden");
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

            return RedirectToAction("Profile");
        }

        public IActionResult Buyer_EventOverview()
        {
            return View();
        }
    }
}
