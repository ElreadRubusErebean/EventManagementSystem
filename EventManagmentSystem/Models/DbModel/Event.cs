﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using EventManagmentSystem.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventManagmentSystem.Models.DbModel;

public class Event
{
    [Key]
    public int EventId { get; set; }

    [Required(ErrorMessage = "Please enter a title.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Please enter a description.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Please enter a date.")]
    [DisplayFormat(DataFormatString = "dd.MM.yyyy H:mm")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Please enter a price.")]
    [DataType(DataType.Currency)]
    [Precision(10,2)]
    public decimal Price { get; set; } //keine Unterscheidung für Plätze 

    [DefaultValue(EventStateEnum.ForSale)]
    public EventStateEnum State { get; set; } = EventStateEnum.ForSale;

    [Required(ErrorMessage = "Please select an amount of tickets.")]
    public int AmountOfTickets { get; set; } //Gesamtanzahl

    [DefaultValue(0)]
    public int NumberOfSoldTickets { get; set; }

    // Navigationseigenschaft zu User
    public User User { get; set; }
    public int UserId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; }

    public Event()
    {
        Bookings = new HashSet<Booking>();
    }


}


