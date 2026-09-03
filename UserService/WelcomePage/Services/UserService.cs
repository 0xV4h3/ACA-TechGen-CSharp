using System;
using WelcomePage.Data;
using WelcomePage.DTOs;
using WelcomePage.Models;
using WelcomePage.Repositories;

namespace WelcomePage.Services;

public class UserService(
    DbContext dbContext,
    IUserRepository userRepository, 
    IFriendRepository friendRepository,
    BcryptPasswordService passwordService) : IUserService
{
    private readonly DbContext _dbContext = dbContext;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IFriendRepository _friendRepository = friendRepository;
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

        return new UserResponseDto(
            user.Id, 
            user.UserName, 
            user.FirstName, 
            user.LastName, 
            user.DateOfBirth, 
            user.CreatedAt, 
            user.UpdatedAt
        );
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

    public bool AddFriend(int currentUserId, string friendUserName)
    {
        var friend = _userRepository.GetByUserName(friendUserName);
        if (friend is null)
        {
            Console.WriteLine($"[ERROR] User with username '{friendUserName}' not found.");
            return false;
        }

        int friendId = friend.Id;

        if (currentUserId == friendId)
        {
            Console.WriteLine("[ERROR] You cannot be friends with yourself.");
            return false;
        }

        if (_friendRepository.AreFriends(currentUserId, friendId))
        {
            Console.WriteLine($"[ERROR] You are already friends with '{friendUserName}'.");
            return false;
        }

        using var connection = _dbContext.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            _friendRepository.AddFriendshipPair(currentUserId, friendId, transaction);
            _friendRepository.AddFriendshipPair(friendId, currentUserId, transaction);

            transaction.Commit();
            Console.WriteLine($"[SUCCESS] You and '{friendUserName}' are now friends!");
            return true;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"[TRANSACTION ROLLBACK] Failed to create friendship: {ex.Message}");
            return false;
        }
    }
    
    public List<UserResponseDto> GetFriends(int userId)
    {
        var friends = _friendRepository.GetFriendsByUserId(userId);
    
        return friends.Select(f => new UserResponseDto(
            f.Id, 
            f.UserName, 
            f.FirstName, 
            f.LastName, 
            f.DateOfBirth, 
            f.CreatedAt, 
            f.UpdatedAt
        )).ToList();
    }
}