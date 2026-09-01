namespace WelcomePage.Models;

public class User
{
    public int Id { get; init; }
    public required string UserName { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public DateOnly DateOfBirth { get; init; } 
    public required string PasswordHash { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}\nUsername: {UserName}\nFirstName: {FirstName}\nLastName: {LastName}\nDate of birth: {DateOfBirth}\nCreated at {CreatedAt}";
    }
}