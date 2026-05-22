namespace MenuImplementation;

public static class GameState
{
    public static string Player1Name { get; set; } = "Player1";
    public static string Player2Name { get; set; } = "Player2";
    public static char PlayerSign { get; set; } = 'X';
    public static bool UseWASD { get; set; } = true;
    public static bool IsVsComputer { get; set; } = false;
}