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
}
