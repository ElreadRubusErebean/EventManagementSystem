using System.ComponentModel.DataAnnotations;
using EventManagmentSystem.Enums;

namespace EventManagmentSystem.Models;

public class Event
{
    [Key]
    public int EventId { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime Date { get; set; }

    public double Price { get; set; } //keine Unterscheidung für Plätze 

    public EventStateEnum State { get; set; }

    public int AmountOfTickets { get; set; } //Gesamtanzahl
    
    public int NumberOfSoldTickets { get; set; }
}


