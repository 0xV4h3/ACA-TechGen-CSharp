using MenuLib;

namespace MenuImplementation;

public class GameplayModeSelectionMenu : Menu
{
    public GameplayModeSelectionMenu() : base("Game Mode")
    {
        ConfigureOptionSize(2);
        AddOption("1", "Player vs Player");
        AddOption("2", "Player vs Computer");
    }

    protected override NavigationResult HandleOption(string option)
    {
        switch (option)
        {
            case "1":
                GameSetupConfig.IsVsComputer = false;
                return NavigationResult.GoTo(new PlayerSignSelectionMenu());
            case "2":
                GameSetupConfig.IsVsComputer = true;
                return NavigationResult.GoTo(new PlayerSignSelectionMenu());
        }
        return NavigationResult.None();
    }
}