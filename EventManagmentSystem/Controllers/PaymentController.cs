using System.Text.RegularExpressions;
using EventManagmentSystem.Models.ViewModel;
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
            bool isErrorInInput = false;

            // ToDo: Hier muss noch Code rein
            if (!CheckThatIbanIsInLegitimateForm(model.IBAN))
            {
                isErrorInInput = true;
            }

            if (!CheckThatCardOwnerIsInLegitimateForm(model.CardOwner))
            {
                isErrorInInput = true;
            }

            if (!CheckExpireDate(model.ExpiredDateTime))
            {
                isErrorInInput = true;
            }

            if (CheckThatSafetyNumberIsInLegitimateForm(model.SecurityNumber))
            {
                isErrorInInput = true;
            }

            if (isErrorInInput)
            {
                return View(model);
            }

            // ToDo: Hier muss noch der Sende-Aufruf hin
            return View(model);
        }


        public bool CheckThatIbanIsInLegitimateForm(string iban)
        {
            
            string computerUnderstandableIban = Regex.Replace(iban, " ", "");
            List<char> characterList = iban.ToCharArray().ToList();

            // Hat die Iban die korrekte Anzahl an Zeichen
            if (!characterList.Count.Equals(22))
            {
                return false;
            }

            // Sind die ersten zwei Zeichen der Iban Buchstaben
            if (!(char.IsLetter(characterList[0]) && char.IsLetter(characterList[1])))
            {
                return false;
            }

            // Sind alle Zeichen nach den ersten beiden Zahlen
            foreach (char character in characterList.Skip(2))
            {
                if (!(character >= '0' && character <= '9'))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckThatCardOwnerIsInLegitimateForm(string name)
        {
            // fängt keine Eingabe, als auch Eingaben, die von der minimalen Länge nicht korrekt sein können
            // Bsp.: "A B" => A = Vorname, B = Nachname => mit Leerzeichen drei Buchstaben nötig
            if (name.ToCharArray().ToList().Count < 3)
            {
                return false;
            }

            int numberOfBlanks = 0;

            foreach (var character in name.ToCharArray())
            {
                if (character.Equals(" "))
                {
                    numberOfBlanks++;
                    break;
                }

                // Zahlen und Sonderzeichen ausschließen
                if (!char.IsLetter(character))
                {
                    return false;
                }
            }

            // Check ob es Vor- und Nachname gibt
            if (numberOfBlanks <= 0)
            {
                return false;
            }

            return true;
        }

        private bool CheckExpireDate(DateTime expireDateTime)
        {
            // Ablaudatum vor der aktuellen Zeit? => dann illegitimes Ablaufdatum
            if (expireDateTime.ToUniversalTime() <= DateTime.Now.ToUniversalTime())
            {
                return true;
            }

            return false;
        }

        private bool CheckThatSafetyNumberIsInLegitimateForm(int securityNumber)
        {
            // Zahl Dreistellig und zwischen 0 und 999
            if (!(securityNumber < 000 || securityNumber > 999 || securityNumber.ToString().Length != 3 || securityNumber.Equals(null)))
            {
                return false;
            }

            return true;
        }
    }
}
