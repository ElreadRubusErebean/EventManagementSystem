﻿using EventManagmentSystem.Enums;

namespace EventManagmentSystem.Models.ViewModel
{
    public class EventViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; } //keine Unterscheidung für Plätze 

        
        /// <summary>
        /// in der Getter-Methode wird bei Abfrage der State-Property geprüft, ob das Event abgelaufen ist,
        /// wenn ja wird das Event als abgelaufen gesetzt,
        /// wenn nein wird der bisherige State zuruückgegeben
        /// </summary>
        private EventStateEnum state;
        public EventStateEnum State
        {
            get
            {
                if (Date.CompareTo(DateTime.Now)>0)
                {
                    State = EventStateEnum.OutOfDate;
                }
                return state;
            }
            set
            {
                state = value;
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
