using EventManagmentSystem.Models.DbModel;

namespace EventManagmentSystem.Models.ViewModel
{
    public class UserBookingsViewModel
    {
        /// <summary>
        /// ich verwende hier eine Liste von BookingWithEventDetails, um die Buchungen und die zugehörigen Event-Details zu speichern
        /// </summary>
        public List<BookingWithEventDetails> Bookings { get; set; }
    }

    /// <summary>
    /// Diese Klasse enthält eine Buchung und die zugehörigen Event-Details
    /// </summary>
    public class BookingWithEventDetails
    {
        //Das ist die Buchung
        public Booking Booking { get; set; }
        //das ist das Event
        public Event Event { get; set; }
    }
}
