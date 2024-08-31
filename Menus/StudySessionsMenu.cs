using FlashCards.Dtos.Stack;
using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;

namespace FlashCards.Menus;

internal class StudySessionsMenu : IMenu
{
    private StackShowDTO? CurrentStack;

    public void Run()
    {
        ConsoleHelper.ClearWindow();

        while (CurrentStack == null)
        {
            SelectCurrentStack(true);
        }

        string option = ConsoleHelper.GetOption($"Study Sessions", GetMenuChoices(),
            $" - [underline greenyellow] {CurrentStack!.Id} - {CurrentStack!.Name} [/]");

        RouteToOption(option.ElementAt(0));
    }

    void SelectCurrentStack(bool goBackOnWrongSelection = false)
    {
        StackShowDTO? selectedStackShowDTO = ManageStacksMenu.ShowStacksAndAskForId("Whats the stack ID to manage?");

        if (selectedStackShowDTO != null)
        {
            CurrentStack = selectedStackShowDTO;
        }
        else
        {
            if (goBackOnWrongSelection)
            {
                ConsoleHelper.PressAnyKeyToContinue("No stack found, going back to main menu");
                MainMenu();
            }
            else
            {
                SelectCurrentStack(false);
            }
        }
    }

    public void RouteToOption(char option)
    {
        switch (option)
        {
            case '1':
                //StudyCard();
                Run();
                break;
            case '2':
                SelectCurrentStack();
                Run();
                break;
            case '3':
                MainMenu();
                break;

            default:
                Run();
                break;
        }
    }

    internal void MainMenu()
    {
        FlashCardsHelper.ChangeMenu(new MainMenu());
    }

    public List<string> GetMenuChoices()
    {
        return [
            "1 - [slateblue1]S[/]udy Card",
            "2 - [slateblue1]S[/]elect Stack",
            "3 - [slateblue1]M[/]ain Menu",
            ];
    }
}
