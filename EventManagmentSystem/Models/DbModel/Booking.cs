using EventManagmentSystem.Models.DbModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagmentSystem.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        // Beziehungen
        public int UserId { get; set; }
        public User User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        //anzahl gekauften Tickets
        public int AmountOfTickets { get; set; }

    }
}
