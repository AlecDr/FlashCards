namespace FlashCards.Data.Dtos.Stack;

internal class StackPromptDTO
{
    internal string? Name { get; }

    internal StackPromptDTO(string? name)
    {
        Name = name;
    }
}
