namespace FlashCards.Dtos.Card;

internal class CardUpdateDTO : CardStoreDTO
{
    internal int Id { get; }

    internal CardUpdateDTO(int id, int stackId, string front, string back, int sequence)
        : base(stackId, front, back, sequence)
    {
        Id = id;
    }

    internal static CardUpdateDTO FromPromptDTO(int id, int stackId, CardPromptDTO stackPromptDTO)
    {
        return new CardUpdateDTO(
            id,
            stackId,
            stackPromptDTO.Front,
            stackPromptDTO.Back,
            stackPromptDTO.Sequence
        );
    }

    internal override object ToAnonymousObject()
    {
        return new
        {
            Id,
            StackId,
            Front,
            Back,
            Sequence,
        };
    }
}
