
using CodingTracker.Daos;
using CodingTracker.Dtos.CodingSession;
using HabitLogger.Dtos.HabitOccurrence;

namespace CodingTracker.Helpers;

internal class CodingTrackerHelper
{
    string? CurrentUser { get; set; }

    internal void Run()
    {
        while (CurrentUser == null)
        {
            CheckUser();
        }

        string option = GetOption();

        RouteToOption(option.ElementAt(0));
    }

    private string GetOption()
    {
        string option = ConsoleHelper.ShowMainMenu(CurrentUser!);

        while (option == null || option.Trim() == "")
        {
            ConsoleHelper.ClearWindow();
            option = ConsoleHelper.ShowMainMenu(CurrentUser!);
        }

        return option;
    }

    private void CheckUser()
    {
        if (CurrentUser == null)
        {
            AskName();
            ConsoleHelper.ClearWindow();
        }
    }

    private void AskName()
    {
        ConsoleHelper.ShowTitle("User Selection");

        string? name = ConsoleHelper.GetText("What is your [blue]name[/]? ");

        if (name != null && name.Trim().Length > 0)
        {
            CurrentUser = name;
        }
    }

    private void CreateCodingSession()
    {
        ConsoleHelper.ShowTitle("Create an Coding Session");

        CodingSessionPromptDTO? codingSessionPromptDTO = PromptUserForCodingSessionData();

        if (codingSessionPromptDTO != null)
        {
            CodingSessionsDao.StoreCodingSessionDapper(
                CodingSessionStoreDTO.FromPromptDTO(
                    CurrentUser!,
                    codingSessionPromptDTO
                )
            );

            ConsoleHelper.ShowMessage("Coding session stored successfully!");
            ConsoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            ConsoleHelper.ShowMessage("No data was provided, operation canceled by user!");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    private void UpdateCodingSession()
    {
        ConsoleHelper.ShowTitle("Update an codingSession");

        CodingSessionShowDTO? selectedCodingSessionShowDTO = ShowCodingSessionsAndAskForId("Whats the Coding session ID to update?");

        if (selectedCodingSessionShowDTO != null)
        {
            CodingSessionPromptDTO? codingSessionPromptDTO = PromptUserForCodingSessionData(selectedCodingSessionShowDTO);

            if (codingSessionPromptDTO != null)
            {
                bool result = CodingSessionsDao.UpdateCodingSessionDapper(
                   CodingSessionUpdateDTO.FromPromptDTO(
                       selectedCodingSessionShowDTO.Id,
                       CurrentUser!,
                       codingSessionPromptDTO
                   )
               );

                ConsoleHelper.ShowMessage(result ? "Coding session updated successfully!" : "Something went wrong :(");
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
            ConsoleHelper.ShowMessage("No Coding sessions found.");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    private void DeleteCodingSession()
    {
        ConsoleHelper.ShowTitle("Delete a Coding session");

        CodingSessionShowDTO? selectedCodingSessionShowDTO = ShowCodingSessionsAndAskForId("Whats the Coding session ID to delete?");

        if (selectedCodingSessionShowDTO != null)
        {
            bool result = CodingSessionsDao.DeleteCodingSessionDapper(selectedCodingSessionShowDTO.Id, CurrentUser!);

            ConsoleHelper.ShowMessage(result ? "Coding session deleted successfully!" : "Something went wrong :(");
            ConsoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            ConsoleHelper.ShowMessage("No Coding sessions found.");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    private void PrintCodingSession(CodingSessionShowDTO codingSession)
    {
        ConsoleHelper.ShowMessage($"{codingSession.Id} - {codingSession.StartDate} to {codingSession.EndDate} - {codingSession.DurationInSeconds} seconds - {codingSession.Description}");
    }

    private void ListCodingSessions()
    {
        ConsoleHelper.ShowTitle("List of codingSessions");

        List<CodingSessionShowDTO> codingSessions = CodingSessionsDao.GetAllCodingSessionsDapper(CurrentUser!);

        if (codingSessions.Count() > 0)
        {
            foreach (CodingSessionShowDTO codingSession in codingSessions)
            {
                PrintCodingSession(codingSession);
            }

            ConsoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            ConsoleHelper.ShowMessage("No Coding sessions found.");
            ConsoleHelper.PressAnyKeyToContinue();
        }

    }

    private CodingSessionShowDTO? ShowCodingSessionsAndAskForId(string message)
    {
        List<CodingSessionShowDTO> codingSessions = CodingSessionsDao.GetAllCodingSessions(CurrentUser!);

        if (codingSessions.Count <= 0)
        {
            return null;
        }
        else
        {
            foreach (CodingSessionShowDTO codingSession in codingSessions)
            {
                PrintCodingSession(codingSession);
            }

            ConsoleHelper.ShowMessage("");

            int.TryParse(ConsoleHelper.GetText(message), out int id);

            return codingSessions.FirstOrDefault(codingSession => codingSession.Id == (id > 0 ? id : 0));
        }
    }

    private void RouteToOption(char option)
    {
        switch (option)
        {
            case '1':
                CreateCodingSession();
                Run();
                break;
            case '2':
                ListCodingSessions();
                Run();
                break;
            case '3':
                UpdateCodingSession();
                Run();
                break;
            case '4':
                DeleteCodingSession();
                Run();
                break;
            case '5':
                AskName();
                Run();
                break;

            default:
                Run();
                break;
        }
    }

    internal static List<string> GetMenuChoices()
    {
        return [
            "1 - [blue]C[/]reate a Coding session",
            "2 - [blue]L[/]ist all Coding sessions",
            "3 - [blue]U[/]pdate Coding session",
            "4 - [blue]D[/]elete Coding session",
            "5 - [blue]A[/]lter username",
            ];
    }

    internal static CodingSessionPromptDTO? PromptUserForCodingSessionData(CodingSessionShowDTO? defaultCodingSessionShowDTO = null)
    {
        string? description = ConsoleHelper.GetText(
            "What did you learn today?",
            defaultCodingSessionShowDTO != null ? defaultCodingSessionShowDTO.Description : null,
            true,
            true
        );

        if (description != null)
        {
            DateTime startDate = ConsoleHelper.GetDateTime("Enter the start date",
                null,
                defaultCodingSessionShowDTO != null ? DateTime.Parse(defaultCodingSessionShowDTO.StartDate) : null
            );
            DateTime endDate = ConsoleHelper.GetDateTime(
                "Enter the end date",
                startDate,
                defaultCodingSessionShowDTO != null ? DateTime.Parse(defaultCodingSessionShowDTO.EndDate) : null
            );
            return new CodingSessionPromptDTO(description, startDate, endDate);
        }


        return null;
    }
}
