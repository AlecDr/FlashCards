using FlashCards.Daos;
using FlashCards.Dtos.Card;
using FlashCards.Dtos.Stack;
using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;

namespace FlashCards.Menus;

internal class ManageStacksMenu : IMenu
{
    private StackShowDTO? CurrentStack;

    public void Run()
    {
        ConsoleHelper.ClearWindow();

        while (CurrentStack == null)
        {
            SelectCurrentStack(true);
        }

        string option = GetOption();

        RouteToOption(option.ElementAt(0));
    }

    void SelectCurrentStack(bool goBackOnWrongSelection = false)
    {
        StackShowDTO? selectedStackShowDTO = MainMenu.ShowStacksAndAskForId("Whats the stack ID to manage?");

        if (selectedStackShowDTO != null)
        {
            CurrentStack = selectedStackShowDTO;
        }
        else
        {
            if (goBackOnWrongSelection)
            {
                ConsoleHelper.PressAnyKeyToContinue("No stack found, going back to main menu");
                GoBack();
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
                CreateCard();
                Run();
                break;

            case '5':
                SelectCurrentStack();
                Run();
                break;

            case '6':
                GoBack();
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
            "1 - [blue]C[/]reate a card",
            //"2 - [blue]L[/]ist all cards",
            //"3 - [blue]U[/]pdate card",
            //"4 - [blue]D[/]elete card",
            "5 - [blue]S[/]elect Stack",
            "6 - [blue]G[/]o back",
            ];
    }

    public string GetOption()
    {
        string option = ConsoleHelper.ShowMenu(
            FlashCardsHelper.CurrentUser!,
            GetMenuChoices(),
            $"Manage Stacks",
            $" - [underline greenyellow] {CurrentStack!.Id} - {CurrentStack!.Name} [/]");

        while (option == null || option.Trim() == "")
        {
            ConsoleHelper.ClearWindow();
            GetOption();
        }

        return option;
    }

    private void CreateCard()
    {
        ConsoleHelper.ShowTitle("Create an Card");

        CardPromptDTO? cardPromptDTO = PromptUserForCardData();

        if (cardPromptDTO != null)
        {
            CardDao.StoreCardDapper(
                CardStoreDTO.FromPromptDTO(
                    CurrentStack!.Id,
                    cardPromptDTO
                )
            );

            ConsoleHelper.ShowMessage("Card stored successfully!");
            ConsoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            ConsoleHelper.ShowMessage("No data was provided, operation canceled by user!");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    internal CardPromptDTO? PromptUserForCardData(CardShowDTO? defaultCardShowDTO = null)
    {
        string? front = ConsoleHelper.GetText(
            "Whats the card front text?",
            defaultCardShowDTO != null ? defaultCardShowDTO.Front : null,
            true,
            true, 2
        );

        if (front != null)
        {
            string? back = ConsoleHelper.GetText(
                "Whats the card back text?",
                defaultCardShowDTO != null ? defaultCardShowDTO.Back : null,
                true,
                true, 2
            );

            if (back != null)
            {
                int? lastSequenceFromStack = StackDao.GetLastSequenceFromStack(CurrentStack!.Id);
                int sequence = 1;

                if (lastSequenceFromStack != null)
                {
                    sequence = lastSequenceFromStack.Value + 1;
                }

                return new CardPromptDTO(front, back, sequence);
            }
        }

        return null;
    }
}
