
using FlashCards.Daos;
using FlashCards.Dtos.Stack;

namespace FlashCards.Helpers;

internal class FlashCardsHelper
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

    private void CreateStack()
    {
        ConsoleHelper.ShowTitle("Create an Stack");

        StackPromptDTO? stackPromptDTO = PromptUserForStackData();

        if (stackPromptDTO != null)
        {
            StackDao.StoreStackDapper(
                StackStoreDTO.FromPromptDTO(
                    CurrentUser!,
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
                       CurrentUser!,
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

    private void DeleteStack()
    {
        ConsoleHelper.ShowTitle("Delete a stack");

        StackShowDTO? selectedStackShowDTO = ShowStacksAndAskForId("Whats the stack ID to delete?");

        if (selectedStackShowDTO != null)
        {
            bool result = StackDao.DeleteStackDapper(selectedStackShowDTO.Id, CurrentUser!);

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
        ConsoleHelper.ShowMessage($"{stack.Id} - {stack.Description}");
    }

    private void ListStacks()
    {
        ConsoleHelper.ShowTitle("List of stacks");

        List<StackShowDTO> stacks = StackDao.GetAllStacksDapper(CurrentUser!);

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
        List<StackShowDTO> stacks = StackDao.GetAllStacksDapper(CurrentUser!);

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

    private void RouteToOption(char option)
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
            "1 - [blue]C[/]reate a stack",
            "2 - [blue]L[/]ist all stacks",
            "3 - [blue]U[/]pdate stack",
            "4 - [blue]D[/]elete stack",
            "5 - [blue]A[/]lter username",
            ];
    }

    internal static StackPromptDTO? PromptUserForStackData(StackShowDTO? defaultStackShowDTO = null)
    {
        string? description = ConsoleHelper.GetText(
            "What did you learn today?",
            defaultStackShowDTO != null ? defaultStackShowDTO.Description : null,
            true,
            true
        );

        if (description != null)
        {
            return new StackPromptDTO(description);
        }


        return null;
    }
}
