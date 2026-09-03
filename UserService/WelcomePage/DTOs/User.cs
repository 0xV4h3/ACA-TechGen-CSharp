namespace WelcomePage.DTOs;

public record UserResponseDto(
    int Id,
    string UserName,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record UserRegisterDto(
    string UserName,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Password
);