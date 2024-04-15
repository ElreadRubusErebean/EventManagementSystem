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

            // ToDo: Hier muss noch Code rein
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

            // ToDo: Hier muss noch der Sende-Aufruf hin
            return View(model);
        }
    }
}
