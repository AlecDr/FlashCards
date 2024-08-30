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
            case '2':
                ListCards();
                Run();
                break;
            case '3':
                UpdateCard();
                Run();
                break;
            case '4':
                DeleteCard();
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
            "2 - [blue]L[/]ist all cards",
            "3 - [blue]U[/]pdate card",
            "4 - [blue]D[/]elete card",
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
        ConsoleHelper.ShowTitle("Create a Card");

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

    private void UpdateCard()
    {
        ConsoleHelper.ShowTitle("Update a card");

        CardShowDTO? selectedCardShowDTO = ShowCardsAndAskForSequence("Whats the card sequence to update?");

        if (selectedCardShowDTO != null)
        {
            CardPromptDTO? cardPromptDTO = PromptUserForCardData(selectedCardShowDTO);

            if (cardPromptDTO != null)
            {
                bool result = CardDao.UpdateCard(
                   CardUpdateDTO.FromPromptDTO(
                       selectedCardShowDTO.Id,
                       CurrentStack!.Id,
                       cardPromptDTO
                   )
               );

                ConsoleHelper.ShowMessage(result ? "Card updated successfully!" : "Something went wrong :(");
                ConsoleHelper.PressAnyKeyToContinue();
            }
            else
            {
                ConsoleHelper.ShowMessage("No data was provided, operation canceled by user!");
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }
        else
        {
            ConsoleHelper.ShowMessage("No card found.");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    internal static void PrintCard(CardShowDTO card)
    {
        ConsoleHelper.ShowMessage($"{card.Sequence} - [dodgerblue1] Front: {card.Front}[/] - [springgreen3] Back: {card.Back}[/]");
    }

    private void ListCards(bool showPressAnyKeyToContinue = true, bool mustClearWindow = true)
    {
        ConsoleHelper.ShowTitle("List of cards", mustClearWindow);

        List<CardShowDTO> cards = CardDao.GetAllCardsFromStack(CurrentStack!.Id);

        if (cards.Count() > 0)
        {
            foreach (CardShowDTO card in cards)
            {
                PrintCard(card);
            }

            if (showPressAnyKeyToContinue)
            {
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }
        else
        {
            if (showPressAnyKeyToContinue)
            {
                ConsoleHelper.ShowMessage("No card found.");
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }

    }

    private void DeleteCard()
    {
        ConsoleHelper.ShowTitle("Delete a card");

        CardShowDTO? selectedCardShowDTO = ShowCardsAndAskForSequence("Whats the card SEQUENCE to delete?");

        if (selectedCardShowDTO != null)
        {
            bool result = CardDao.DeleteCard(selectedCardShowDTO.Id);

            ConsoleHelper.ShowMessage(result ? "Card deleted successfully!" : "Something went wrong :(");
            ConsoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            ConsoleHelper.ShowMessage("No cards found.");
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
                int recommendedSequence = lastSequenceFromStack != null ? lastSequenceFromStack.Value + 1 : 1;
                int desiredSequence = 1;

                if (lastSequenceFromStack != null)
                {
                    ListCards(false, false);

                    int? promptSequence = ConsoleHelper.GetInteger(
                           "Whats the card sequence?",
                           defaultCardShowDTO != null ? defaultCardShowDTO.Sequence : recommendedSequence,
                           true,
                           true,
                           1,
                           recommendedSequence
                       );

                    if (promptSequence != null)
                    {
                        desiredSequence = promptSequence.Value;
                    }
                }

                if (defaultCardShowDTO != null)
                {
                    if (desiredSequence != defaultCardShowDTO.Sequence)
                    {
                        if (desiredSequence < defaultCardShowDTO.Sequence)
                        {
                            CardDao.Add1ToAllSequencesStartingFrom(desiredSequence, CurrentStack!.Id, defaultCardShowDTO.Sequence);
                        }
                        else
                        {
                            CardDao.Subtract1ToAllSequencesStartingFrom(defaultCardShowDTO.Sequence, CurrentStack!.Id, desiredSequence);
                        }
                    }
                }
                else
                {
                    if (desiredSequence != recommendedSequence)
                    {
                        // in this case we add 1 to all cards with sequence equal or bigger than this
                        CardDao.Add1ToAllSequencesStartingFrom(desiredSequence, CurrentStack!.Id);
                    }
                }


                return new CardPromptDTO(front, back, desiredSequence);
            }
        }

        return null;
    }

    internal CardShowDTO? ShowCardsAndAskForSequence(string message)
    {
        List<CardShowDTO> cards = CardDao.GetAllCardsFromStack(CurrentStack!.Id);

        if (cards.Count <= 0)
        {
            return null;
        }
        else
        {
            foreach (CardShowDTO card in cards)
            {
                PrintCard(card);
            }

            ConsoleHelper.ShowMessage("");

            int.TryParse(ConsoleHelper.GetText(message), out int sequence);

            return cards.FirstOrDefault(stack => stack.Sequence == (sequence > 0 ? sequence : 0));
        }
    }
}
