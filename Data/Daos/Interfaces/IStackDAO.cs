using FlashCards.Data.Dtos.Stack;

namespace FlashCards.Data.Daos.Interfaces;

interface IStackDAO
{
    StackShowDTO? Find(int id, string username);
    List<StackShowDTO> All(string username);
    void Store(StackStoreDTO stackStoreDTO);
    bool Update(StackUpdateDTO stackUpdateDTO);
    bool Delete(int id, string username);
    StackShowDTO? FindByName(string name, int? idToIgnore = null);
    int? GetLastSequenceById(int id);
}
