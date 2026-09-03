using System.Data;
using WelcomePage.Models;

namespace WelcomePage.Repositories;

public interface IFriendRepository
{
    bool AreFriends(int userId, int friendId);
    void AddFriendshipPair(int userId, int friendId, IDbTransaction transaction);
    List<User> GetFriendsByUserId(int userId);
}