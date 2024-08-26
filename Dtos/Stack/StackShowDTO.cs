namespace FlashCards.Dtos.Stack;

internal class StackShowDTO
{
    internal int Id { get; }
    internal string? Name { get; }

    internal StackShowDTO() { }

    internal StackShowDTO(int id, string? name)
    {
        Id = id;
        Name = name;
    }
}
