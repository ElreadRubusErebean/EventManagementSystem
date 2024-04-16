using System.Text.RegularExpressions;
using EventManagmentSystem.Models.ViewModel;
using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class PaymentController : ValidationController
    {
        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Payment(PaymentViewModel model)
        {
            PaymentCheckerService paymentCheckerService = new PaymentCheckerService();
            bool isErrorInInput = false;

            // ToDo: Hier müssen die Anmerkungen für die View generiert werden

            if (!paymentCheckerService.CheckThatIbanIsInLegitimateForm(model.IBAN))
            {
                SetErrorMessageWithMessageType("IbanMessage", "Die Iban entspricht nicht den gültigen Standarts!");
                isErrorInInput = true;
            }

            if (!paymentCheckerService.CheckThatCardOwnerIsInLegitimateForm(model.CardOwner))
            {
                SetErrorMessageWithMessageType("CardOwnerMessage", "Sie müssen Vor- und Nachname eingeben!");
                isErrorInInput = true;
            }

            if (!paymentCheckerService.CheckExpireDate(model.ExpiredDateTime))
            {
                SetErrorMessageWithMessageType("ExpireDateMessage", "Das angegebene Ablaufdatum ist entweder ein unmögliches Datum oder bereits verstrichen!");
                isErrorInInput = true;
            }

            if (!paymentCheckerService.CheckThatSafetyNumberIsInLegitimateForm(model.SecurityNumber))
            {
                SetErrorMessageWithMessageType("SafetyNumberMessage", "Ihre Sicherheitsnummer muss drei Stellen haben mit jeweils Zahlen zwischen 0 - 9!");
                isErrorInInput = true;
            }

            // Wenn eine Eingabe nicht korrekt war
            if (isErrorInInput)
            {
                SetErrorMessage("In Ihren Angaben befinden sich mögliche Fehler. Überprüfen Sie diese noch einmal!");
                return View(model);
            }

            // Daten werden an den Server gesendet
            // ToDo: Der Sende-Aufruf an die DB fehlt
            if (!SendDataToServer())
            {
                SetErrorMessage("Die Daten konnten nicht an den Server gesendet werden");
            }

            return View(model);
        }

        public bool SendDataToServer()
        {

        }
    }
}