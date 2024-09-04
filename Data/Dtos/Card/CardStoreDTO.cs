namespace FlashCards.Data.Dtos.Card;

internal class CardStoreDTO
{
    internal string Front { get; }
    internal string Back { get; }
    internal int StackId { get; }
    internal int Sequence { get; }

    internal CardStoreDTO(int stackId, string front, string back, int sequence)
    {
        StackId = stackId;
        Front = front;
        Back = back;
        Sequence = sequence;
    }

    internal static CardStoreDTO FromPromptDTO(int stackId, CardPromptDTO stackPromptDTO)
    {
        return new CardStoreDTO(
            stackId,
            stackPromptDTO.Front,
            stackPromptDTO.Back,
            stackPromptDTO.Sequence
        );
    }

    internal virtual object ToAnonymousObject()
    {
        return new
        {
            StackId,
            Front,
            Back,
            Sequence,
        };
    }
}
