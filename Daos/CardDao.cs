using Dapper;
using FlashCards.Dtos.Card;
using FlashCards.Helpers;

namespace FlashCards.Daos;

internal abstract class CardDao
{
    internal static void StoreCardDapper(CardStoreDTO cardStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "INSERT INTO CARDS (front, back, stack_id, sequence) VALUES (@Front, @Back, @StackId, @Sequence);";

        DatabaseHelper.SqliteConnection!.Execute(query, cardStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();
    }
}
