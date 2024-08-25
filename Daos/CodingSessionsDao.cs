using CodingTracker.Helpers;
using Dapper;
using HabitLogger.Dtos.HabitOccurrence;
using System.Data.SQLite;

namespace CodingTracker.Daos;

internal abstract class CodingSessionsDao
{
    internal static CodingSessionShowDTO? FindCodingSession(int id, string username)
    {
        CodingSessionShowDTO? codingSession = null;

        using (SQLiteCommand command = DatabaseHelper.CreateCommand())
        {
            DatabaseHelper.SqliteConnection!.Open();

            command.CommandText = "SELECT id, description, start_date, end_date, duration_in_seconds FROM CODING_SESSIONS WHERE id = @id AND username = @username;";
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("username", username);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    codingSession = new CodingSessionShowDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt64(4));
                }
            }

            DatabaseHelper.SqliteConnection!.Close();
        }

        return codingSession;
    }

    internal static CodingSessionShowDTO? FindCodingSessionDapper(int id, string username)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id, description, start_date, end_date, duration_in_seconds FROM CODING_SESSIONS WHERE id = @id AND username = @username";

        return DatabaseHelper.SqliteConnection.QueryFirstOrDefault<CodingSessionShowDTO>(query, new { id, username });
    }

    internal static List<CodingSessionShowDTO> GetAllCodingSessions(string username)
    {
        List<CodingSessionShowDTO> codingSessions = [];

        using (SQLiteCommand command = DatabaseHelper.CreateCommand())
        {
            DatabaseHelper.SqliteConnection!.Open();
            command.CommandText = "SELECT id, description, start_date, end_date, duration_in_seconds FROM CODING_SESSIONS WHERE username = @username;";
            command.Parameters.AddWithValue("username", username);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    codingSessions.Add(new CodingSessionShowDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt64(4)));
                }
            }


            DatabaseHelper.SqliteConnection!.Close();
        }

        return codingSessions;
    }

    internal static List<CodingSessionShowDTO> GetAllCodingSessionsDapper(string username)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "SELECT id as Id, description as Description, start_date as StartDate, end_date as EndDate, duration_in_seconds as DurationInSeconds FROM CODING_SESSIONS WHERE username = @username";

        List<CodingSessionShowDTO> codingSessions = DatabaseHelper.SqliteConnection.Query<CodingSessionShowDTO>(query, new { username }).ToList();
        DatabaseHelper.SqliteConnection!.Close();

        return codingSessions;
    }

    internal static void StoreCodingSession(CodingSessionStoreDTO codingSessionStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        using (SQLiteCommand command = DatabaseHelper.CreateCommand())
        {
            command.CommandText = "INSERT INTO CODING_SESSIONS (description, username, start_date, end_date, duration_in_seconds) VALUES (@description, @username, @startDate, @endDate, @durationInSeconds)";

            command.Parameters.AddWithValue("description", codingSessionStoreDTO.Description);
            command.Parameters.AddWithValue("username", codingSessionStoreDTO.Username);
            command.Parameters.AddWithValue("startDate", codingSessionStoreDTO.StartDateTime);
            command.Parameters.AddWithValue("endDate", codingSessionStoreDTO.EndDateTime);
            command.Parameters.AddWithValue("durationInSeconds", codingSessionStoreDTO.DurationInSeconds);

            command.ExecuteNonQuery();
        }

        DatabaseHelper.SqliteConnection!.Close();
    }

    internal static void StoreCodingSessionDapper(CodingSessionStoreDTO codingSessionStoreDTO)
    {
        DatabaseHelper.SqliteConnection!.Open();

        string query = "INSERT INTO CODING_SESSIONS (description, username, start_date, end_date, duration_in_seconds) VALUES (@Description, @Username, @StartDateTime, @EndDateTime, @DurationInSeconds);";

        DatabaseHelper.SqliteConnection!.Execute(query, codingSessionStoreDTO.ToAnonymousObject());
        DatabaseHelper.SqliteConnection!.Close();
    }

    internal static bool UpdateCodingSessionDapper(CodingSessionUpdateDTO codingSessionUpdateDTO)
    {
        CodingSessionShowDTO? codingSession = FindCodingSession(codingSessionUpdateDTO.Id, codingSessionUpdateDTO.Username);

        if (codingSession != null)
        {
            DatabaseHelper.SqliteConnection!.Open();

            string query = "UPDATE CODING_SESSIONS SET description = @Description, start_date = @StartDateTime, end_date = @EndDateTime, duration_in_seconds = @DurationInSeconds WHERE id = @Id and username = @Username;";

            DatabaseHelper.SqliteConnection!.Execute(query, codingSessionUpdateDTO.ToAnonymousObject());

            DatabaseHelper.SqliteConnection!.Close();

            return true;
        }

        return false;
    }

    internal static bool UpdateCodingSession(CodingSessionUpdateDTO codingSessionUpdateDTO)
    {
        CodingSessionShowDTO? codingSession = FindCodingSession(codingSessionUpdateDTO.Id, codingSessionUpdateDTO.Username);

        if (codingSession != null)
        {
            DatabaseHelper.SqliteConnection!.Open();

            using (SQLiteCommand command = DatabaseHelper.CreateCommand())
            {
                command.CommandText = "UPDATE CODING_SESSIONS SET description = @description, start_date = @startDate, end_date = @endDate, duration_in_seconds = @durationInSeconds WHERE id = @id and username = @username;";

                command.Parameters.AddWithValue("id", codingSessionUpdateDTO.Id);
                command.Parameters.AddWithValue("description", codingSessionUpdateDTO.Description);
                command.Parameters.AddWithValue("startDate", codingSessionUpdateDTO.StartDateTime);
                command.Parameters.AddWithValue("endDate", codingSessionUpdateDTO.EndDateTime);
                command.Parameters.AddWithValue("username", codingSessionUpdateDTO.Username);
                command.Parameters.AddWithValue("durationInSeconds", codingSessionUpdateDTO.DurationInSeconds);

                command.ExecuteNonQuery();
            }

            DatabaseHelper.SqliteConnection!.Close();

            return true;
        }

        return false;
    }

    internal static bool DeleteCodingSession(int id, string username)
    {
        CodingSessionShowDTO? codingSession = FindCodingSession(id, username);

        if (codingSession != null)
        {
            DatabaseHelper.SqliteConnection!.Open();

            using (SQLiteCommand command = DatabaseHelper.CreateCommand())
            {
                command.CommandText = "DELETE FROM CODING_SESSIONS WHERE id = @id and username = @username;";

                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("username", username);

                command.ExecuteNonQuery();
            }

            DatabaseHelper.SqliteConnection!.Close();

            return true;
        }

        return false;
    }

    internal static bool DeleteCodingSessionDapper(int id, string username)
    {
        CodingSessionShowDTO? codingSession = FindCodingSession(id, username);

        if (codingSession != null)
        {
            DatabaseHelper.SqliteConnection!.Open();

            string query = "DELETE FROM CODING_SESSIONS WHERE id = @Id and username = @Username;";
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
