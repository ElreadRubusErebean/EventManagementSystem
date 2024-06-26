﻿using System.ComponentModel.DataAnnotations;
using EventManagmentSystem.Enums;

namespace EventManagmentSystem.Models.DbModel
{
    public class User
    {

        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter User Name...")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter First Name...")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name...")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Password...")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Email...")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]

        public string Email { get; set; }

        public bool IsAdmin { get; set; } = false;

        //rolle des Users definieren enweder NormUser oder Seller
        public UserRole Role { get; set; } = UserRole.NormalUser; //default ist NormalUser

        public string Salt { get; set; }

        public ICollection<Event> Events { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public User()
        {
            Events = new HashSet<Event>();
            Bookings = new HashSet<Booking>();
        }
    }
}
