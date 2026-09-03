using WelcomePage.Models;

namespace WelcomePage.Repositories;

public interface IUserRepository
{
    bool Add(User user);
    User? GetByUserName(string userName);
    User? GetById(int id);
    bool UpdatePassword(string userName, string newPasswordHash, DateTime updatedAt);
    bool Delete(string userName);
}