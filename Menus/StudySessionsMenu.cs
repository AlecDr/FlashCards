using FlashCards.Data.Daos;
using FlashCards.Data.Daos.Interfaces;
using FlashCards.Data.Dtos.Card;
using FlashCards.Data.Dtos.Stack;
using FlashCards.Data.Dtos.StudySession;
using FlashCards.Data.Dtos.StudySessionAnswer;
using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace FlashCards.Menus;

internal class StudySessionsMenu : IMenu
{
    private StackShowDTO? CurrentStack;
    private readonly IServiceProvider _serviceProvider;
    public StudySessionsMenu(IServiceProvider serviceProvider, IStackDAO stackDAO)
    {
        _serviceProvider = serviceProvider;
    }

    public void Run()
    {
        _serviceProvider.GetRequiredService<ConsoleHelper>().ClearWindow();

        while (CurrentStack == null)
        {
            SelectCurrentStack(true);
        }

        string option = _serviceProvider.GetRequiredService<ConsoleHelper>().GetOption($"Study Sessions", GetMenuChoices(),
            $" - [underline greenyellow] {CurrentStack!.Id} - {CurrentStack!.Name} [/]");

        RouteToOption(option.ElementAt(0));
    }

    void SelectCurrentStack(bool goBackOnWrongSelection = false)
    {
        StackShowDTO? selectedStackShowDTO = _serviceProvider.GetRequiredService<FlashCardsHelper>().ShowStacksAndAskForId("Whats the stack ID to study?");

        if (selectedStackShowDTO != null)
        {
            CurrentStack = selectedStackShowDTO;
        }
        else
        {
            if (goBackOnWrongSelection)
            {
                _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("No stack found, going back to main menu");
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
                StudyStack();
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
        _serviceProvider.GetRequiredService<FlashCardsHelper>().ChangeMenu(_serviceProvider.GetRequiredService<MainMenu>());
    }

    public List<string> GetMenuChoices()
    {
        return [
            "1 - [slateblue1]S[/]tudy Stack",
            "2 - [slateblue1]S[/]elect Stack",
            "3 - [slateblue1]M[/]ain Menu",
            ];
    }

    private void ShowCardAsPanel(CardShowDTO card, bool showFront = true)
    {
        AnsiConsole.Write(
            new Panel(new Text(showFront ? card.Front : card.Back))
                .RoundedBorder()
                .Header($"[blue]{(showFront ? "FRONT" : "BACK")}[/]")
                .HeaderAlignment(Justify.Center));
    }

    private void StudyStack()
    {
        List<CardShowDTO> cards = CardDao.GetAllCardsFromStack(CurrentStack!.Id);
        if (cards.Count > 0)
        {
            // start study session
            StudySessionShowDTO? studySession = StudySessionDao.StoreStudySession(new StudySessionStoreDTO(CurrentStack!.Id, DateTime.Now));
            List<StudySessionAnswerPromptDTO> answeredCards = [];

            if (studySession != null)
            {

                for (global::System.Int32 i = 0; i < cards.Count; i++)
                {
                    CardShowDTO card = cards[i];

                    // for each card, show it and ask for the answer
                    StudySessionAnswerPromptDTO? studySessionAnswerPromptDTO = ShowCardAndAskForAnswer(card);

                    if (studySessionAnswerPromptDTO != null)
                    {
                        answeredCards.Add(studySessionAnswerPromptDTO);
                        StudySessionAnswerDao.StoreStudySessionAnswer(StudySessionAnswerStoreDTO.FromPromptDTO(card.Id, studySession.Id, studySessionAnswerPromptDTO));
                    }
                    else
                    {
                        // if the user refused to answer, we finish this study session
                        i = cards.Count;
                    }
                }

                // update the study session to finish it
                StudySessionDao.UpdateStudySession(new StudySessionUpdateDTO(studySession.Id, studySession.StackId, studySession.StartedAt, DateTime.Now));

                int totalPoints = answeredCards.Sum(x => x.Points);

                // show the total points
                _serviceProvider.GetRequiredService<ConsoleHelper>().ShowMessage($"The study session ended, you computed {totalPoints} points.");
            }
            else
            {
                _serviceProvider.GetRequiredService<ConsoleHelper>().ShowMessage($"Something went wrong and we could not start the study session.");
            }
        }
        else
        {
            _serviceProvider.GetRequiredService<ConsoleHelper>().ShowMessage("This stack have 0 cards, you must create at least one card to be able to study it!");
            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue();
        }
    }

    private StudySessionAnswerPromptDTO? ShowCardAndAskForAnswer(CardShowDTO card, bool hasNextCard = true)
    {
        ShowCardAsPanel(card);

        string? answer = _serviceProvider.GetRequiredService<ConsoleHelper>().GetText("What is the answer for this card?");

        ShowCardAsPanel(card, false);


        if (answer != null)
        {
            int points = 0;

            if (answer.ToLower().Trim() == card.Back.ToLower().Trim())
            {
                points = 1;
                _serviceProvider.GetRequiredService<ConsoleHelper>().ShowMessage("Your answer is correct!");
            }
            else
            {
                _serviceProvider.GetRequiredService<ConsoleHelper>().ShowMessage("Your answer is not correct :(");
            }

            if (hasNextCard)
            {
                _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("Press any key to go to the next card");
            }

            return new StudySessionAnswerPromptDTO(answer, points);
        }

        return null;
    }
}
