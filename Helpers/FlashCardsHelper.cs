using FlashCards.Data.Daos.Interfaces;
using FlashCards.Data.Dtos.Stack;
using FlashCards.Menus;
using FlashCards.Menus.Interfaces;

namespace FlashCards.Helpers;

internal class FlashCardsHelper
{
    private readonly IStackDAO _stackDao;
    private readonly ConsoleHelper _consoleHelper;
    private IMenu _currentMenu;

    public FlashCardsHelper(IStackDAO stackDao, ConsoleHelper consoleHelper, MainMenu mainMenu)
    {
        _stackDao = stackDao;
        _consoleHelper = consoleHelper;
        _currentMenu = mainMenu;
    }

    internal string? CurrentUser { get; set; }
    internal IMenu CurrentMenu => _currentMenu;

    internal void Run()
    {
        while (CurrentUser == null)
        {
            CheckUser();
        }

        CurrentMenu.Run();
    }

    internal void CheckUser()
    {
        if (CurrentUser == null)
        {
            AskName();
            _consoleHelper.ClearWindow();
        }
    }

    internal void AskName()
    {
        _consoleHelper.ShowTitle("User Selection");

        string? name = _consoleHelper.GetText("What is your [slateblue1]name[/]? ");

        if (name != null && name.Trim().Length > 0)
        {
            CurrentUser = name;
        }
    }

    internal void ChangeMenu(IMenu menu)
    {
        _currentMenu = menu;
        Run();
    }

    internal void PrintStack(StackShowDTO stack)
    {
        _consoleHelper.ShowMessage($"{stack.Id} - {stack.Name}");
    }

    internal StackShowDTO? ShowStacksAndAskForId(string message)
    {
        List<StackShowDTO> stacks = _stackDao.All(CurrentUser!);

        if (stacks.Count <= 0)
        {
            return null;
        }
        else
        {
            foreach (StackShowDTO stack in stacks)
            {
                PrintStack(stack);
            }

            _consoleHelper.ShowMessage("");

            int.TryParse(_consoleHelper.GetText(message), out int id);

            return stacks.FirstOrDefault(stack => stack.Id == (id > 0 ? id : 0));
        }
    }
}
