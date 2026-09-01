using Microsoft.Data.Sqlite;
using WelcomePage.Data;
using WelcomePage.Models;

namespace WelcomePage.Repositories;

public class UserRepository(DbContext dbContext) : IUserRepository
{
    private readonly DbContext _dbContext = dbContext;

    public bool Add(User user)
    {
        try
        {
            using var connection = _dbContext.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Users (UserName, FirstName, LastName, DateOfBirth, PasswordHash, CreatedAt, UpdatedAt) 
                VALUES ($userName, $firstName, $lastName, $dateOfBirth, $passwordHash, $createdAt, $updatedAt)";

            command.Parameters.AddWithValue("$userName", user.UserName);
            command.Parameters.AddWithValue("$firstName", user.FirstName);
            command.Parameters.AddWithValue("$lastName", user.LastName);
            command.Parameters.AddWithValue("$dateOfBirth", user.DateOfBirth.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("$passwordHash", user.PasswordHash);
            command.Parameters.AddWithValue("$createdAt", user.CreatedAt.ToString("o"));
            command.Parameters.AddWithValue("$updatedAt", user.UpdatedAt.ToString("o"));

            command.ExecuteNonQuery();
            return true;
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
        {
            return false;
        }
    }

    public User? GetByUserName(string userName)
    {
        using var connection = _dbContext.CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.Connection = connection;
        command.CommandText = "SELECT Id, UserName, FirstName, LastName, DateOfBirth, PasswordHash, CreatedAt, UpdatedAt FROM Users WHERE UserName = $userName";
        command.Parameters.AddWithValue("$userName", userName);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;

        return new User
        {
            Id = reader.GetInt32(0),
            UserName = reader.GetString(1),
            FirstName = reader.GetString(2),
            LastName = reader.GetString(3),
            DateOfBirth = DateOnly.Parse(reader.GetString(4)),
            PasswordHash = reader.GetString(5),
            CreatedAt = DateTime.Parse(reader.GetString(6)),
            UpdatedAt = DateTime.Parse(reader.GetString(7))
        };
    }

    public bool UpdatePassword(string userName, string newPasswordHash, DateTime updatedAt)
    {
        using var connection = _dbContext.CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Users SET PasswordHash = $newHash, UpdatedAt = $updatedAt WHERE UserName = $userName";
        command.Parameters.AddWithValue("$newHash", newPasswordHash);
        command.Parameters.AddWithValue("$updatedAt", updatedAt.ToString("o"));
        command.Parameters.AddWithValue("$userName", userName);

        int rowsAffected = command.ExecuteNonQuery();
        return rowsAffected > 0;
    }

    public bool Delete(string userName)
    {
        using var connection = _dbContext.CreateConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Users WHERE UserName = $userName";
        command.Parameters.AddWithValue("$userName", userName);

        int rowsAffected = command.ExecuteNonQuery();
        return rowsAffected > 0;
    }
}
