using FlashCards.Data.Daos;
using FlashCards.Data.Daos.Interfaces;
using FlashCards.Data.Dtos.Card;
using FlashCards.Data.Dtos.Stack;
using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FlashCards.Menus;

internal class ManageCardsMenu : IMenu
{
    private StackShowDTO? CurrentStack;
    private readonly IServiceProvider _serviceProvider;
    private readonly IStackDAO _stackDao;
    private readonly FlashCardsHelper _flashCardsHelper;
    private readonly ConsoleHelper _consoleHelper;



    public ManageCardsMenu(IServiceProvider serviceProvider, IStackDAO stackDAO, FlashCardsHelper flashCardsHelper, ConsoleHelper consoleHelper)
    {
        _serviceProvider = serviceProvider;
        _stackDao = stackDAO;
        _flashCardsHelper = flashCardsHelper;
        _consoleHelper = consoleHelper;
    }

    public void Run()
    {
        _consoleHelper.ClearWindow();

        while (CurrentStack == null)
        {
            SelectCurrentStack(true);
        }

        string option = _consoleHelper.GetOption($"Manage Cards", GetMenuChoices(),
            $" - [underline greenyellow] {CurrentStack!.Id} - {CurrentStack!.Name} [/]");

        RouteToOption(option.ElementAt(0));
    }

    void SelectCurrentStack(bool goBackOnWrongSelection = false)
    {
        StackShowDTO? selectedStackShowDTO = _flashCardsHelper.ShowStacksAndAskForId("Whats the stack ID to manage?");

        if (selectedStackShowDTO != null)
        {
            CurrentStack = selectedStackShowDTO;
        }
        else
        {
            if (goBackOnWrongSelection)
            {
                _consoleHelper.PressAnyKeyToContinue("No stack found, going back to main menu");
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
                MainMenu();
                break;

            default:
                Run();
                break;
        }
    }

    internal void MainMenu()
    {
        _flashCardsHelper.ChangeMenu(_serviceProvider.GetRequiredService<MainMenu>());
    }

    public List<string> GetMenuChoices()
    {
        return [
            "1 - [slateblue1]C[/]reate a Card",
            "2 - [slateblue1]L[/]ist All Cards",
            "3 - [slateblue1]U[/]pdate Card",
            "4 - [slateblue1]D[/]elete Card",
            "5 - [slateblue1]S[/]elect Stack",
            "6 - [slateblue1]M[/]ain Menu",
            ];
    }

    private void CreateCard()
    {
        _consoleHelper.ShowTitle("Create a Card");

        CardPromptDTO? cardPromptDTO = PromptUserForCardData();

        if (cardPromptDTO != null)
        {
            CardDao.StoreCardDapper(
                CardStoreDTO.FromPromptDTO(
                    CurrentStack!.Id,
                    cardPromptDTO
                )
            );

            _consoleHelper.ShowMessage("Card stored successfully!");
            _consoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            _consoleHelper.ShowMessage("No data was provided, operation canceled by user!");
            _consoleHelper.PressAnyKeyToContinue();
        }
    }

    private void UpdateCard()
    {
        _consoleHelper.ShowTitle("Update a card");

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

                _consoleHelper.ShowMessage(result ? "Card updated successfully!" : "Something went wrong :(");
                _consoleHelper.PressAnyKeyToContinue();
            }
            else
            {
                _consoleHelper.ShowMessage("No data was provided, operation canceled by user!");
                _consoleHelper.PressAnyKeyToContinue();
            }
        }
        else
        {
            _consoleHelper.ShowMessage("No card found.");
            _consoleHelper.PressAnyKeyToContinue();
        }
    }

    internal void PrintCard(CardShowDTO card)
    {
        _consoleHelper.ShowMessage($"{card.Sequence} - [dodgerblue1] Front: {card.Front}[/] - [springgreen3] Back: {card.Back}[/]");
    }

    private void ListCards(bool showPressAnyKeyToContinue = true, bool mustClearWindow = true)
    {
        _consoleHelper.ShowTitle("List of cards", mustClearWindow);

        List<CardShowDTO> cards = CardDao.GetAllCardsFromStack(CurrentStack!.Id);

        if (cards.Count() > 0)
        {
            foreach (CardShowDTO card in cards)
            {
                PrintCard(card);
            }

            if (showPressAnyKeyToContinue)
            {
                _consoleHelper.PressAnyKeyToContinue();
            }
        }
        else
        {
            if (showPressAnyKeyToContinue)
            {
                _consoleHelper.ShowMessage("No card found.");
                _consoleHelper.PressAnyKeyToContinue();
            }
        }

    }

    private void DeleteCard()
    {
        _consoleHelper.ShowTitle("Delete a card");

        CardShowDTO? selectedCardShowDTO = ShowCardsAndAskForSequence("Whats the card SEQUENCE to delete?");

        if (selectedCardShowDTO != null)
        {
            bool result = CardDao.DeleteCardById(selectedCardShowDTO.Id);

            _consoleHelper.ShowMessage(result ? "Card deleted successfully!" : "Something went wrong :(");
            _consoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            _consoleHelper.ShowMessage("No cards found.");
            _consoleHelper.PressAnyKeyToContinue();
        }
    }

    internal CardPromptDTO? PromptUserForCardData(CardShowDTO? defaultCardShowDTO = null)
    {
        string? front = _consoleHelper.GetText(
            "Whats the card front text?",
            defaultCardShowDTO != null ? defaultCardShowDTO.Front : null,
            true,
            true, 2
        );

        if (front != null)
        {
            string? back = _consoleHelper.GetText(
                "Whats the card back text?",
                defaultCardShowDTO != null ? defaultCardShowDTO.Back : null,
                true,
                true, 2
            );

            if (back != null)
            {
                int? lastSequenceFromStack = _stackDao.GetLastSequenceById(CurrentStack!.Id);
                int recommendedSequence = lastSequenceFromStack != null ? lastSequenceFromStack.Value + 1 : 1;
                int desiredSequence = 1;

                if (lastSequenceFromStack != null)
                {
                    ListCards(false, false);

                    int? promptSequence = _consoleHelper.GetInteger(
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

            _consoleHelper.ShowMessage("");

            int.TryParse(_consoleHelper.GetText(message), out int sequence);

            return cards.FirstOrDefault(stack => stack.Sequence == (sequence > 0 ? sequence : 0));
        }
    }
}
