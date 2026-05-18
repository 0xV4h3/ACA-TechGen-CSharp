using MenuLib;
using MenuImplementation;

namespace Game;

class Program
{
    static void Main()
    {
        Console.Clear();
        Console.Write("Enter your username: ");
        string name;
        while (true)
        {
            name = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(name)) break;
            Console.Write("Invalid. Enter your username: ");
        }
        GameState.Username = name;
        
        var root = new MainMenu();
        MenuRunner.Run(root);
    }
}
