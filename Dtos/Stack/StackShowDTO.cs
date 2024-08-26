namespace FlashCards.Dtos.Stack;

internal class StackShowDTO
{
    internal int Id { get; }
    internal string? Description { get; }

    internal StackShowDTO() { }

    internal StackShowDTO(int id, string? description)
    {
        Id = id;
        Description = description;
    }
}
