using Spectre.Console;
using System.Globalization;

namespace FlashCards.Helpers;

internal abstract class ConsoleHelper
{
    internal static string ShowMainMenu(string username)
    {
        ShowMessage($"FlashCards - [blue] {username} [/] - [underline blue]Main Menu[/]", true, true, false);
        ShowMessage("");

        return GetChoice(FlashCardsHelper.GetMenuChoices(), "Choose an option bellow");
    }

    internal static void ClearWindow()
    {
        Console.Clear();
        AnsiConsole.Clear();
        Console.Clear();
    }

    internal static void ShowMessage(
        string message,
        bool breakLine = true,
        bool shouldClearWindow = false,
        bool figlet = false)
    {
        if (shouldClearWindow)
        {
            ClearWindow();
        }

        if (figlet)
        {
            if (breakLine)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Write(
            new FigletText(message)
                .LeftJustified()
                .Color(Color.Blue));
            }
            else
            {

                AnsiConsole.Write(
                new FigletText(message)
                    .LeftJustified()
                    .Color(Color.Blue));
            }
        }
        else
        {
            if (breakLine)
            {
                AnsiConsole.MarkupLine(message);
            }
            else
            {
                AnsiConsole.Markup(message);
            }
        }

    }

    internal static string? GetText(
        string message,
        string? defaultValue = null,
        bool canCancel = false,
        bool shouldValidate = false,
        int minLength = 1,
        int maxLength = 255
    )
    {
        //return AnsiConsole.Ask<string>(message);

        ConsoleKey key;
        string input = defaultValue ?? string.Empty;
        bool continueReading = true;

        if (canCancel)
        {
            // Inform the user about cancellation option
            AnsiConsole.MarkupLine("[grey](Press Esc to cancel)[/]");
        }

        if (shouldValidate)
        {
            AnsiConsole.MarkupLine($"\n[red](Input must have at least {minLength} characters and a maximum of {maxLength} characters)[/]");
        }

        // Begin the prompt
        AnsiConsole.Markup($"[bold]{message}[/] ");

        if (input.Length > 0)
        {
            Console.Write(input);
        }


        // Read user input, one key at a time
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            // Handle Escape key
            if (key == ConsoleKey.Escape && canCancel)
            {
                AnsiConsole.MarkupLine("\n[red]Input cancelled.[/]");
                return null;  // Or throw an exception if you want to handle it elsewhere
            }

            // Handle Backspace key
            if (key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input[..^1];
                Console.Write("\b \b");  // Erase the last character on the console
            }
            // Handle standard keys
            else if (key != ConsoleKey.Enter && key != ConsoleKey.Backspace)
            {
                input += keyInfo.KeyChar;
                Console.Write(keyInfo.KeyChar);  // Show the character
            }

            if (key == ConsoleKey.Enter)
            {
                bool valid = true;

                if (shouldValidate)
                {
                    valid = false;

                    string? validationMessage = ValidationHelper.ValidateRequiredText(input, 4);

                    if (validationMessage == null)
                    {
                        valid = true;
                    }
                }

                if (valid)
                {
                    continueReading = false;
                }
            }

        } while (continueReading);

        AnsiConsole.WriteLine(); // Move to the next line after Enter is pressed
        return input;
    }

    internal static string GetChoice(
        List<string> choices,
        string title,
        int pageSize = 10,
        string moreChoicesText = "[grey](Move up and down to reveal more options)[/]")
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .PageSize(pageSize)
                .MoreChoicesText(moreChoicesText)
                .AddChoices(choices));
    }

    internal static void PressAnyKeyToContinue()
    {
        ShowMessage("");
        ShowMessage("Press any key to continue");
        AnsiConsole.Console.Input.ReadKey(false);
    }

    internal static DateTime GetDateTime(string message = "Enter a date and time", DateTime? afterDateTime = null, DateTime? defaultValue = null)
    {
        TextPrompt<string> dateTimePrompt = new TextPrompt<string>($"{message} [grey](yyyy-MM-dd HH:mm:ss)[/]:")
            .DefaultValue(defaultValue != null
                ? defaultValue.Value.ToString("yyyy-MM-dd HH:mm:ss")
                : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .Validate(input =>
            {
                // Attempt to parse the input as a date and time in the specified format
                bool isValid = DateTime.TryParseExact(input,
                                                      "yyyy-MM-dd HH:mm:ss",
                                                      CultureInfo.InvariantCulture,
                                                      DateTimeStyles.None,
                                                      out DateTime result);

                bool isAfterDateTime = true;
                string afterDateTimeValidationMessage = string.Empty;

                if (isValid)
                {
                    if (afterDateTime != null)
                    {
                        isAfterDateTime = result.CompareTo(DateTime.Now) > 0;

                        if (!isAfterDateTime)
                        {
                            afterDateTimeValidationMessage = $"Date must be after {afterDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")}";
                        }
                    }
                }

                return isValid && isAfterDateTime
                    ? ValidationResult.Success()
                    : ValidationResult.Error($"[red]Invalid date and time format. {afterDateTimeValidationMessage}[/]");
            });

        // Prompt the user for input
        string dateTimeInput = AnsiConsole.Prompt(dateTimePrompt);

        // Parse the valid input to a DateTime object
        DateTime dateTime = DateTime.ParseExact(dateTimeInput,
                                                "yyyy-MM-dd HH:mm:ss",
                                                CultureInfo.InvariantCulture);

        return dateTime;
    }

    internal static void ShowTitle(string message)
    {
        ShowMessage($"FlashCards - [underline blue]{message}[/]", true, true, false);
        ShowMessage("");
    }
}
