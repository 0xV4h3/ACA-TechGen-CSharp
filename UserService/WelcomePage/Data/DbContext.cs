using Microsoft.Data.Sqlite;

namespace WelcomePage.Data;

public class DbContext
{
    private const string ConnectionString = "Data Source=app.db";

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(ConnectionString);
    }

    public void InitializeDatabase()
    {
        using var connection = CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserName TEXT UNIQUE NOT NULL,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                DateOfBirth TEXT NOT NULL,
                PasswordHash TEXT NOT NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL
            );";
        command.ExecuteNonQuery();
    }
}