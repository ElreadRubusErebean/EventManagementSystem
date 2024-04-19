using System.Text.RegularExpressions;
using EventManagmentSystem.Models.DbModel;
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
            //Der ViewBag muss gesendet werden, um einen möglichen Error zu vermeiden
            ViewBag.IbanError = false;
            ViewBag.CardOwnerError = false;
            ViewBag.ExpireDateError = false;
            ViewBag.SafetyNumberError = false;

            return View();
        }

        [HttpPost]
        public IActionResult Payment(PaymentViewModel model)
        {
            PaymentCheckerService paymentCheckerService = new PaymentCheckerService();
            bool isErrorInInput = false;

            ViewBag.IbanError = false;
            ViewBag.CardOwnerError = false;
            ViewBag.ExpireDateError = false;
            ViewBag.SafetyNumberError = false;

            // Aufruf der Checks für die Korrektheit der Daten => bei Fehler werden Fehlernachrichten erstellt
            if (!paymentCheckerService.CheckThatIbanIsInLegitimateForm(model.IBAN))
            {
                SetErrorMessageWithMessageType("IbanMessage", "Die Iban entspricht nicht den gültigen Standarts!");
                ViewBag.IbanError = true;
                isErrorInInput = true;
            }

            if (!paymentCheckerService.CheckThatCardOwnerIsInLegitimateForm(model.CardOwner))
            {
                SetErrorMessageWithMessageType("CardOwnerMessage", "Sie müssen Vor- und Nachname eingeben!");
                ViewBag.CardOwnerError = true;
                isErrorInInput = true;
            }

            if (!paymentCheckerService.CheckExpireDate(model.ExpiredDateTime))
            {
                SetErrorMessageWithMessageType("ExpireDateMessage", 
                    "Das angegebene Ablaufdatum ist entweder ein unmögliches Datum oder bereits verstrichen!");
                ViewBag.ExpireDateError = true;
                isErrorInInput = true;
            }

            if (!paymentCheckerService.CheckThatSafetyNumberIsInLegitimateForm(model.SecurityNumber))
            {
                SetErrorMessageWithMessageType("SafetyNumberMessage", 
                    "Ihre Sicherheitsnummer muss drei Stellen haben mit jeweils Zahlen zwischen 0 - 9!");
                ViewBag.SafetyNumberError = true;
                isErrorInInput = true;
            }

            // Wenn eine Eingabe nicht korrekt war
            if (isErrorInInput)
            {
                SetErrorMessage("In Ihren Angaben befinden sich mögliche Fehler. Überprüfen Sie diese noch einmal!");
                return View();
            }

            // Die Eingabe ist korrekt
            SetSuccessMessage("Ihre Buchung war erfolgreich. Danke für Ihren Einkauf");
            return RedirectToAction("Event", "Event");
        }
    }
}