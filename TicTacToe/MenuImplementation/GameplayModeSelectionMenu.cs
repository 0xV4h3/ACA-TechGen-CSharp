using MenuLib;

namespace MenuImplementation;

public class GameplayModeSelectionMenu : Menu
{
    public GameplayModeSelectionMenu() : base(MenuTitles.GameplayModeSelectionMenuTitle)
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
                GameState.IsVsComputer = false;
                return NavigationResult.GoTo(new PlayerSignSelectionMenu());
            case "2":
                GameState.IsVsComputer = true;
                return NavigationResult.GoTo(new PlayerSignSelectionMenu());
        }
        return NavigationResult.None();
    }
}