using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;

namespace FlashCards.Menus;

internal class ManageStacksMenu : IMenu
{
    public void Run()
    {
        ConsoleHelper.ClearWindow();

        string option = GetOption();

        RouteToOption(option.ElementAt(0));
    }

    public void RouteToOption(char option)
    {
        switch (option)
        {

            case '5':
                GoBack();
                Run();
                break;

            default:
                Run();
                break;
        }
    }

    internal void GoBack()
    {
        FlashCardsHelper.ChangeMenu(new MainMenu());
    }

    public List<string> GetMenuChoices()
    {
        return [
            //"1 - [blue]C[/]reate a card",
            //"2 - [blue]L[/]ist all cards",
            //"3 - [blue]U[/]pdate card",
            //"4 - [blue]D[/]elete card",
            "5 - [blue]G[/]o back",
            ];
    }

    public string GetOption()
    {
        string option = ConsoleHelper.ShowMenu(FlashCardsHelper.CurrentUser!, GetMenuChoices(), "Manage Stacks");

        while (option == null || option.Trim() == "")
        {
            ConsoleHelper.ClearWindow();
            GetOption();
        }

        return option;
    }



}
