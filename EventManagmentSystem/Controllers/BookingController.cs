using EventManagmentSystem.Models.ViewModel;
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
        public async Task<IActionResult> UserBookings()
        {
            var userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            var bookingsWithDetails = await _bookingService.GetUserBookingsAsync(userId);

            var viewModel = new UserBookingsViewModel
            {
                Bookings = bookingsWithDetails
            };

            return View(viewModel);
        }

        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            if (bookingId == 0)
            {
                SetErrorMessage("Ungültige Buchungs-ID.");
                return RedirectToAction("Index", "Home");
            }

            // Aufruf der Stornierungsmethode aus dem Service
            bool isCancelled = await _bookingService.Cancel(bookingId);
            if (!isCancelled)
            {
                SetErrorMessage("Buchung konnte nicht storniert werden, da das Event bereits stattgefunden hat oder die Buchung nicht gefunden wurde.");
                return RedirectToAction("Index", "Home");
            }

            // Weiterleitung oder Aktualisierung der Ansicht
            SetSuccessMessage("Buchung wurde erfolgreich storniert");
            return RedirectToAction("Index", "Home");
        }






    }
}
