using Microsoft.Data.Sqlite;
using WelcomePage.Models;

namespace WelcomePage.Services;

public class UserService(BcryptPasswordService passwordService)
{
    private const string ConnectionString = "Data Source=app.db";
    private readonly BcryptPasswordService _passwordService = passwordService;

    public void InitializeDatabase()
    {
        using var connection = new SqliteConnection(ConnectionString);
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
    
    public bool RegisterUser(string userName, string firstName, string lastName, DateOnly dateOfBirth, string rawPassword)
    {
        try
        {
            var hashedPassword = _passwordService.HashPassword(rawPassword);
            var now = DateTime.UtcNow;

            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Users (UserName, FirstName, LastName, DateOfBirth, PasswordHash, CreatedAt, UpdatedAt) 
                VALUES ($userName, $firstName, $lastName, $dateOfBirth, $passwordHash, $createdAt, $updatedAt)";

            command.Parameters.AddWithValue("$userName", userName);
            command.Parameters.AddWithValue("$firstName", firstName);
            command.Parameters.AddWithValue("$lastName", lastName);
            command.Parameters.AddWithValue("$dateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("$passwordHash", hashedPassword);
            command.Parameters.AddWithValue("$createdAt", now.ToString("o"));
            command.Parameters.AddWithValue("$updatedAt", now.ToString("o"));

            command.ExecuteNonQuery();
            return true;
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
        {
            return false;
        }
    }

    public User? LoginUser(string userName, string providedPassword)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, UserName, FirstName, LastName, DateOfBirth, PasswordHash, CreatedAt, UpdatedAt FROM Users WHERE UserName = $userName";
        command.Parameters.AddWithValue("$userName", userName);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var storedHash = reader.GetString(5);

            if (_passwordService.VerifyPassword(providedPassword, storedHash))
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),
                    DateOfBirth = DateOnly.Parse(reader.GetString(4)),
                    PasswordHash = storedHash,
                    CreatedAt = DateTime.Parse(reader.GetString(6)),
                    UpdatedAt = DateTime.Parse(reader.GetString(7))
                };
            }
        }

        return null;
    }

    public bool ChangePassword(string userName, string oldPassword, string newPassword)
    {
        var user = LoginUser(userName, oldPassword);
        if (user is null)
        {
            Console.WriteLine("[ERROR] Invalid password or user not found");
            return false;
        }

        var newHash = _passwordService.HashPassword(newPassword);
        var now = DateTime.UtcNow;

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Users SET PasswordHash = $newHash, UpdatedAt = $updatedAt WHERE UserName = $userName";
        command.Parameters.AddWithValue("$newHash", newHash);
        command.Parameters.AddWithValue("$updatedAt", now.ToString("o"));
        command.Parameters.AddWithValue("$userName", userName);

        command.ExecuteNonQuery();
        return true;
    }

    public bool DeleteUser(string userName, string providedPassword)
    {
        var user = LoginUser(userName, providedPassword);
        if (user is null)
        {
            Console.WriteLine("[ERROR] Invalid password or user not found");
            return false;
        }
        
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Users WHERE UserName = $userName";
        command.Parameters.AddWithValue("$userName", userName);
        int deleted = command.ExecuteNonQuery();
        if (deleted == 0)
            return false;

        return true;
    }
}