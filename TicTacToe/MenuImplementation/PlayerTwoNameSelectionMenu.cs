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
    
    protected override void InternalDisplay()
    {
        Console.WriteLine($"Current opponent : {GameState.Player2Name}");
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
                    Console.Write("Invalid. Enter opponent's username: ");
                }
                GameState.Player2Name = name;
                return NavigationResult.Refresh();
        }
        return NavigationResult.None();
    }
}