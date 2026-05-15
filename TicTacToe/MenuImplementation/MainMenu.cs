using MenuLib;

namespace MenuImplementation;

// MainMenu.cs
public class MainMenu : Menu
{
    public MainMenu() : base("Main Menu")
    {
        ConfigureOptionSize(3);
        AddOption("1", "Play");
        AddOption("2", "Settings");
        AddOption("3", "About");
    }
    protected override NavigationResult HandleOption(string option)
    {
        switch (option)
        {
            case "1": return NavigationResult.GoTo(new GameplayModeSelectionMenu());
            case "2": return NavigationResult.GoTo(new SettingsMenu());
            case "3": return NavigationResult.GoTo(new AboutMenu());
        }
        return NavigationResult.None();
    }
}