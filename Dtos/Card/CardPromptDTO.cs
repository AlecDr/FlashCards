namespace FlashCards.Dtos.Card;

internal class CardPromptDTO
{
    internal string Front { get; }
    internal string Back { get; }

    internal CardPromptDTO(string front, string back)
    {
        Front = front;
        Back = back;
    }
}
