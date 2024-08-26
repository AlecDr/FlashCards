namespace FlashCards.Dtos.Stack;

internal class StackStoreDTO
{
    internal string Username { get; }
    internal string? Name { get; }

    internal StackStoreDTO(string username, string? name)
    {
        Username = username;
        Name = name;
    }

    internal static StackStoreDTO FromPromptDTO(string username, StackPromptDTO stackPromptDTO)
    {
        return new StackStoreDTO(
            username,
            stackPromptDTO.Name
        );
    }

    internal virtual object ToAnonymousObject()
    {
        return new
        {
            Username,
            Name,
        };
    }
}
