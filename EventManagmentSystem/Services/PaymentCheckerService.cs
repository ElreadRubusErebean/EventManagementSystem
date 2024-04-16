using System.Numerics;
using System.Text.RegularExpressions;

namespace EventManagmentSystem.Services
{
    public class PaymentCheckerService
    {
        public bool CheckThatIbanIsInLegitimateForm(string iban)
        {
            string computerUnderstandableIban = Regex.Replace(iban, " ", "");
            List<char> characterList = computerUnderstandableIban.ToCharArray().ToList();

            // Hat die Iban die korrekte Anzahl an Zeichen
            if (!characterList.Count.Equals(22))
            {
                return false;
            }

            if (!(char.IsLetter(characterList[0]) && char.IsLetter(characterList[1])))
            {
                return false;
            }

            foreach (char character in characterList.Skip(2))
            {
                if (!(character >= '0' && character <= '9'))
                {
                    return false;
                }
            }

            // Check That TestDigit is correct
            int checkDigit = 0;

            for (int index = 0; index <= 3; index++)
            {
                if (index <= 1)
                {
                    int valueInt = (int)(((characterList[index] % 32) - 1) + 10);
                    List<char> valueList = valueInt.ToString().ToCharArray().ToList();

                    foreach (var value in valueList)
                    {
                        characterList.Add(value);
                    }
                    continue;
                }

                if (index == 2)
                {
                    checkDigit += Convert.ToInt32(characterList[index].ToString()) * 10;
                    continue;
                }
                checkDigit += Convert.ToInt32(characterList[index].ToString());
            }

            for (int index = 0; index < 4; index++)
            {
                characterList.RemoveAt(0);
            }

            characterList.Add(Convert.ToChar("0"));
            characterList.Add(Convert.ToChar("0"));

            string resultString = new string(characterList.ToArray());
            BigInteger result = BigInteger.Parse(resultString);
            result = (result % 97);
            result = 98 - result;

            if (result != checkDigit)
            {
                return false;
            }

            return true;
        }

        public bool CheckThatCardOwnerIsInLegitimateForm(string name)
        {
            // Bsp.: "A B" => A = Vorname, B = Nachname => mit Leerzeichen drei Buchstaben nötig
            if (name.ToCharArray().ToList().Count < 3)
            {
                return false;
            }

            int numberOfBlanks = 0;

            foreach (var character in name.ToCharArray())
            {
                if (character.Equals(' '))
                {
                    numberOfBlanks++;
                    continue;
                }

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

        public bool CheckExpireDate(DateTime expireDateTime)
        {
            switch (expireDateTime.Month)
            {
                case 1 or 3 or 5 or 7 or 8 or 10 or 12:
                    if (expireDateTime.Day > 31)
                    {
                        return false;
                    }
                    break;
                case 4 or 6 or 9 or 11:
                    if (expireDateTime.Day > 30)
                    {
                        return false;
                    }
                    break;
                case 2:
                    if (expireDateTime.Day > 29)
                    {
                        return false;
                    }
                    break;
                default:
                    return false;
            }

            if (expireDateTime.ToUniversalTime() <= DateTime.Now.ToUniversalTime())
            {
                return false;
            }

            return true;
        }

        public bool CheckThatSafetyNumberIsInLegitimateForm(int securityNumber)
        {
            // Zahl ist dreistellig und liegt zwischen 0 und 999
            if (securityNumber < 000 || securityNumber > 999 || securityNumber.ToString().Length != 3 || securityNumber.Equals(null))
            {
                return false;
            }

            return true;
        }
    }
}
