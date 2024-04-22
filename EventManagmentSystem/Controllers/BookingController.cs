using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class BookingController : ValidationController
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> Book(int eventId, int numberOfTickets)
        {
            // Die Benutzer-ID aus der Sitzung abrufen oder aus einem anderen Ort, z. B. dem Anmeldeprozess
            var userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

            // Buchung durchführen
            bool success = await _bookingService.CreateBooking(eventId, userId, numberOfTickets);

            // Wenn die Buchung erfolgreich war
            if (success)
            {
                SetSuccessMessage("Booking successful.");
                return RedirectToAction("Index", "Home");
            }
            // Wenn die Buchung fehlgeschlagen ist
            else
            {
                SetErrorMessage("Booking failed.");
                return RedirectToAction("Index", "Home");
            }
        }


    }
}
