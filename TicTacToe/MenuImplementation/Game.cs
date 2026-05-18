namespace MenuImplementation;

public static class Game
{
    public static void PlayTicTacToe()
    {
        Console.Clear();
        
        var game = new TicTacToe(
            GameState.IsVsComputer,
            GameState.Username,
            GameState.PlayerSign,
            GameState.UseWASD
        );

        game.PlayGame();
    }
}