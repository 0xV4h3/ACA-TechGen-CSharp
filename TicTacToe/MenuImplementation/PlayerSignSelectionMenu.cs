using MenuLib;

namespace MenuImplementation;

public class PlayerSignSelectionMenu : Menu
{
    public PlayerSignSelectionMenu() : base("Choose X or O")
    {
        ConfigureOptionSize(2);
        AddOption("X", "Play as X");
        AddOption("O", "Play as O");
    }

    protected override NavigationResult HandleOption(string option)
    {
        if (option.ToUpper() == "X" || option.ToUpper() == "O")
        {
            GameSetupConfig.PlayerSign = option[0];
            return NavigationResult.GoTo(new PlayMenu());
        }
        return NavigationResult.None();
    }
}