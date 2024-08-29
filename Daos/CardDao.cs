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

    internal static List<CardShowDTO> GetAllCardsFromStack(int stackId)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id as Id, front as Front, back as Back, stack_id as StackId, sequence as Sequence FROM CARDS WHERE stack_id = @StackId ORDER BY SEQUENCE";

        List<CardShowDTO> cards = DatabaseHelper.SqliteConnection.Query<CardShowDTO>(query, new { StackId = stackId }).ToList();
        DatabaseHelper.SqliteConnection!.Close();

        return cards;
    }
}
