namespace EventManagmentSystem.Models.ViewModel;

public class PaymentViewModel
{
    public string IBAN { get; set; }
    public string CardOwner { get; set; }
    public DateTime ExpiredDateTime { get; set; }
    public int SecurityNumber { get; set; }

    public bool IsIbanError { get; set; }
}