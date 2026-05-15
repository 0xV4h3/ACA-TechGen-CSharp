using MenuLib;

namespace MenuImplementation;

public class AboutMenu : Menu
{
    public AboutMenu() : base("About") { }
    protected override void InternalDisplay()
    {
        Console.WriteLine("TicTacToe by Vahe Babayan, Tech Gen C#, 2026");
    }
    protected override NavigationResult HandleOption(string option)
    {
        return NavigationResult.None();
    }
}