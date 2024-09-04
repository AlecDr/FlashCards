namespace FlashCards.Data.Dtos.Stack;

internal class StackUpdateDTO : StackStoreDTO
{
    internal int Id { get; }

    internal StackUpdateDTO(
        int id,
        string username,
        string? name)
        : base(username, name)
    {
        Id = id;
    }

    internal static StackUpdateDTO FromPromptDTO(int id, string username, StackPromptDTO stackPromptDTO)
    {
        return new StackUpdateDTO(
            id,
            username,
            stackPromptDTO.Name
        );
    }

    internal override object ToAnonymousObject()
    {
        return new
        {
            Id,
            Username,
            Name,
        };
    }
}
