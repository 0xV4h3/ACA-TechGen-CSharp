// using System.Data;
// using WelcomePage.Data;
// using WelcomePage.Models;
//
// namespace WelcomePage.Repositories;
//
// public class FriendRepository(DbContext dbContext) : IFriendRepository
// {
//     private readonly DbContext _dbContext = dbContext;
//
//     public bool AreFriends(int userId, int friendId)
//     {
//         using var connection = _dbContext.CreateConnection();
//         connection.Open();
//
//         using var command = connection.CreateCommand();
//         command.CommandText = "SELECT COUNT(1) FROM Friends WHERE UserId = @userId AND FriendUserId = @friendId";
//         
//         AddParameter(command, "@userId", userId);
//         AddParameter(command, "@friendId", friendId);
//
//         long count = (long)command.ExecuteScalar()!;
//         return count > 0;
//     }
//
//     public void AddFriendshipPair(int userId, int friendId, IDbTransaction transaction)
//     {
//         var command = transaction.Connection!.CreateCommand();
//         command.Transaction = transaction;
//         command.CommandText = "INSERT INTO Friends (UserId, FriendUserId, CreatedAt) VALUES (@userId, @friendId, @createdAt)";
//
//         AddParameter(command, "@userId", userId);
//         AddParameter(command, "@friendId", friendId);
//         AddParameter(command, "@createdAt", DateTime.UtcNow);
//
//         command.ExecuteNonQuery();
//     }
//     
//     public List<User> GetFriendsByUserId(int userId)
//     {
//         var friends = new List<User>();
//         using var connection = _dbContext.CreateConnection();
//         connection.Open();
//
//         using var command = connection.CreateCommand();
//         command.CommandText = @"
//         SELECT u.UserId, u.Username, u.FirstName, u.LastName, u.DateOfBirth, u.PasswordHash, u.CreatedAt, u.UpdatedAt 
//         FROM Friends f
//         JOIN Users u ON f.FriendUserId = u.UserId
//         WHERE f.UserId = @userId";
//
//         AddParameter(command, "@userId", userId);
//
//         using var reader = command.ExecuteReader();
//         while (reader.Read())
//         {
//             friends.Add(MapReaderToUser(reader));
//         }
//
//         return friends;
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