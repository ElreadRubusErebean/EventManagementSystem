﻿using EventManagmentSystem.Services;
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
        public IActionResult Seller()
        {
            return View();
        }

        public IActionResult Seller_EventOverview()
        {
            return View();
         }

        //Konto löschen
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

            // Benutzerdaten asynchron mit der Benutzer-ID abrufens
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
