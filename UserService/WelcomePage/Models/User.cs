namespace WelcomePage.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string DateOfBirth { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Friend> FriendsOf { get; set; } = new List<Friend>();
    public ICollection<Friend> FriendsWith { get; set; } = new List<Friend>();
    
    // public override string ToString()
    // {
    //     return $"Id: {Id}\nUsername: {UserName}\nFirstName: {FirstName}\nLastName: {LastName}\nDate of birth: {DateOfBirth}\nCreated at {CreatedAt}";
    // }
}