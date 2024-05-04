using EventManagmentSystem.Enums;

namespace EventManagmentSystem.Models.ViewModel
{
    public class EventViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; } //keine Unterscheidung für Plätze 

        
        /// <summary>
        /// in der Getter und Setter-Methode wird geprüft, ob das Event abgelaufen ist,
        /// wenn ja wird das Event als abgelaufen gesetzt,
        /// ob die Tickets ausverkauft sind,
        /// wenn ja wird das Event als ausverkauft gesetzt,
        /// ansonsten ist das Event zu verkaufen
        /// </summary>
        private EventStateEnum state;
        public EventStateEnum State
        {
            get
            {
                State = EventStateEnum.ForSale;
                
                if (AmountOfTickets<=0)
                {
                    State = EventStateEnum.SoldOut;
                }
                
                if (Date.CompareTo(DateTime.Now)<0)
                {
                    State = EventStateEnum.OutOfDate;
                }
                
                return state;
            }
            set
            {
                state = value;
                
                if (AmountOfTickets<=0)
                {
                    state = EventStateEnum.SoldOut;
                }
                
                if (Date.CompareTo(DateTime.Now)<0)
                {
                    state = EventStateEnum.OutOfDate;
                }
            } }

        public int AmountOfTickets { get; set; } //Gesamtanzahl
        public int EventId { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj==null|!(obj is EventViewModel))
            {
                return false;
            }

            var objEvent = (EventViewModel)obj;
            if (objEvent.EventId==this.EventId
                &&objEvent.Title==this.Title
                &&objEvent.Description==this.Description
                &&objEvent.Date==this.Date
                &&objEvent.Price==this.Price
                &&objEvent.AmountOfTickets==this.AmountOfTickets
                &&objEvent.State==this.State)
            {
                return true;
            }

            return false;
        }
    }
}
