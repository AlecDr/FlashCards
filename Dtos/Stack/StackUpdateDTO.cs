namespace FlashCards.Dtos.Stack;

internal class StackUpdateDTO : StackStoreDTO
{
    internal int Id { get; }

    internal StackUpdateDTO(
        int id,
        string username,
        string? description)
        : base(username, description)
    {
        Id = id;
    }

    internal static StackUpdateDTO FromPromptDTO(int id, string username, StackPromptDTO stackPromptDTO)
    {
        return new StackUpdateDTO(
            id,
            username,
            stackPromptDTO.Description
        );
    }

    internal override object ToAnonymousObject()
    {
        return new
        {
            Id,
            Username,
            Description,
        };
    }
}
