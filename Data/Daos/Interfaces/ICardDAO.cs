using FlashCards.Data.Dtos.Card;

namespace FlashCards.Data.Daos.Interfaces;

interface ICardDAO
{
    public CardShowDTO? Find(int id);
    public void Store(CardStoreDTO cardStoreDTO);
    public bool Update(CardUpdateDTO cardUpdateDTO);
    public List<CardShowDTO> AllCardsFromStack(int stackId);
    public bool Delete(int id);
    public bool DeleteByStackId(int stackId);
    public void Add1ToAllSequencesStartingFrom(int fromSequence, int stackId, int? maxSequenceToUpdate = null);
    public void Subtract1ToAllSequencesStartingFrom(int fromSequence, int stackId, int? maxSequenceToUpdate);
}
