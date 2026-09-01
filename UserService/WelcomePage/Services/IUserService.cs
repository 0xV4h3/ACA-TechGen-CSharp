using WelcomePage.DTOs;

namespace WelcomePage.Services;

public interface IUserService
{
    bool Register(UserRegisterDto dto);
    UserResponseDto? Login(string userName, string providedPassword);
    bool ChangePassword(string userName, string oldPassword, string newPassword);
    bool DeleteUser(string userName, string providedPassword);
}