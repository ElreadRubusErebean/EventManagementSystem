using EventManagmentSystem.DAL;
using EventManagmentSystem.Enums;
using EventManagmentSystem.Models;
using EventManagmentSystem.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace EventManagmentSystem.Services
{
    public class BookingService
    {
        private readonly EventDbContext _context;

        public BookingService(EventDbContext context)
        {
            _context = context;
        }

        //create Booking
        /*
         * freischalten nur wenn AmountOfTickets nicht gleich 0
         * check, ob die Anzahl der Tickets die der User kaufen will, kleiner oder gleich AmountOfTickets
         * EventId und UserId holen
         * Anzahl der Tickets von AmountOfTickets abziehen
         * Booking speichern mit EventId, UserId und AmountOfTickets und BookingDate
         * Bestätigungsmail an User senden
         */
        public async Task<bool> CreateBooking(int eventId, int userId, int amountOfTickets)
        {
            //ToDo: Payment Methode überprüfen

            //freischalten nur wenn AmountOfTickets nicht gleich 0
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.EventId == eventId);
            if (@event == null || @event.AmountOfTickets < amountOfTickets)
            {
                return false;
            }

            // Überprüfen, ob der Benutzer eine normale Benutzerrolle hat
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId && u.Role == UserRole.NormalUser);
            if (user == null)
            {
                return false;
            }

            //Booking speichern
            var booking = new Booking
            {
                EventId = eventId,
                UserId = userId,
                AmountOfTickets = amountOfTickets,
                BookingDate = DateTime.Now
            };
            _context.Bookings.Add(booking);

            // Anzahl der verfügbaren Tickets aktualisieren
            @event.AmountOfTickets -= amountOfTickets;
            await _context.SaveChangesAsync();
            return true;
        }
        //canceled Booking
        /*
         * BookingId holen
         * check, ob Datum des Events noch nicht vorbei ist
         * Anzahl der Tickets zu AmountOfTickets addieren
         * Booking löschen
         * Bestätigungsmail an User senden
         */

        public void CancelBooking()
        {

        }

        //Buchungen anzeigen für User
        /*
         * UserId holen
         * alle Buchungen für den User holen
         * Buchungen zurückgeben
       */
        public async Task<List<BookingWithEventDetails>> GetUserBookingsAsync(int userId)
        {
            ///Erstmal Die Buchungen des Benutzers abrufen
            ///Dann die Event-Details für jede Buchung abrufen
            ///Die Include-Methode ermöglicht es, die Navigationseigenschaften der Buchung zu verwenden, um die Event-Daten abzurufen

            var bookingsWithEvents = await _context.Bookings
                .Include(b => b.Event)
                .Where(b => b.UserId == userId)
                ///Hier wird die Buchung in ein BookingWithEventDetails-Objekt umgewandelt, das sowohl die Buchung als auch die Event-Daten enthält
                .Select(b => new BookingWithEventDetails
                {
                    Booking = b,
                    /// Navigationsbeziehung nutzen, um Event-Daten einzubeziehen
                    Event = b.Event 
                })
                .ToListAsync();

            return bookingsWithEvents;
        }




    }
}
