namespace FlashCards.Dtos.Card;

internal class CardShowDTO
{
    internal int Id { get; }
    internal string? Front { get; }
    internal string? Back { get; }
    internal int StackId { get; }
    internal int Sequence { get; }

    internal CardShowDTO() { }

    internal CardShowDTO(int id, string front, string back, int stackId, int sequence)
    {
        Id = id;
        Front = front;
        Back = back;
        StackId = stackId;
        Sequence = sequence;
    }
}
