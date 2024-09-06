using FlashCards.Data.Daos.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using System.Globalization;

namespace FlashCards.Helpers;

internal class ConsoleHelper
{
    private readonly IServiceProvider _serviceProvider;

    public ConsoleHelper(
        IServiceProvider serviceProvider,
        IStackDAO stackDAO
    )
    {
        _serviceProvider = serviceProvider;
    }

    internal string GetOption(string menuTitle, List<string> menuChoices, string? menuSubtitle = null)
    {
        FlashCardsHelper flashCardsHelper = _serviceProvider.GetRequiredService<FlashCardsHelper>();

        string option = ShowMenu(flashCardsHelper.CurrentUser!, menuChoices, menuTitle, menuSubtitle);

        while (option == null || option.Trim() == "")
        {
            ClearWindow();
            GetOption(menuTitle, menuChoices);
        }

        return option;
    }

    internal string ShowMenu(string username, List<string> menuChoices, string menuTitle, string? menuSubTitle = null)
    {
        ShowMessage($"FlashCards - [steelblue1] {username} [/] - [underline darkslategray2]{menuTitle}[/] {menuSubTitle ?? ""}", true, true, false);
        ShowMessage("");

        return GetChoice(menuChoices, "Choose an option bellow");
    }

    internal void ClearWindow()
    {
        Console.Clear();
        AnsiConsole.Clear();
        Console.Clear();
    }

    internal void ShowMessage(
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
                .Color(Color.SlateBlue1));
            }
            else
            {

                AnsiConsole.Write(
                new FigletText(message)
                    .LeftJustified()
                    .Color(Color.SlateBlue1));
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

    internal string? GetText(
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

    internal int? GetInteger(
        string message,
        int? defaultValue = null,
        bool canCancel = false,
        bool shouldValidate = false,
        int minValue = 1,
        int maxValue = 255
    )
    {
        ConsoleKey key;
        string input = defaultValue != null ? defaultValue.Value.ToString() : string.Empty;
        bool continueReading = true;

        if (canCancel)
        {
            // Inform the user about cancellation option
            AnsiConsole.MarkupLine("[grey](Press Esc to cancel)[/]");
        }

        if (shouldValidate)
        {
            AnsiConsole.MarkupLine($"\n[red](Input must have a minimum value of {minValue} and a maximum value of {maxValue})[/]");
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
            else if (key != ConsoleKey.Enter && key != ConsoleKey.Backspace && new List<ConsoleKey>([
                ConsoleKey.NumPad0,
                ConsoleKey.NumPad1,
                ConsoleKey.NumPad2,
                ConsoleKey.NumPad3,
                ConsoleKey.NumPad4,
                ConsoleKey.NumPad5,
                ConsoleKey.NumPad6,
                ConsoleKey.NumPad7,
                ConsoleKey.NumPad8,
                ConsoleKey.NumPad9,
                ConsoleKey.D0,
                ConsoleKey.D1,
                ConsoleKey.D2,
                ConsoleKey.D3,
                ConsoleKey.D4,
                ConsoleKey.D5,
                ConsoleKey.D6,
                ConsoleKey.D7,
                ConsoleKey.D8,
                ConsoleKey.D9,
                ]).Contains(key))
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

                    string? validationMessage = ValidationHelper.ValidateRequiredInteger(input, minValue, maxValue);

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

        return int.Parse(input);
    }

    internal string GetChoice(
        List<string> choices,
        string title,
        int pageSize = 10,
        string moreChoicesText = "[grey](Move up and down to reveal more options)[/]")
    {
        var highlightStyle = new Style().Foreground(Color.SlateBlue1);

        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .PageSize(pageSize)
                .HighlightStyle(highlightStyle)
                .MoreChoicesText(moreChoicesText)
                .AddChoices(choices));
    }

    internal void PressAnyKeyToContinue(string? message = null)
    {
        ShowMessage("");

        if (message != null)
        {
            ShowMessage(message);
        }

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

    internal void ShowTitle(string message, bool mustClearWindow = true)
    {
        ShowMessage($"FlashCards - [underline slateblue1]{message}[/]", true, mustClearWindow, false);
        ShowMessage("");
    }
}
