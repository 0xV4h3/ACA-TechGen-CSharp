using MenuLib;

namespace MenuImplementation;

public class PlayerSignSelectionMenu : Menu
{
    public PlayerSignSelectionMenu() : base(MenuTitles.PlayerSignSelectionMenuTitle)
    {
        ConfigureOptionSize(2);
        AddOption("1", "Play as X");
        AddOption("2", "Play as O");
    }

    protected override NavigationResult HandleOption(string option)
    {
        switch (option)
        {
            case "1":
                GameState.PlayerSign = 'X';
                Game.PlayTicTacToe();
                Console.WriteLine("Press any key to return to Main Menu...");
                Console.ReadKey(true);
                return NavigationResult.ToRoot();
                // return NavigationResult.Jump(MenuTitles.MainMenuTitle);
            case "2":
                GameState.PlayerSign = 'O';
                Game.PlayTicTacToe();
                Console.WriteLine("Press any key to return to Main Menu...");
                Console.ReadKey(true);
                return NavigationResult.ToRoot();
                // return NavigationResult.Jump(MenuTitles.MainMenuTitle);
        }
        return NavigationResult.None();
    }
}