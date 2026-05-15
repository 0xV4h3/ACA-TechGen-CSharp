using MenuLib;

namespace MenuImplementation;

public class PlayMenu : Menu
{
    public PlayMenu() : base("Gameplay Mode")
    {
        ConfigureOptionSize(1);
        AddOption("1", "Start");
    }

    protected override NavigationResult HandleOption(string option)
    {
        if (option == "1")
        {
            GameSetupConfig.IsVsComputer = (option == "2");
            
            Console.Clear();
            
            var game = new TicTacToe(
                GameSetupConfig.IsVsComputer,
                GameSetupConfig.Username,
                GameSetupConfig.PlayerSign,
                GameSetupConfig.UseWASD
            );

            game.PlayGame();

            Console.WriteLine("Press any key to return to Main Menu...");
            Console.ReadKey(true);
            return NavigationResult.Back();
        }
        return NavigationResult.None();
    }
}