using System.Text.RegularExpressions;
using EventManagmentSystem.Models.ViewModel;
using EventManagmentSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class PaymentController : Controller
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
                isErrorInInput = true;
            }

            if (!paymentCheckerService.CheckThatCardOwnerIsInLegitimateForm(model.CardOwner))
            {
                isErrorInInput = true;
            }

            if (!paymentCheckerService.CheckExpireDate(model.ExpiredDateTime))
            {
                isErrorInInput = true;
            }

            if (paymentCheckerService.CheckThatSafetyNumberIsInLegitimateForm(model.SecurityNumber))
            {
                isErrorInInput = true;
            }

            // Wenn eine Eingabe nicht korrekt war
            if (isErrorInInput)
            {
                return View(model);
            }

            // ToDo: Der Sende-Aufruf an die DB fehlt
            return View(model);
        }
    }
}