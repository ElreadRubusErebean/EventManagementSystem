using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EventManagmentSystem.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
public sealed class IbanAttribute : DataTypeAttribute
{
    public IbanAttribute() : base(DataType.MultilineText)
    {

    }

    public override bool IsValid(object? value)
    {
        string iban = Regex.Replace(value.ToString(), "", " ");
        List<char> characterList = iban.ToCharArray().ToList();

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

        return true;
    }
}