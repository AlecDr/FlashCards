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
    private readonly ConsoleHelper _consoleHelper;
    private readonly FlashCardsHelper _flashCardsHelper;

    public MainMenu(
        IStackDAO stackDao,
        IServiceProvider serviceProvider,
        ConsoleHelper consoleHelper,
        FlashCardsHelper flashCardsHelper
        )
    {
        _stackDao = stackDao;
        _serviceProvider = serviceProvider;
        _consoleHelper = consoleHelper;
        _flashCardsHelper = flashCardsHelper;
    }

    public void Run()
    {
        _consoleHelper.ClearWindow();

        string option = _consoleHelper.GetOption("Main Menu", GetMenuChoices());

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
                _flashCardsHelper.AskName();
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
        _flashCardsHelper.ChangeMenu(_serviceProvider.GetRequiredService<ManageStacksMenu>());
    }

    private void Reports()
    {
        _flashCardsHelper.ChangeMenu(_serviceProvider.GetRequiredService<ReportsMenu>());
    }

    private void ManageCards()
    {
        List<StackShowDTO> stacks = _stackDao.All(_flashCardsHelper.CurrentUser!);

        if (stacks.Count > 0)
        {
            _flashCardsHelper.ChangeMenu(_serviceProvider.GetRequiredService<ManageCardsMenu>());
        }
        else
        {
            _consoleHelper.PressAnyKeyToContinue("You must create a stack before going to the manage cards menu!");
            Run();
        }
    }

    private void StudySessions()
    {
        List<StackShowDTO> stacks = _stackDao.All(_flashCardsHelper.CurrentUser!);

        if (stacks.Count > 0)
        {
            _flashCardsHelper.ChangeMenu(_serviceProvider.GetRequiredService<StudySessionsMenu>());
        }
        else
        {
            _consoleHelper.PressAnyKeyToContinue("You must create a stack before going to the study sessions menu!");
            Run();
        }
    }

}
