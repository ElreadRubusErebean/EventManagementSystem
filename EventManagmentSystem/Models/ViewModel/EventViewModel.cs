using EventManagmentSystem.Enums;

namespace EventManagmentSystem.Models.ViewModel
{
    public class EventViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; } //keine Unterscheidung für Plätze 

        public EventStateEnum State { get; set; }

        public int AmountOfTickets { get; set; } //Gesamtanzahl

    }
}
