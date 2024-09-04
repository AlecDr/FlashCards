using Dapper;
using FlashCards.Data.Dtos.Stack;
using FlashCards.Helpers;

namespace FlashCards.Data.Daos;

internal abstract class StackDao
{
    internal static StackShowDTO? FindStack(int id, string username)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id, name FROM STACKS WHERE id = @id AND username = @username";

        StackShowDTO? stackShowDTO = DatabaseHelper.SqliteConnection.QueryFirstOrDefault<StackShowDTO>(query, new { id, username });

        DatabaseHelper.SqliteConnection!.Close();

        return stackShowDTO;
    }

    internal static StackShowDTO? FindStackByName(string name, int? idToIgnore = null)
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

    internal static List<StackShowDTO> GetAllStacks(string username)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id as Id, name as Name FROM STACKS WHERE username = @username";

        List<StackShowDTO> stacks = DatabaseHelper.SqliteConnection.Query<StackShowDTO>(query, new { username }).ToList();
        DatabaseHelper.SqliteConnection!.Close();

        return stacks;
    }

    internal static void StoreStack(StackStoreDTO stackStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "INSERT INTO STACKS (name, username) VALUES (@Name, @Username);";

        DatabaseHelper.SqliteConnection!.Execute(query, stackStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();
    }

    internal static bool UpdateStack(StackUpdateDTO stackUpdateDTO)
    {
        StackShowDTO? stack = FindStack(stackUpdateDTO.Id, stackUpdateDTO.Username);

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

    internal static bool DeleteStack(int id, string username)
    {
        StackShowDTO? stack = FindStack(id, username);

        if (stack != null)
        {
            CardDao.DeleteCardByStackId(stack.Id);
            StudySessionDao.DeleteStudySessionByStackId(stack.Id);

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

    internal static int? GetLastSequenceFromStack(int id)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT max(sequence) from CARDS WHERE stack_id = @id";

        int? stackShowDTO = DatabaseHelper.SqliteConnection.QueryFirstOrDefault<int?>(query, new { id });

        DatabaseHelper.SqliteConnection!.Close();

        return stackShowDTO;
    }
}
