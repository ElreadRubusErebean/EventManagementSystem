namespace EventManagmentSystem.Models.ViewModel;

public class PaymentViewModel
{
    public string IBAN { get; set; }
    public string CardOwner { get; set; }
    public DateTime ExpiredDateTime { get; set; }
    public int SecurityNumber { get; set; }

    // Zusätzliche Felder für die Buchungsdetails
    public int EventId { get; set; }
    public string UserId { get; set; }
    public int NumberOfTickets { get; set; }
}