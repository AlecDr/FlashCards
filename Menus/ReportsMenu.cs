using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;

namespace FlashCards.Menus;

internal class ReportsMenu : IMenu
{
    public void Run()
    {
        ConsoleHelper.ClearWindow();

        string option = ConsoleHelper.GetOption("Reports Menu", GetMenuChoices());

        RouteToOption(option.ElementAt(0));
    }

    public void RouteToOption(char option)
    {
        switch (option)
        {
            case '1':
                ResumedStudySessions();
                Run();
                break;
            case '2':
                MainMenu();
                break;

            default:
                Run();
                break;
        }
    }

    public List<string> GetMenuChoices()
    {
        return [
        "1 - [slateblue1]R[/]esumed Study Sessions",
        "2 - [slateblue1]M[/]ain Menu",
        ];
    }

    private void MainMenu()
    {
        FlashCardsHelper.ChangeMenu(new MainMenu());
    }

    private void ResumedStudySessions()
    {
        ConsoleHelper.PressAnyKeyToContinue("Resumed Study Sessions Report");
    }

}
