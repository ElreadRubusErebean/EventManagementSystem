using System.ComponentModel.DataAnnotations;
using EventManagmentSystem.Attributes;

namespace EventManagmentSystem.Models.ViewModel;

public class PaymentViewModel
{
    [Required(ErrorMessage = "Please enter your IBAN...")]
    [Iban(ErrorMessage = "Die IBAN ist fehlerhaft")]
    [Display(Name = "IBAN")]
    public string IBAN { get; set; }

    [Required(ErrorMessage = "Please enter the card owner...")]
    [Display(Name = "Kartenbesitzer")]
    public string cardOwner { get; set; }

    [Required(ErrorMessage = "Please enter the expiration date of your credit card...")]
    [Display(Name = "Ablaufdatum")]
    public DateTime ExpiredDateTime { get; set; }

    [Required(ErrorMessage = "Please enter the security number...")]
    [Display(Name = "Sicherheitsnummer")]
    public int securityNumber { get; set; }
}

// ToDo: Erstelle Attribute für alle weitere Elemente und überprüfe diese