using MenuLib;

namespace MenuImplementation;

public class SettingsMenu : Menu
{
    public SettingsMenu() : base(MenuTitles.SettingsMenuTitle)
    {
        ConfigureOptionSize(3);
        AddOption("1", "Change Username");
        AddOption("2", "Toggle WASD controls");
        AddOption("3", "Change Default Sign (X/O)");
    }

    protected override NavigationResult HandleOption(string option)
    {
        switch(option)
        {
            case "1":
                Console.Write("Enter new username: ");
                var name = Console.ReadLine();
                if(!string.IsNullOrWhiteSpace(name)) GameState.Username = name;
                return NavigationResult.Wait();
            case "2":
                GameState.UseWASD = !GameState.UseWASD;
                Console.WriteLine($"WASD Enabled: {GameState.UseWASD}");
                return NavigationResult.Wait();
            case "3":
                Console.Write("Choose default sign (X/O): ");
                var sign = Console.ReadLine();
                if (sign is not null && (sign.ToUpper() == "X" || sign.ToUpper() == "O"))
                    GameState.PlayerSign = sign.ToUpper()[0];
                return NavigationResult.Wait();
        }
        return NavigationResult.None();
    }
}