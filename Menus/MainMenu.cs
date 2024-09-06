using FlashCards.Data.Daos.Interfaces;
using FlashCards.Data.Dtos.Stack;
using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FlashCards.Menus;

internal class MainMenu : IMenu
{
    private readonly IStackDAO _stackDao;
    private readonly IServiceProvider _serviceProvider;

    public MainMenu(IStackDAO stackDao, IServiceProvider serviceProvider)
    {
        _stackDao = stackDao;
        _serviceProvider = serviceProvider;
    }

    public void Run()
    {
        var consoleHelper = _serviceProvider.GetRequiredService<ConsoleHelper>();

        consoleHelper.ClearWindow();

        string option = consoleHelper.GetOption("Main Menu", GetMenuChoices());

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
                StudySessions();
                break;
            case '4':
                Reports();
                break;
            case '5':
                _serviceProvider.GetRequiredService<FlashCardsHelper>().AskName();
                Run();
                break;
            case '6':
                Environment.Exit(0);
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
        "3 - [slateblue1]S[/]tudy Sessions",
        "4 - [slateblue1]R[/]eports",
        "5 - [slateblue1]C[/]hange User",
        "6 - [slateblue1]E[/]xit",
        ];
    }

    private void ManageStacks()
    {
        _serviceProvider.GetRequiredService<FlashCardsHelper>().ChangeMenu(_serviceProvider.GetRequiredService<ManageStacksMenu>());
    }

    private void Reports()
    {
        _serviceProvider.GetRequiredService<FlashCardsHelper>().ChangeMenu(_serviceProvider.GetRequiredService<ReportsMenu>());
    }

    private void ManageCards()
    {
        List<StackShowDTO> stacks = _stackDao.All(_serviceProvider.GetRequiredService<FlashCardsHelper>().CurrentUser!);

        if (stacks.Count > 0)
        {
            _serviceProvider.GetRequiredService<FlashCardsHelper>().ChangeMenu(_serviceProvider.GetRequiredService<ManageCardsMenu>());
        }
        else
        {
            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("You must create a stack before going to the manage cards menu!");
            Run();
        }
    }

    private void StudySessions()
    {
        List<StackShowDTO> stacks = _stackDao.All(_serviceProvider.GetRequiredService<FlashCardsHelper>().CurrentUser!);

        if (stacks.Count > 0)
        {
            _serviceProvider.GetRequiredService<FlashCardsHelper>().ChangeMenu(_serviceProvider.GetRequiredService<StudySessionsMenu>());
        }
        else
        {
            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("You must create a stack before going to the study sessions menu!");
            Run();
        }
    }

}
