using Dapper;
using FlashCards.Dtos.Card;
using FlashCards.Helpers;

namespace FlashCards.Daos;

internal abstract class CardDao
{
    internal static CardShowDTO? FindCard(int id)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id as Id, front as Front, back as Back, stack_id as StackId, sequence as Sequence FROM CARDS WHERE id = @Id";

        CardShowDTO? cardShowDTO = DatabaseHelper.SqliteConnection.QueryFirstOrDefault<CardShowDTO>(query, new { Id = id });

        DatabaseHelper.SqliteConnection!.Close();

        return cardShowDTO;
    }

    internal static void StoreCardDapper(CardStoreDTO cardStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "INSERT INTO CARDS (front, back, stack_id, sequence) VALUES (@Front, @Back, @StackId, @Sequence);";

        DatabaseHelper.SqliteConnection!.Execute(query, cardStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();
    }

    internal static bool UpdateCard(CardUpdateDTO cardUpdateDTO)
    {
        CardShowDTO? card = FindCard(cardUpdateDTO.Id);

        if (card != null)
        {
            DatabaseHelper.SqliteConnection!.Open();

            string query = "UPDATE CARDS SET front = @Front, back = @Back, sequence = @Sequence WHERE id = @Id and stack_id = @StackId;";

            DatabaseHelper.SqliteConnection!.Execute(query, cardUpdateDTO.ToAnonymousObject());
            DatabaseHelper.SqliteConnection!.Close();

            return true;
        }

        return false;
    }

    internal static List<CardShowDTO> GetAllCardsFromStack(int stackId)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id as Id, front as Front, back as Back, stack_id as StackId, sequence as Sequence FROM CARDS WHERE stack_id = @StackId ORDER BY SEQUENCE";

        List<CardShowDTO> cards = DatabaseHelper.SqliteConnection.Query<CardShowDTO>(query, new { StackId = stackId }).ToList();
        DatabaseHelper.SqliteConnection!.Close();

        return cards;
    }

    internal static bool DeleteCard(int id)
    {
        CardShowDTO? card = FindCard(id);

        if (card != null)
        {
            DatabaseHelper.SqliteConnection!.Open();

            string query = "DELETE FROM CARDS WHERE id = @Id;";

            DatabaseHelper.SqliteConnection.Execute(query, new
            {
                Id = id,
            });

            DatabaseHelper.SqliteConnection!.Close();

            return true;
        }

        return false;
    }

    internal static void Add1ToAllSequencesStartingFrom(int fromSequence, int stackId, int? maxSequenceToUpdate = null)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string command = "UPDATE CARDS SET SEQUENCE = SEQUENCE + 1 WHERE stack_id = @StackId AND sequence >= @FromSequence";

        if (maxSequenceToUpdate != null)
        {
            command = $"{command} AND sequence < @MaxSequenceToUpdate";
        }

        DatabaseHelper.SqliteConnection.Execute(command, new { FromSequence = fromSequence, StackId = stackId, MaxSequenceToUpdate = maxSequenceToUpdate });

        DatabaseHelper.SqliteConnection!.Close();
    }

    internal static void Subtract1ToAllSequencesStartingFrom(int fromSequence, int stackId, int? maxSequenceToUpdate)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string command = "UPDATE CARDS SET SEQUENCE = SEQUENCE - 1 WHERE stack_id = @StackId AND sequence > @FromSequence";

        if (maxSequenceToUpdate != null)
        {
            command = $"{command} AND sequence <= @MaxSequenceToUpdate";
        }

        DatabaseHelper.SqliteConnection.Execute(command, new { FromSequence = fromSequence, StackId = stackId, MaxSequenceToUpdate = maxSequenceToUpdate });

        DatabaseHelper.SqliteConnection!.Close();
    }
}
