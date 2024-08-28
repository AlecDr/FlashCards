namespace FlashCards.Dtos.Card;

internal class CardStoreDTO
{
    internal string Front { get; }
    internal string Back { get; }
    internal int StackId { get; }

    internal CardStoreDTO(int stackId, string front, string back)
    {
        StackId = stackId;
        Front = front;
        Back = back;
    }

    internal static CardStoreDTO FromPromptDTO(int stackId, CardPromptDTO stackPromptDTO)
    {
        return new CardStoreDTO(
            stackId,
            stackPromptDTO.Front,
            stackPromptDTO.Back
        );
    }

    internal virtual object ToAnonymousObject()
    {
        return new
        {
            StackId,
            Front,
            Back,
        };
    }
}
