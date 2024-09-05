using FlashCards.Data.Daos;
using FlashCards.Data.Dtos.Reports;
using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;
using Spectre.Console;

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
        List<ResumedStudySessionsReportDTO> resumedStudySessions = ReportsDao.ResumedStudySessionsByUser(FlashCardsHelper.CurrentUser!);

        if (resumedStudySessions.Count > 0)
        {
            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("Study Session ID");
            table.AddColumn("Stack Name");
            table.AddColumn("Started At");
            table.AddColumn("Finished At");
            table.AddColumn("Total Points");

            foreach (ResumedStudySessionsReportDTO resumedStudySession in resumedStudySessions)
            {
                // Add some rows
                table.AddRow(resumedStudySession.SessionId.ToString(), resumedStudySession.StackName, resumedStudySession.StartedAt.ToString(), resumedStudySession.FinishedAt.ToString(), resumedStudySession.TotalPoints.ToString());
            }

            // Render the table to the console
            AnsiConsole.Write(table);

        }
        else
        {
            ConsoleHelper.PressAnyKeyToContinue("No study sessions found!");
        }

        ConsoleHelper.PressAnyKeyToContinue("Resumed Study Sessions Report");
    }

}
