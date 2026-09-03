using System.Globalization;
using WelcomePage.Data;
using WelcomePage.DTOs;
using WelcomePage.Repositories;
using WelcomePage.Services;

namespace WelcomePage;

class Program
{
    static void Main(string[] args)
    {
        var dbContext = new DbContext();
        dbContext.EnsureSchema();

        var userRepository = new UserRepository(dbContext);
        var friendRepository = new FriendRepository(dbContext);
        var passwordService = new BcryptPasswordService();

        var userService = new UserService(dbContext, userRepository, friendRepository, passwordService);

        while (true)
        {
            Console.WriteLine("\n--- Main Page ---");
            Console.WriteLine("Select: ");
            Console.WriteLine("1 - Register");
            Console.WriteLine("2 - Login");
            Console.WriteLine("3 - Change Password");
            Console.WriteLine("4 - Delete Account");
            Console.WriteLine("5 - Exit");
            
            Console.Write("Option: ");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    Console.Write("UserName: ");
                    string userName = Console.ReadLine() ?? "";
                    
                    Console.Write("FirstName: ");
                    string firstName = Console.ReadLine() ?? "";
                    
                    Console.Write("LastName: ");
                    string lastName = Console.ReadLine() ?? "";
                    
                    DateOnly dateOfBirth;
                    string inputFormat = "yyyy-MM-dd";

                    Console.Write($"Enter your Date of Birth ({inputFormat}): ");
                    string? input = Console.ReadLine();

                    while (!DateOnly.TryParseExact(input, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid format. Please try again.");
                        Console.ResetColor();

                        Console.Write($"Enter your Date of Birth ({inputFormat}): ");
                        input = Console.ReadLine();
                    }
                    
                    Console.Write("Password: ");
                    string password = Console.ReadLine() ?? "";
                    var newUser = new UserRegisterDto(userName, firstName, lastName, dateOfBirth, password);
                    if (!userService.Register(newUser))
                    {
                        Console.WriteLine("Something went wrong (Username might be taken)");
                    }
                    else
                    {
                        Console.WriteLine($"User {userName} successfully registered");
                    }
                    break;

                case "2":
                    Console.Write("UserName: ");
                    string loginName = Console.ReadLine() ?? "";
                    
                    Console.Write("Password: ");
                    string loginPassword = Console.ReadLine() ?? "";
                    
                    var currentUser = userService.Login(loginName, loginPassword);
                    if (currentUser is null)
                    {
                        Console.WriteLine("Invalid username or password.");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\nSuccessfully logged in! Welcome, {currentUser.FirstName}.");
                        Console.ResetColor();

                        bool inUserSession = true;
                        while (inUserSession)
                        {
                            Console.WriteLine($"\n--- User Menu ({currentUser.UserName}) ---");
                            Console.WriteLine("1 - Show My Profile");
                            Console.WriteLine("2 - Show Friends List");
                            Console.WriteLine("3 - Add Friend by UserName");
                            Console.WriteLine("4 - Logout");
                            Console.Write("Option: ");
                            
                            var userOption = Console.ReadLine();
                            switch (userOption)
                            {
                                case "1":
                                    Console.WriteLine("\n--- Profile ---");
                                    Console.WriteLine($"ID: {currentUser.Id}\nUsername: {currentUser.UserName}\nName: {currentUser.FirstName} {currentUser.LastName}\nBirth Date: {currentUser.DateOfBirth}");
                                    break;

                                case "2":
                                    Console.WriteLine("\n--- Your Friends ---");
                                    List<UserResponseDto> friends = userService.GetFriends(currentUser.Id);
                                    if (friends.Count == 0)
                                    {
                                        Console.WriteLine("You don't have any friends yet.");
                                    }
                                    else
                                    {
                                        foreach (var friend in friends)
                                        {
                                            Console.WriteLine($"[ID: {friend.Id}] {friend.FirstName} {friend.LastName} (@{friend.UserName})");
                                        }
                                    }
                                    break;

                                case "3":
                                    Console.Write("Enter Friend's UserName: ");
                                    string targetFriendName = Console.ReadLine() ?? "";
    
                                    if (!string.IsNullOrWhiteSpace(targetFriendName))
                                    {
                                        userService.AddFriend(currentUser.Id, targetFriendName);
                                    }
                                    else
                                    {
                                        Console.WriteLine("UserName cannot be empty.");
                                    }
                                    break;


                                case "4":
                                    Console.WriteLine("Logging out...");
                                    inUserSession = false;
                                    break;

                                default:
                                    Console.WriteLine("Invalid option.");
                                    break;
                            }
                        }
                    }
                    break;

                case "3":
                    Console.Write("UserName: ");
                    string changeUserName = Console.ReadLine() ?? "";
                    
                    Console.Write("Old Password: ");
                    string oldPassword = Console.ReadLine() ?? "";
                    
                    Console.Write("New Password: ");
                    string newPassword = Console.ReadLine() ?? "";
                    
                    if(!userService.ChangePassword(changeUserName, oldPassword, newPassword))
                        Console.WriteLine("Failed to change password");
                    else
                        Console.WriteLine("Password changed successfully");
                    break;

                case "4":
                    Console.Write("UserName: ");
                    string deleteName = Console.ReadLine() ?? "";
                    
                    Console.Write("Password: ");
                    string deletePassword = Console.ReadLine() ?? "";
                    
                    if (!userService.DeleteUser(deleteName, deletePassword))
                        Console.WriteLine("Failed to delete");
                    else
                        Console.WriteLine($"User {deleteName} deleted");
                    break;

                case "5":
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }
}