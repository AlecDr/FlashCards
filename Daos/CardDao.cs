using Dapper;
using FlashCards.Dtos.Card;
using FlashCards.Helpers;

namespace FlashCards.Daos;

internal abstract class CardDao
{
    internal static void StoreCardDapper(CardStoreDTO cardStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "INSERT INTO CARDS (front, back, stack_id) VALUES (@Front, @Back, @StackId);";

        DatabaseHelper.SqliteConnection!.Execute(query, cardStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();
    }
}
