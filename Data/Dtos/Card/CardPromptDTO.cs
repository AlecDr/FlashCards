namespace FlashCards.Data.Dtos.Card;

internal class CardPromptDTO
{
    internal string Front { get; }
    internal string Back { get; }
    internal int Sequence { get; }

    internal CardPromptDTO(string front, string back, int sequence)
    {
        Front = front;
        Back = back;
        Sequence = sequence;
    }
}
