using System.Text.RegularExpressions;
using EventManagmentSystem.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EventManagmentSystem.Controllers
{
    public class PaymentController : Controller
    {
        // ToDo: Das hier soll in die Attribute und dann gelöscht werden
        /*public bool CheckPaymentData()
        {
            if (!CheckThatIBANIsInLegitimateForm("DE24100900002648730007"))
            {
                return false;
            }
            if (!CheckThatNameIsInLegitimateForm("Yannick Rammelt"))
            {
                return false;
            }
            if (!CheckExpireDate(DateTime.Now))
            {
                return false;
            }
            if (!CheckThatSafetyNumberIsInLegitimateForm())
            {
                return false;
            }

            return true;
        }

        private bool CheckThatNameIsInLegitimateForm(string name)
        {
            int numberOfBlanks = 0;

            foreach (var character in name.ToCharArray())
            {
                if (character >= '0' && character <= '9')
                {
                    return false;
                }

                if (character.Equals(" "))
                {
                    numberOfBlanks++;
                }
            }

            if (numberOfBlanks == 0)
            {
                return false;
            }

            return true;
        }

        private bool CheckExpireDate(DateTime expireDateTime)
        {
            if (expireDateTime.ToUniversalTime() <= DateTime.Now.ToUniversalTime())
            {
                return true;
            }

            return false;
        }

        private bool CheckThatSafetyNumberIsInLegitimateForm()
        {
            return true;
        }*/

        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Payment(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error");
                return View(model);
            }
        }
    }
}
