using FlashCards.Data.Daos;
using FlashCards.Data.Dtos.Stack;
using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;

namespace FlashCards.Menus;

internal class ManageStacksMenu : IMenu
{
    public void Run()
    {
        ConsoleHelper.ClearWindow();

        string option = ConsoleHelper.GetOption("Manage Stacks Menu", GetMenuChoices());

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
        ConsoleHelper.ShowTitle("Create an Stack");

        StackPromptDTO? stackPromptDTO = null;
        bool cancelCreation = false;

        while ((stackPromptDTO != null ? StackDao.FindStackByName(stackPromptDTO.Name!) != null : true) && !cancelCreation)
        {
            if (stackPromptDTO != null)
            {
                ConsoleHelper.ShowMessage("This name is already in use, pick other!");
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
                    FlashCardsHelper.CurrentUser!,
                    stackPromptDTO!
                );

            StackDao.StoreStack(
                StackStoreDTO.FromPromptDTO(FlashCardsHelper.CurrentUser!, stackPromptDTO!)
            );

            ConsoleHelper.PressAnyKeyToContinue("Stack stored successfully!");
        }
        else
        {
            ConsoleHelper.PressAnyKeyToContinue("No data was provided, operation canceled by user!");
        }


    }

    private void UpdateStack()
    {
        ConsoleHelper.ShowTitle("Update an stack");

        StackShowDTO? selectedStackShowDTO = ShowStacksAndAskForId("Whats the stack ID to update?");

        if (selectedStackShowDTO != null)
        {
            StackPromptDTO? stackPromptDTO = null;
            bool cancelCreation = false;

            while ((stackPromptDTO != null ? StackDao.FindStackByName(stackPromptDTO.Name!, selectedStackShowDTO.Id) != null : true) && !cancelCreation)
            {
                if (stackPromptDTO != null)
                {
                    ConsoleHelper.ShowMessage("This name is already in use, pick other!");
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
                        FlashCardsHelper.CurrentUser!,
                        stackPromptDTO!
                    );

                StackDao.UpdateStack(
                    stackUpdateDTO
                );

                ConsoleHelper.PressAnyKeyToContinue("Stack updated successfully!");
            }
            else
            {
                ConsoleHelper.PressAnyKeyToContinue("No data was provided, operation canceled by user!");
            }
        }
        else
        {
            ConsoleHelper.PressAnyKeyToContinue("No stack found.");
        }
    }

    private void ManageStacks()
    {
        List<StackShowDTO> stacks = StackDao.GetAllStacks(FlashCardsHelper.CurrentUser!);

        if (stacks.Count > 0)
        {
            FlashCardsHelper.ChangeMenu(new ManageCardsMenu());
        }
        else
        {
            ConsoleHelper.PressAnyKeyToContinue("You must create a stack before going to the manage stacks menu!");
            Run();
        }
    }

    private void DeleteStack()
    {
        ConsoleHelper.ShowTitle("Delete a stack");

        StackShowDTO? selectedStackShowDTO = ShowStacksAndAskForId("Whats the stack ID to delete?");

        if (selectedStackShowDTO != null)
        {
            bool result = StackDao.DeleteStack(selectedStackShowDTO.Id, FlashCardsHelper.CurrentUser!);

            ConsoleHelper.ShowMessage(result ? "Stack deleted successfully!" : "Something went wrong :(");
            ConsoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            ConsoleHelper.ShowMessage("No stacks found.");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    internal void MainMenu()
    {
        FlashCardsHelper.ChangeMenu(new MainMenu());
    }

    internal static void PrintStack(StackShowDTO stack)
    {
        ConsoleHelper.ShowMessage($"{stack.Id} - {stack.Name}");
    }

    private void ListStacks()
    {
        ConsoleHelper.ShowTitle("List of stacks");

        List<StackShowDTO> stacks = StackDao.GetAllStacks(FlashCardsHelper.CurrentUser!);

        if (stacks.Count() > 0)
        {
            foreach (StackShowDTO stack in stacks)
            {
                PrintStack(stack);
            }

            ConsoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            ConsoleHelper.ShowMessage("No stack found.");
            ConsoleHelper.PressAnyKeyToContinue();
        }

    }

    internal static StackShowDTO? ShowStacksAndAskForId(string message)
    {
        List<StackShowDTO> stacks = StackDao.GetAllStacks(FlashCardsHelper.CurrentUser!);

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

            ConsoleHelper.ShowMessage("");

            int.TryParse(ConsoleHelper.GetText(message), out int id);

            return stacks.FirstOrDefault(stack => stack.Id == (id > 0 ? id : 0));
        }
    }

    internal static StackPromptDTO? PromptUserForStackData(StackShowDTO? defaultStackShowDTO = null)
    {
        string? name = ConsoleHelper.GetText(
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
