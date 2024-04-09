using System.ComponentModel.DataAnnotations;

namespace EventManagmentSystem.Models;

public class Event
{
    [Key]
    public int EventId { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime Date { get; set; }

    public double Price { get; set; } //keine Unterscheidung für Plätze 

    public EventState State { get; set; }

    public int NumberOfTickets { get; set; } //Gesamtanzahl

    public int NumberOfSoldTickets { get; set; }
}

public enum EventState
{   
    Sold, ForSale, OutOfDate
}
