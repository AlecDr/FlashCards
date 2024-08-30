using FlashCards.Daos;
using FlashCards.Dtos.Stack;
using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;

namespace FlashCards.Menus;

internal class MainMenu : IMenu
{
    public void Run()
    {
        ConsoleHelper.ClearWindow();

        string option = ConsoleHelper.GetOption("Main Menu", GetMenuChoices());

        RouteToOption(option.ElementAt(0));
    }

    public void RouteToOption(char option)
    {
        switch (option)
        {
            case '1':
                ManageStacks();
                break;
            case '2':
                ManageCards();
                break;
            case '3':
                FlashCardsHelper.AskName();
                Run();
                break;
            default:
                Run();
                break;
        }
    }

    public List<string> GetMenuChoices()
    {
        return [
        "1 - [slateblue1]M[/]anage Stacks",
        "2 - [slateblue1]M[/]anage Cards",
        "3 - [slateblue1]C[/]hange User",
        ];
    }

    private void ManageStacks()
    {
        List<StackShowDTO> stacks = StackDao.GetAllStacks(FlashCardsHelper.CurrentUser!);

        if (stacks.Count > 0)
        {
            FlashCardsHelper.ChangeMenu(new ManageStacksMenu());
        }
        else
        {
            ConsoleHelper.PressAnyKeyToContinue("You must create a stack before going to the manage stacks menu!");
            Run();
        }
    }

    private void ManageCards()
    {
        List<StackShowDTO> stacks = StackDao.GetAllStacks(FlashCardsHelper.CurrentUser!);

        if (stacks.Count > 0)
        {
            FlashCardsHelper.ChangeMenu(new ManageCardsMenu());
        }
        else
        {
            ConsoleHelper.PressAnyKeyToContinue("You must create a stack before going to the manage cards menu!");
            Run();
        }
    }

}
