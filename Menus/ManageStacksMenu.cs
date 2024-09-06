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

    public ManageStacksMenu(IServiceProvider serviceProvider, IStackDAO stackDAO)
    {
        _serviceProvider = serviceProvider;
        _stackDao = stackDAO;
    }

    public void Run()
    {
        _serviceProvider.GetRequiredService<ConsoleHelper>().ClearWindow();

        string option = _serviceProvider.GetRequiredService<ConsoleHelper>().GetOption("Manage Stacks Menu", GetMenuChoices());

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
        _serviceProvider.GetRequiredService<ConsoleHelper>().ShowTitle("Create an Stack");

        StackPromptDTO? stackPromptDTO = null;
        bool cancelCreation = false;

        while ((stackPromptDTO != null ? _stackDao.FindByName(stackPromptDTO.Name!) != null : true) && !cancelCreation)
        {
            if (stackPromptDTO != null)
            {
                _serviceProvider.GetRequiredService<ConsoleHelper>().ShowMessage("This name is already in use, pick other!");
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
                    _serviceProvider.GetRequiredService<FlashCardsHelper>().CurrentUser!,
                    stackPromptDTO!
                );

            _stackDao.Store(
                StackStoreDTO.FromPromptDTO(_serviceProvider.GetRequiredService<FlashCardsHelper>().CurrentUser!, stackPromptDTO!)
            );

            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("Stack stored successfully!");
        }
        else
        {
            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("No data was provided, operation canceled by user!");
        }


    }

    private void UpdateStack()
    {
        _serviceProvider.GetRequiredService<ConsoleHelper>().ShowTitle("Update an stack");

        StackShowDTO? selectedStackShowDTO = _serviceProvider.GetRequiredService<FlashCardsHelper>().ShowStacksAndAskForId("Whats the stack ID to update?");

        if (selectedStackShowDTO != null)
        {
            StackPromptDTO? stackPromptDTO = null;
            bool cancelCreation = false;

            while ((stackPromptDTO == null || _stackDao.FindByName(stackPromptDTO.Name!, selectedStackShowDTO.Id) != null) && !cancelCreation)
            {
                if (stackPromptDTO != null)
                {
                    _serviceProvider.GetRequiredService<ConsoleHelper>().ShowMessage("This name is already in use, pick other!");
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
                        _serviceProvider.GetRequiredService<FlashCardsHelper>().CurrentUser!,
                        stackPromptDTO!
                    );

                _stackDao.Update(
                    stackUpdateDTO
                );

                _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("Stack updated successfully!");
            }
            else
            {
                _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("No data was provided, operation canceled by user!");
            }
        }
        else
        {
            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("No stack found.");
        }
    }

    private void ManageStacks()
    {
        List<StackShowDTO> stacks = _stackDao.All(_serviceProvider.GetRequiredService<FlashCardsHelper>().CurrentUser!);

        if (stacks.Count > 0)
        {
            _serviceProvider.GetRequiredService<FlashCardsHelper>().ChangeMenu(_serviceProvider.GetRequiredService<ManageCardsMenu>());
        }
        else
        {
            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue("You must create a stack before going to the manage stacks menu!");
            Run();
        }
    }

    private void DeleteStack()
    {
        _serviceProvider.GetRequiredService<ConsoleHelper>().ShowTitle("Delete a stack");

        StackShowDTO? selectedStackShowDTO = _serviceProvider.GetRequiredService<FlashCardsHelper>().ShowStacksAndAskForId("Whats the stack ID to delete?");

        if (selectedStackShowDTO != null)
        {
            bool result = _stackDao.Delete(selectedStackShowDTO.Id, _serviceProvider.GetRequiredService<FlashCardsHelper>().CurrentUser!);

            _serviceProvider.GetRequiredService<ConsoleHelper>().ShowMessage(result ? "Stack deleted successfully!" : "Something went wrong :(");
            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue();
        }
        else
        {
            _serviceProvider.GetRequiredService<ConsoleHelper>().ShowMessage("No stacks found.");
            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue();
        }
    }

    internal void MainMenu()
    {
        _serviceProvider.GetRequiredService<FlashCardsHelper>().ChangeMenu(_serviceProvider.GetRequiredService<MainMenu>());
    }



    private void ListStacks()
    {
        _serviceProvider.GetRequiredService<ConsoleHelper>().ShowTitle("List of stacks");

        List<StackShowDTO> stacks = _stackDao.All(_serviceProvider.GetRequiredService<FlashCardsHelper>().CurrentUser!);

        if (stacks.Count() > 0)
        {
            foreach (StackShowDTO stack in stacks)
            {
                _serviceProvider.GetRequiredService<FlashCardsHelper>().PrintStack(stack);
            }

            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue();
        }
        else
        {
            _serviceProvider.GetRequiredService<ConsoleHelper>().ShowMessage("No stack found.");
            _serviceProvider.GetRequiredService<ConsoleHelper>().PressAnyKeyToContinue();
        }

    }



    internal StackPromptDTO? PromptUserForStackData(StackShowDTO? defaultStackShowDTO = null)
    {
        string? name = _serviceProvider.GetRequiredService<ConsoleHelper>().GetText(
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
