using Dapper;
using FlashCards.Dtos.Stack;
using FlashCards.Helpers;

namespace FlashCards.Daos;

internal abstract class StackDao
{
    internal static StackShowDTO? FindStackDapper(int id, string username)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id, name FROM STACKS WHERE id = @id AND username = @username";

        StackShowDTO? stackShowDTO = DatabaseHelper.SqliteConnection.QueryFirstOrDefault<StackShowDTO>(query, new { id, username });

        DatabaseHelper.SqliteConnection!.Close();

        return stackShowDTO;
    }

    internal static List<StackShowDTO> GetAllStacksDapper(string username)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id as Id, name as Name FROM STACKS WHERE username = @username";

        List<StackShowDTO> stacks = DatabaseHelper.SqliteConnection.Query<StackShowDTO>(query, new { username }).ToList();
        DatabaseHelper.SqliteConnection!.Close();

        return stacks;
    }

    internal static void StoreStackDapper(StackStoreDTO stackStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "INSERT INTO STACKS (name, username) VALUES (@Name, @Username);";

        DatabaseHelper.SqliteConnection!.Execute(query, stackStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();
    }

    internal static bool UpdateStackDapper(StackUpdateDTO stackUpdateDTO)
    {
        StackShowDTO? stack = FindStackDapper(stackUpdateDTO.Id, stackUpdateDTO.Username);

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

    internal static bool DeleteStackDapper(int id, string username)
    {
        StackShowDTO? stack = FindStackDapper(id, username);

        if (stack != null)
        {
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
}
