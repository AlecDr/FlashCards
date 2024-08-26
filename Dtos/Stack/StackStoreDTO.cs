namespace FlashCards.Dtos.Stack;

internal class StackStoreDTO
{
    internal string Username { get; }
    internal string? Description { get; }

    internal StackStoreDTO(string username, string? description)
    {
        Username = username;
        Description = description;
    }

    internal static StackStoreDTO FromPromptDTO(string username, StackPromptDTO stackPromptDTO)
    {
        return new StackStoreDTO(
            username,
            stackPromptDTO.Description
        );
    }

    internal virtual object ToAnonymousObject()
    {
        return new
        {
            Username,
            Description,
        };
    }
}
