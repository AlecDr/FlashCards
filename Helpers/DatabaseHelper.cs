using System.Configuration;
using System.Data.SqlClient;

namespace FlashCards.Helpers;

internal abstract class DatabaseHelper
{
    static SqlConnection? _sqliteConnection;

    internal static SqlConnection? SqliteConnection { get { return GetConnection(); } private set { SqliteConnection = value; } }

    private static SqlConnection GetConnection()
    {
        if (_sqliteConnection == null)
        {
            CreateDatabase();
            CreateConnection();
            CreateTables();
        }

        return _sqliteConnection!;
    }

    internal static void CreateConnection()
    {
        _sqliteConnection = new SqlConnection($"Server=localhost\\MSSQLSERVER02;Database={GetDatabaseName()};Trusted_Connection=True;");
    }

    private static string GetDatabaseName()
    {
        string databaseName = ConfigurationManager.AppSettings.Get("DatabaseName");

        return databaseName;
    }

    internal static SqlCommand CreateCommand()
    {
        return GetConnection().CreateCommand();
    }

    private static void CreateDatabase()
    {
        using (SqlConnection connection = new SqlConnection($"Server=localhost\\MSSQLSERVER02;Trusted_Connection=True;"))
        {
            connection.Open();
            object result;
            string databaseName = GetDatabaseName();

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT db_id('{databaseName}')";
                command.Parameters.AddWithValue("databaseName", databaseName);
                result = command.ExecuteScalar();
            }

            if (result == DBNull.Value)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"CREATE DATABASE {databaseName}";
                    command.ExecuteNonQuery();
                }
            }

            connection.Close();
        }
    }

    private static void CreateTables()
    {
        _sqliteConnection!.Open();

        // stacks table
        SqlCommand command = CreateCommand();

        command.CommandText = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'STACKS' AND schema_id = SCHEMA_ID('dbo'))
            BEGIN
                CREATE TABLE STACKS
                    (id INTEGER PRIMARY KEY, name VARCHAR(255) NOT NULL, username VARCHAR(255) NOT NULL);
            END;
            ";
        command.ExecuteNonQuery();

        _sqliteConnection.Close();
    }
}
