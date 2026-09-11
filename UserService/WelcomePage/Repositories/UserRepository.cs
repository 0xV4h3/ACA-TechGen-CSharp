// using System.Data;
// using Npgsql;
// using WelcomePage.Data;
// using WelcomePage.Models;
//
// namespace WelcomePage.Repositories;
//
// public class UserRepository(DbContext dbContext) : IUserRepository
// {
//     private readonly DbContext _dbContext = dbContext;
//
//     public bool Add(User user)
//     {
//         try
//         {
//             using var connection = _dbContext.CreateConnection();
//             connection.Open();
//
//             using var command = connection.CreateCommand();
//             command.CommandText = @"
//                 INSERT INTO Users (Username, PasswordHash, FirstName, LastName, DateOfBirth, CreatedAt, UpdatedAt) 
//                 VALUES (@userName, @passwordHash, @firstName, @lastName, @dateOfBirth, @createdAt, @updatedAt)";
//
//             AddParameter(command, "@userName", user.UserName);
//             AddParameter(command, "@passwordHash", user.PasswordHash);
//             AddParameter(command, "@firstName", user.FirstName);
//             AddParameter(command, "@lastName", user.LastName);
//             AddParameter(command, "@dateOfBirth", user.DateOfBirth.ToString("yyyy-MM-dd"));
//             AddParameter(command, "@createdAt", user.CreatedAt);
//             AddParameter(command, "@updatedAt", user.UpdatedAt);
//
//             command.ExecuteNonQuery();
//             return true;
//         }
//         catch (PostgresException ex) when (ex.SqlState == "23505")
//         {
//             return false;
//         }
//     }
//
//     public User? GetByUserName(string userName)
//     {
//         using var connection = _dbContext.CreateConnection();
//         connection.Open();
//
//         using var command = connection.CreateCommand();
//         command.CommandText = "SELECT UserId, Username, FirstName, LastName, DateOfBirth, PasswordHash, CreatedAt, UpdatedAt FROM Users WHERE Username = @userName";
//         AddParameter(command, "@userName", userName);
//
//         using var reader = command.ExecuteReader();
//         if (!reader.Read()) return null;
//
//         return MapReaderToUser(reader);
//     }
//
//     public User? GetById(int id)
//     {
//         using var connection = _dbContext.CreateConnection();
//         connection.Open();
//
//         using var command = connection.CreateCommand();
//         command.CommandText = "SELECT UserId, Username, FirstName, LastName, DateOfBirth, PasswordHash, CreatedAt, UpdatedAt FROM Users WHERE UserId = @id";
//         AddParameter(command, "@id", id);
//
//         using var reader = command.ExecuteReader();
//         if (!reader.Read()) return null;
//
//         return MapReaderToUser(reader);
//     }
//
//     public bool UpdatePassword(string userName, string newPasswordHash, DateTime updatedAt)
//     {
//         using var connection = _dbContext.CreateConnection();
//         connection.Open();
//
//         using var command = connection.CreateCommand();
//         command.CommandText = "UPDATE Users SET PasswordHash = @newHash, UpdatedAt = @updatedAt WHERE Username = @userName";
//         AddParameter(command, "@newHash", newPasswordHash);
//         AddParameter(command, "@updatedAt", updatedAt);
//         AddParameter(command, "@userName", userName);
//
//         return command.ExecuteNonQuery() > 0;
//     }
//
//     public bool Delete(string userName)
//     {
//         using var connection = _dbContext.CreateConnection();
//         connection.Open();
//
//         using var command = connection.CreateCommand();
//         command.CommandText = "DELETE FROM Users WHERE Username = @userName";
//         AddParameter(command, "@userName", userName);
//
//         return command.ExecuteNonQuery() > 0;
//     }
//
//     private static void AddParameter(IDbCommand command, string name, object value)
//     {
//         var parameter = command.CreateParameter();
//         parameter.ParameterName = name;
//         parameter.Value = value;
//         command.Parameters.Add(parameter);
//     }
//
//     private static User MapReaderToUser(IDataReader reader) => new()
//     {
//         Id = reader.GetInt32(0),
//         UserName = reader.GetString(1),
//         FirstName = reader.GetString(2),
//         LastName = reader.GetString(3),
//         DateOfBirth = DateOnly.Parse(reader.GetString(4)),
//         PasswordHash = reader.GetString(5),
//         CreatedAt = reader.GetDateTime(6),
//         UpdatedAt = reader.GetDateTime(7)
//     };
// }