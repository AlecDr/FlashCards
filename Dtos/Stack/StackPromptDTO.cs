namespace FlashCards.Dtos.Stack;

internal class StackPromptDTO
{
    internal string? Description { get; }

    internal StackPromptDTO(string? description)
    {
        Description = description;
    }
}
