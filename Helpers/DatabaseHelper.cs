using System.Configuration;
using System.Data.SQLite;

namespace FlashCards.Helpers;

internal abstract class DatabaseHelper
{
    static SQLiteConnection? _sqliteConnection;

    internal static SQLiteConnection? SqliteConnection { get { return GetConnection(); } private set { SqliteConnection = value; } }

    private static SQLiteConnection GetConnection()
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
        _sqliteConnection = new SQLiteConnection($"Data Source={GetDatabasePath()};Version=3;");
    }

    private static string GetDatabasePath()
    {
        string projectFolder = Environment.CurrentDirectory;
        string databaseName = ConfigurationManager.AppSettings.Get("DatabaseName");
        string databasePath = System.IO.Path.Combine(projectFolder, databaseName);

        return databasePath;
    }

    internal static SQLiteCommand CreateCommand()
    {
        return GetConnection().CreateCommand();
    }

    private static void CreateDatabase()
    {
        if (!File.Exists(GetDatabasePath()))
        {
            SQLiteConnection.CreateFile(GetDatabasePath());
        }
    }

    private static void CreateTables()
    {
        _sqliteConnection!.Open();

        // stacks table
        SQLiteCommand command = CreateCommand();

        command.CommandText = "CREATE TABLE IF NOT EXISTS STACKS(id INTEGER PRIMARY KEY AUTOINCREMENT, description VARCHAR(255), username VARCHAR(255) NOT NULL)";
        command.ExecuteNonQuery();

        _sqliteConnection.Close();
    }
}
