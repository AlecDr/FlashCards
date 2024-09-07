using Dapper;
using FlashCards.Data.Daos.Interfaces;
using FlashCards.Data.Dtos.Card;
using FlashCards.Helpers;

namespace FlashCards.Data.Daos.Implementations;

internal class SQLServerCardDAO : ICardDAO
{
    private readonly IStudySessionAnswerDAO _studySessionAnswerDao;

    public SQLServerCardDAO(IStudySessionAnswerDAO studySessionAnswerDao)
    {
        _studySessionAnswerDao = studySessionAnswerDao;
    }

    public CardShowDTO? Find(int id)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id as Id, front as Front, back as Back, stack_id as StackId, sequence as Sequence FROM CARDS WHERE id = @Id";

        CardShowDTO? cardShowDTO = DatabaseHelper.SqliteConnection.QueryFirstOrDefault<CardShowDTO>(query, new { Id = id });

        DatabaseHelper.SqliteConnection!.Close();

        return cardShowDTO;
    }

    public void Store(CardStoreDTO cardStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "INSERT INTO CARDS (front, back, stack_id, sequence) VALUES (@Front, @Back, @StackId, @Sequence);";

        DatabaseHelper.SqliteConnection!.Execute(query, cardStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();
    }

    public bool Update(CardUpdateDTO cardUpdateDTO)
    {
        CardShowDTO? card = Find(cardUpdateDTO.Id);

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

    public List<CardShowDTO> AllCardsFromStack(int stackId)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id as Id, front as Front, back as Back, stack_id as StackId, sequence as Sequence FROM CARDS WHERE stack_id = @StackId ORDER BY SEQUENCE";

        List<CardShowDTO> cards = DatabaseHelper.SqliteConnection.Query<CardShowDTO>(query, new { StackId = stackId }).ToList();
        DatabaseHelper.SqliteConnection!.Close();

        return cards;
    }

    public bool Delete(int id)
    {
        CardShowDTO? card = Find(id);

        if (card != null)
        {
            _studySessionAnswerDao.DeleteByCardId(card.Id);

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

    public bool DeleteByStackId(int stackId)
    {
        List<CardShowDTO> cards = AllCardsFromStack(stackId);

        if (cards.Count > 0)
        {
            foreach (CardShowDTO card in cards)
            {
                Delete(card.Id);
            }

            return true;
        }

        return false;
    }

    public void Add1ToAllSequencesStartingFrom(int fromSequence, int stackId, int? maxSequenceToUpdate = null)
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

    public void Subtract1ToAllSequencesStartingFrom(int fromSequence, int stackId, int? maxSequenceToUpdate)
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
