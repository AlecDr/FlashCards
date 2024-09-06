using FlashCards.Data.Daos.Interfaces;
using FlashCards.Data.Dtos.Stack;
using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FlashCards.Menus;

internal class ManageStacksMenu : IMenu
{
    private readonly IStackDAO _stackDao;
    private readonly IServiceProvider _serviceProvider;
    private readonly FlashCardsHelper _flashCardsHelper;
    private readonly ConsoleHelper _consoleHelper;

    public ManageStacksMenu(IServiceProvider serviceProvider, IStackDAO stackDAO, FlashCardsHelper flashCardsHelper, ConsoleHelper consoleHelper)
    {
        _serviceProvider = serviceProvider;
        _stackDao = stackDAO;
        _flashCardsHelper = flashCardsHelper;
        _consoleHelper = consoleHelper;
    }

    public void Run()
    {
        _consoleHelper.ClearWindow();

        string option = _consoleHelper.GetOption("Manage Stacks Menu", GetMenuChoices());

        RouteToOption(option.ElementAt(0));
    }

    public void RouteToOption(char option)
    {
        switch (option)
        {
            case '1':
                CreateStack();
                Run();
                break;
            case '2':
                ListStacks();
                Run();
                break;
            case '3':
                UpdateStack();
                Run();
                break;
            case '4':
                DeleteStack();
                Run();
                break;
            case '5':
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
            "1 - [slateblue1 slowblink]C[/]reate a stack",
            "2 - [slateblue1 slowblink]L[/]ist all stacks",
            "3 - [slateblue1 slowblink]U[/]pdate stack",
            "4 - [slateblue1 slowblink]D[/]elete stack",
            "5 - [slateblue1 slowblink]M[/]ain Menu",
            ];
    }

    private void CreateStack()
    {
        _consoleHelper.ShowTitle("Create an Stack");

        StackPromptDTO? stackPromptDTO = null;
        bool cancelCreation = false;

        while ((stackPromptDTO != null ? _stackDao.FindByName(stackPromptDTO.Name!) != null : true) && !cancelCreation)
        {
            if (stackPromptDTO != null)
            {
                _consoleHelper.ShowMessage("This name is already in use, pick other!");
                stackPromptDTO = PromptUserForStackData();

                if (stackPromptDTO == null)
                {
                    cancelCreation = true;
                }
            }
            else
            {
                stackPromptDTO = PromptUserForStackData();
            }
        }

        if (!cancelCreation)
        {
            StackStoreDTO stackStoreDTO = StackStoreDTO.FromPromptDTO(
                    _flashCardsHelper.CurrentUser!,
                    stackPromptDTO!
                );

            _stackDao.Store(
                StackStoreDTO.FromPromptDTO(_flashCardsHelper.CurrentUser!, stackPromptDTO!)
            );

            _consoleHelper.PressAnyKeyToContinue("Stack stored successfully!");
        }
        else
        {
            _consoleHelper.PressAnyKeyToContinue("No data was provided, operation canceled by user!");
        }


    }

    private void UpdateStack()
    {
        _consoleHelper.ShowTitle("Update an stack");

        StackShowDTO? selectedStackShowDTO = _flashCardsHelper.ShowStacksAndAskForId("Whats the stack ID to update?");

        if (selectedStackShowDTO != null)
        {
            StackPromptDTO? stackPromptDTO = null;
            bool cancelCreation = false;

            while ((stackPromptDTO == null || _stackDao.FindByName(stackPromptDTO.Name!, selectedStackShowDTO.Id) != null) && !cancelCreation)
            {
                if (stackPromptDTO != null)
                {
                    _consoleHelper.ShowMessage("This name is already in use, pick other!");
                    stackPromptDTO = PromptUserForStackData(selectedStackShowDTO);

                    if (stackPromptDTO == null)
                    {
                        cancelCreation = true;
                    }
                }
                else
                {
                    stackPromptDTO = PromptUserForStackData(selectedStackShowDTO);
                }
            }

            if (!cancelCreation)
            {
                StackUpdateDTO stackUpdateDTO = StackUpdateDTO.FromPromptDTO(
                        selectedStackShowDTO.Id,
                        _flashCardsHelper.CurrentUser!,
                        stackPromptDTO!
                    );

                _stackDao.Update(
                    stackUpdateDTO
                );

                _consoleHelper.PressAnyKeyToContinue("Stack updated successfully!");
            }
            else
            {
                _consoleHelper.PressAnyKeyToContinue("No data was provided, operation canceled by user!");
            }
        }
        else
        {
            _consoleHelper.PressAnyKeyToContinue("No stack found.");
        }
    }

    private void ManageStacks()
    {
        List<StackShowDTO> stacks = _stackDao.All(_flashCardsHelper.CurrentUser!);

        if (stacks.Count > 0)
        {
            _flashCardsHelper.ChangeMenu(_serviceProvider.GetRequiredService<ManageCardsMenu>());
        }
        else
        {
            _consoleHelper.PressAnyKeyToContinue("You must create a stack before going to the manage stacks menu!");
            Run();
        }
    }

    private void DeleteStack()
    {
        _consoleHelper.ShowTitle("Delete a stack");

        StackShowDTO? selectedStackShowDTO = _flashCardsHelper.ShowStacksAndAskForId("Whats the stack ID to delete?");

        if (selectedStackShowDTO != null)
        {
            bool result = _stackDao.Delete(selectedStackShowDTO.Id, _flashCardsHelper.CurrentUser!);

            _consoleHelper.ShowMessage(result ? "Stack deleted successfully!" : "Something went wrong :(");
            _consoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            _consoleHelper.ShowMessage("No stacks found.");
            _consoleHelper.PressAnyKeyToContinue();
        }
    }

    internal void MainMenu()
    {
        _flashCardsHelper.ChangeMenu(_serviceProvider.GetRequiredService<MainMenu>());
    }



    private void ListStacks()
    {
        _consoleHelper.ShowTitle("List of stacks");

        List<StackShowDTO> stacks = _stackDao.All(_flashCardsHelper.CurrentUser!);

        if (stacks.Count() > 0)
        {
            foreach (StackShowDTO stack in stacks)
            {
                _flashCardsHelper.PrintStack(stack);
            }

            _consoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            _consoleHelper.ShowMessage("No stack found.");
            _consoleHelper.PressAnyKeyToContinue();
        }

    }



    internal StackPromptDTO? PromptUserForStackData(StackShowDTO? defaultStackShowDTO = null)
    {
        string? name = _consoleHelper.GetText(
            "Whats the stack name?",
            defaultStackShowDTO != null ? defaultStackShowDTO.Name : null,
            true,
            true, 2
        );

        if (name != null)
        {
            return new StackPromptDTO(name);
        }


        return null;
    }
}
