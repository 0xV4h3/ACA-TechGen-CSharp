using System.Globalization;
using WelcomePage.Models;
using WelcomePage.Services;

namespace WelcomePage;

class Program
{
    static void Main(string[] args)
    {
        var passwordService = new BcryptPasswordService();
        var userService = new UserService(passwordService);
        
        userService.InitializeDatabase();

        while (true)
        {
            // Console.Clear();
            Console.WriteLine("Welcome page");
            Console.WriteLine("Select: ");
            Console.WriteLine("1 - Register");
            Console.WriteLine("2 - Login");
            Console.WriteLine("3 - Change Password?");
            Console.WriteLine("4 - Delete Account");
            Console.WriteLine("5 - Exit");
            
            Console.Write("Option: ");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    Console.Write("UserName: ");
                    string userName = Console.ReadLine();
                    
                    Console.Write("FirstName: ");
                    string firstName = Console.ReadLine();
                    
                    Console.Write("LastName: ");
                    string lastName = Console.ReadLine();
                    
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
                    string password = Console.ReadLine();
                    if (!userService.RegisterUser(userName, firstName, lastName, dateOfBirth, password))
                    {
                        Console.WriteLine("Something went wrong");
                    }
                    else
                    {
                        Console.WriteLine($"User {userName} successfully registered");
                    }
                    break;
                case "2":
                    Console.Write("UserName: ");
                    string loginName = Console.ReadLine();
                    
                    Console.Write("Password: ");
                    string loginPassword = Console.ReadLine();
                    
                    var user = userService.LoginUser(loginName, loginPassword);
                    if (user is null)
                        Console.WriteLine("Something not found");
                    else
                        Console.WriteLine(user);
                    break;
                case "3":
                    Console.Write("UserName: ");
                    string changeUserName = Console.ReadLine();
                    
                    Console.Write("Old Password: ");
                    string oldPassword = Console.ReadLine();
                    
                    Console.Write("New Password: ");
                    string newPassword = Console.ReadLine();
                    
                    if(!userService.ChangePassword( changeUserName, oldPassword,  newPassword))
                        Console.WriteLine("Failed to change password");
                    else
                        Console.WriteLine("Password changed successfully");
                    break;
                case "4":
                    Console.Write("UserName: ");
                    string deleteName = Console.ReadLine();
                    
                    Console.Write("Password: ");
                    string deletePassword = Console.ReadLine();
                    
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
