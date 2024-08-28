using FlashCards.Daos;
using FlashCards.Dtos.Stack;
using FlashCards.Helpers;
using FlashCards.Menus.Interfaces;

namespace FlashCards.Menus;

internal class MainMenu : IMenu
{
    public void Run()
    {
        ConsoleHelper.ClearWindow();

        string option = GetOption();

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
                FlashCardsHelper.AskName();
                Run();
                break;
            case '6':
                ManageStacks();
                break;

            default:
                Run();
                break;
        }
    }

    public List<string> GetMenuChoices()
    {
        return [
            "1 - [blue]C[/]reate a stack",
            "2 - [blue]L[/]ist all stacks",
            "3 - [blue]U[/]pdate stack",
            "4 - [blue]D[/]elete stack",
            "5 - [blue]A[/]lter username",
            "6 - [blue]M[/]anage stacks",
            ];
    }

    public string GetOption()
    {
        string option = ConsoleHelper.ShowMenu(FlashCardsHelper.CurrentUser!, GetMenuChoices(), "Main Menu");

        while (option == null || option.Trim() == "")
        {
            ConsoleHelper.ClearWindow();
            GetOption();
        }

        return option;
    }

    private void CreateStack()
    {
        ConsoleHelper.ShowTitle("Create an Stack");

        StackPromptDTO? stackPromptDTO = PromptUserForStackData();

        if (stackPromptDTO != null)
        {
            StackDao.StoreStackDapper(
                StackStoreDTO.FromPromptDTO(
                    FlashCardsHelper.CurrentUser!,
                    stackPromptDTO
                )
            );

            ConsoleHelper.ShowMessage("Stack stored successfully!");
            ConsoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            ConsoleHelper.ShowMessage("No data was provided, operation canceled by user!");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    private void UpdateStack()
    {
        ConsoleHelper.ShowTitle("Update an stack");

        StackShowDTO? selectedStackShowDTO = ShowStacksAndAskForId("Whats the stack ID to update?");

        if (selectedStackShowDTO != null)
        {
            StackPromptDTO? stackPromptDTO = PromptUserForStackData(selectedStackShowDTO);

            if (stackPromptDTO != null)
            {
                bool result = StackDao.UpdateStackDapper(
                   StackUpdateDTO.FromPromptDTO(
                       selectedStackShowDTO.Id,
                       FlashCardsHelper.CurrentUser!,
                       stackPromptDTO
                   )
               );

                ConsoleHelper.ShowMessage(result ? "Stack updated successfully!" : "Something went wrong :(");
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
            ConsoleHelper.ShowMessage("No stack found.");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    private void ManageStacks()
    {
        List<StackShowDTO> stacks = StackDao.GetAllStacksDapper(FlashCardsHelper.CurrentUser!);

        if (stacks.Count > 0)
        {
            FlashCardsHelper.ChangeMenu(new ManageStacksMenu());
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
            bool result = StackDao.DeleteStackDapper(selectedStackShowDTO.Id, FlashCardsHelper.CurrentUser!);

            ConsoleHelper.ShowMessage(result ? "Stack deleted successfully!" : "Something went wrong :(");
            ConsoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            ConsoleHelper.ShowMessage("No stacks found.");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    private void PrintStack(StackShowDTO stack)
    {
        ConsoleHelper.ShowMessage($"{stack.Id} - {stack.Name}");
    }

    private void ListStacks()
    {
        ConsoleHelper.ShowTitle("List of stacks");

        List<StackShowDTO> stacks = StackDao.GetAllStacksDapper(FlashCardsHelper.CurrentUser!);

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

    private StackShowDTO? ShowStacksAndAskForId(string message)
    {
        List<StackShowDTO> stacks = StackDao.GetAllStacksDapper(FlashCardsHelper.CurrentUser!);

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
