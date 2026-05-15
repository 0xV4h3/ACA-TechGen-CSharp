using MenuLib;
using MenuImplementation;

namespace TicTacToe;

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
        GameSetupConfig.Username = name;
        
        var root = new MainMenu();
        MenuRunner.Run(root);
    }
}
