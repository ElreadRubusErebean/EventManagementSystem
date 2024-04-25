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




    }
}
