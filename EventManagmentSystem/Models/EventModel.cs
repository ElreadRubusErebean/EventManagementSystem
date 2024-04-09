namespace EventManagmentSystem.Models;

public class EventModel
{
    //appsettings.json Namen ändern zu Servername bei SQL Managment dieses PCFEu14 oder so und neue Datenbank anlegen mit gleichem Namen
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime Date { get; set; }

    public int Price { get; set; } //keine Unterscheidung für Plätze 

    public EventState State { get; set; }

    public int NumberOfTickets { get; set; } //Gesamtanzahl

    public int NumberOfSoldTickets { get; set; }
}

public enum EventState
{   
    Sold, ForSale, OutOfDate
}
