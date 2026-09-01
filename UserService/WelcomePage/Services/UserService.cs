using WelcomePage.DTOs;
using WelcomePage.Models;
using WelcomePage.Repositories;

namespace WelcomePage.Services;

public class UserService(IUserRepository userRepository, BcryptPasswordService passwordService) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly BcryptPasswordService _passwordService = passwordService;

    public bool Register(UserRegisterDto dto)
    {
        var passwordHash = _passwordService.HashPassword(dto.Password);
        var now = DateTime.UtcNow;

        var user = new User
        {
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            PasswordHash = passwordHash,
            CreatedAt = now,
            UpdatedAt = now
        };

        return _userRepository.Add(user);
    }

    public UserResponseDto? Login(string userName, string providedPassword)
    {
        var user = _userRepository.GetByUserName(userName);
        if (user is null) return null;

        if (!_passwordService.VerifyPassword(providedPassword, user.PasswordHash))
        {
            return null;
        }

        return MapToDto(user);
    }

    public bool ChangePassword(string userName, string oldPassword, string newPassword)
    {
        var user = _userRepository.GetByUserName(userName);
        if (user is null || !_passwordService.VerifyPassword(oldPassword, user.PasswordHash))
        {
            Console.WriteLine("[ERROR] Invalid old password or user not found");
            return false;
        }

        var newHash = _passwordService.HashPassword(newPassword);
        return _userRepository.UpdatePassword(userName, newHash, DateTime.UtcNow);
    }

    public bool DeleteUser(string userName, string providedPassword)
    {
        var user = _userRepository.GetByUserName(userName);
        if (user is null || !_passwordService.VerifyPassword(providedPassword, user.PasswordHash))
        {
            Console.WriteLine("[ERROR] Invalid password or user not found");
            return false;
        }

        return _userRepository.Delete(userName);
    }

    private static UserResponseDto MapToDto(User user) =>
        new(user.Id, user.UserName, user.FirstName, user.LastName, user.DateOfBirth, user.CreatedAt, user.UpdatedAt);
}