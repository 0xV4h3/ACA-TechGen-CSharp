using MenuLib;

namespace MenuImplementation;

public class PlayerTwoNameSelectionMenu : Menu
{
    public PlayerTwoNameSelectionMenu() : base(MenuTitles.PlayerTwoNameSelectionMenuTitle)
    {
        ConfigureOptionSize(2);
        AddOption("1", "Start");
        AddOption("2", "Change Opponent");
    }
    
    public override void Display()
    {
        Console.Clear();
        Console.WriteLine($"=== {Title} ===");
        Console.WriteLine($"Current opponent : {GameState.Player2Name}");

        if (_options != null)
        {
            foreach (var option in _options)
            {
                Console.WriteLine($"{option.Key} - {option.Value}");
            }
        }

        InternalDisplay();

        Console.WriteLine("\n--- Navigation ---");
        Console.WriteLine("Type 'refresh' to refresh.");
        Console.WriteLine("Type 'back' to go back.");
        Console.WriteLine("Type 'exit' to exit.");
    }

    protected override NavigationResult HandleOption(string option)
    {
        switch (option)
        {
            case "1":
                return NavigationResult.GoTo(new PlayerSignSelectionMenu());
            case "2":
                Console.Write("Enter opponent's username: ");
                string name;
                while (true)
                {
                    name = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(name)) break;
                    Console.Write("Invalid. Enter your username: ");
                }
                GameState.Player2Name = name;
                return NavigationResult.Refresh();
        }
        return NavigationResult.None();
    }
}