using FlashCards.Data.Daos;
using FlashCards.Data.Dtos.Reports;
using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace FlashCards.Menus;

internal class ReportsMenu : IMenu
{
    private readonly IServiceProvider _serviceProvider;

    public ReportsMenu(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Run()
    {
        _serviceProvider.GetRequiredService<ConsoleHelper>().ClearWindow();

        string option = _serviceProvider.GetRequiredService<ConsoleHelper>().GetOption("Reports Menu", GetMenuChoices());

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
        _serviceProvider.GetRequiredService<FlashCardsHelper>().ChangeMenu(_serviceProvider.GetRequiredService<MainMenu>());
    }

    private void ResumedStudySessions()
    {
        List<ResumedStudySessionsReportDTO> resumedStudySessions = ReportsDao.ResumedStudySessionsByUser(_serviceProvider.GetRequiredService<FlashCardsHelper>().CurrentUser!);

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

            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("Resumed Study Sessions Report");
        }
        else
        {
            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("No study sessions found!");
        }

    }

}
