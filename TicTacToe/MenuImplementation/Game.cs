namespace MenuImplementation;

public static class Game
{
    public static void PlayTicTacToe()
    {
        Console.Clear();
        
        var game = new TicTacToe(
            GameState.IsVsComputer,
            GameState.Player1Name,
            GameState.Player2Name,
            GameState.PlayerSign,
            GameState.UseWASD
        );

        game.PlayGame();
    }
}