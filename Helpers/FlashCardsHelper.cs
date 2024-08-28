using FlashCards.Menus;
using FlashCards.Menus.Interfaces;

namespace FlashCards.Helpers;

internal abstract class FlashCardsHelper
{
    internal static string? CurrentUser { get; set; }

    private static IMenu _currentMenu = new MainMenu();
    internal static IMenu CurrentMenu { get { return _currentMenu; } }

    internal static void Run()
    {
        while (CurrentUser == null)
        {
            CheckUser();
        }

        CurrentMenu.Run();
    }

    internal static void CheckUser()
    {
        if (CurrentUser == null)
        {
            AskName();
            ConsoleHelper.ClearWindow();
        }
    }

    internal static void AskName()
    {
        ConsoleHelper.ShowTitle("User Selection");

        string? name = ConsoleHelper.GetText("What is your [blue]name[/]? ");

        if (name != null && name.Trim().Length > 0)
        {
            CurrentUser = name;
        }
    }

    internal static void ChangeMenu(IMenu menu)
    {
        _currentMenu = menu;
        Run();
    }


}
