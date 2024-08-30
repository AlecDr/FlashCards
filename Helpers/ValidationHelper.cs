using System.Diagnostics;

namespace FlashCards.Helpers;

internal class ValidationHelper
{
    internal static string? ValidateRequiredText(string value, int? minLength = 1, int? maxLength = 255)
    {
        return
            value.Length >= minLength & value.Length <= maxLength
                ? null
                : $"This field must have at least {minLength} characters and a maximum of {maxLength} characters.";
    }

    internal static string? ValidateRequiredInteger(string value, int? minValue = 1, int? maxValue = 255)
    {
        bool parseableInteger = int.TryParse(value, out int result);
        string validationMessage = $"This field must have a minimum value of {minValue} and a maximum value of {maxValue}.";

        Debug.Print(validationMessage);

        return parseableInteger ?
            result >= minValue & result <= maxValue
                ? null
                : validationMessage
            : validationMessage;
    }
}
