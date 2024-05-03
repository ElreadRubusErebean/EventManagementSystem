using EventManagmentSystem.Models.ViewModel;
using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{

    public class PaymentController : ValidationController
    {
        //ich verwende hier BookingService, um die Buchung zu speichern
        //nachdem die Zahlung erfolgreich war
        private readonly BookingService _bookingService;

        //Konstruktor, damit BookingService verwendet werden kann
        public PaymentController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public IActionResult Payment(int eventId, int numberOfTickets)
        {
            var viewModel = new PaymentViewModel
            {
                EventId = eventId,
                NumberOfTickets = numberOfTickets
            };
           
            //Der ViewBag muss gesendet werden, um einen möglichen Error zu vermeiden
            ViewBag.IbanError = false;
            ViewBag.CardOwnerError = false;
            ViewBag.ExpireDateError = false;
            ViewBag.SafetyNumberError = false;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Payment(PaymentViewModel model, int eventId, int numberOfTickets)
        {
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

                if (isErrorInInput)
                {
                    SetErrorMessage("In Ihren Angaben befinden sich mögliche Fehler. Überprüfen Sie diese noch einmal!");
                    return View(model);
                }

                // Buchung durchführen, nachdem die Zahlungsinformationen validiert wurden
                //To Do: userId aus Session holen externe Methode verwenden und nicht hier damit es wiederverwendbar ist
                var userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                bool bookingSuccess = await _bookingService.CreateBooking(eventId, userId, numberOfTickets);
                if (bookingSuccess)
                {
                    SetSuccessMessage("Ihre Buchung war erfolgreich. Danke für Ihren Einkauf!");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    SetErrorMessage("Buchung fehlgeschlagen. Bitte versuchen Sie es erneut.");
                    return View(model);
                }
            }
        }
    }
}