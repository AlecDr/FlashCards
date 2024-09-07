using Dapper;
using FlashCards.Data.Daos.Interfaces;
using FlashCards.Data.Dtos.Stack;
using FlashCards.Helpers;

namespace FlashCards.Data.Daos.Implementations;

class SQLServerStackDAO : IStackDAO
{
    private readonly ICardDAO _cardDAO;
    private readonly IStudySessionDAO _studySessionDAO;

    public SQLServerStackDAO(
        ICardDAO cardDAO,
        IStudySessionDAO studySessionDAO
    )
    {
        _cardDAO = cardDAO;
        _studySessionDAO = studySessionDAO;
    }

    public StackShowDTO? Find(int id, string username)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id, name FROM STACKS WHERE id = @id AND username = @username";

        StackShowDTO? stackShowDTO = DatabaseHelper.SqliteConnection.QueryFirstOrDefault<StackShowDTO>(query, new { id, username });

        DatabaseHelper.SqliteConnection!.Close();

        return stackShowDTO;
    }

    public StackShowDTO? FindByName(string name, int? idToIgnore = null)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id, name FROM STACKS WHERE name = @Name";

        if (idToIgnore != null)
        {
            query = $"{query} and id != @Id";
        }

        StackShowDTO? stackShowDTO = DatabaseHelper.SqliteConnection.QueryFirstOrDefault<StackShowDTO>(query, new { Id = idToIgnore, Name = name });

        DatabaseHelper.SqliteConnection!.Close();

        return stackShowDTO;
    }

    public List<StackShowDTO> All(string username)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id as Id, name as Name FROM STACKS WHERE username = @username";

        List<StackShowDTO> stacks = DatabaseHelper.SqliteConnection.Query<StackShowDTO>(query, new { username }).ToList();
        DatabaseHelper.SqliteConnection!.Close();

        return stacks;
    }

    public void Store(StackStoreDTO stackStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "INSERT INTO STACKS (name, username) VALUES (@Name, @Username);";

        DatabaseHelper.SqliteConnection!.Execute(query, stackStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();
    }

    public bool Update(StackUpdateDTO stackUpdateDTO)
    {
        StackShowDTO? stack = Find(stackUpdateDTO.Id, stackUpdateDTO.Username);

        if (stack != null)
        {
            DatabaseHelper.SqliteConnection!.Open();

            string query = "UPDATE STACKS SET name = @Name WHERE id = @Id and username = @Username;";

            DatabaseHelper.SqliteConnection!.Execute(query, stackUpdateDTO.ToAnonymousObject());
            DatabaseHelper.SqliteConnection!.Close();

            return true;
        }

        return false;
    }

    public bool Delete(int id, string username)
    {
        StackShowDTO? stack = Find(id, username);

        if (stack != null)
        {
            _cardDAO.DeleteByStackId(stack.Id);
            _studySessionDAO.DeleteByStackId(stack.Id);

            DatabaseHelper.SqliteConnection!.Open();

            string query = "DELETE FROM STACKS WHERE id = @Id and username = @Username;";
            DatabaseHelper.SqliteConnection.Execute(query, new
            {
                Id = id,
                Username = username
            });

            DatabaseHelper.SqliteConnection!.Close();

            return true;
        }

        return false;
    }

    public int? GetLastSequenceById(int id)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT max(sequence) from CARDS WHERE stack_id = @id";

        int? stackShowDTO = DatabaseHelper.SqliteConnection.QueryFirstOrDefault<int?>(query, new { id });

        DatabaseHelper.SqliteConnection!.Close();

        return stackShowDTO;
    }
}
