using System;

namespace MenuImplementation;

public class TicTacToe
{
    private char[] board = new char[9];
    private int currentPlayer;
    private string player1Name;
    private string player2Name;
    private char player1Sign;
    private char player2Sign;
    private bool isVsComputer;
    private bool useWASD;
    private int cursorPos;

    public TicTacToe(bool isVsComputer, string playerOne, string playerTwo, char playerSign, bool useWASD)
    {
        this.isVsComputer = isVsComputer;
        player1Name  = string.IsNullOrEmpty(playerOne) ? "Player1" : playerOne;
        player2Name  = isVsComputer ? "Computer" : string.IsNullOrEmpty(playerOne) ? "Player2" : playerTwo;
        player1Sign  = playerSign;
        player2Sign  = playerSign == 'X' ? 'O' : 'X';
        this.useWASD = useWASD;
        board = new char[] { '1','2','3','4','5','6','7','8','9' };
        currentPlayer = 1;
        cursorPos = 4;
    }
    
    public void PlayGame()
    { 
        int gameStatus;
        do
        {
            Console.Clear();
            DrawTitle();
            DrawBoard();
            DrawHelp();
            DrawTurn();

            if (isVsComputer && currentPlayer == 2)
            {
                System.Threading.Thread.Sleep(500);
                int move = GetBestMove();
                board[move] = player2Sign;
            }
            else
            {
                HandleNavigationAndMove();
            }

            gameStatus = CheckWinStatus();
            currentPlayer = currentPlayer == 1 ? 2 : 1;
        }
        while (gameStatus == 0);

        Console.Clear();
        DrawTitle();
        DrawBoard();
        PrintResult(gameStatus);
    }

    private void DrawTitle()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{player1Name} ({player1Sign}) vs {player2Name} ({player2Sign})\n");
        Console.ResetColor();
    }

    private void DrawBoard()
    {
        var origColor = Console.ForegroundColor;
        for (int row = 0; row < 3; row++)
        {
            Console.Write("  ");
            for (int col = 0; col < 3; col++)
            {
                int idx = row * 3 + col;
                if (idx == cursorPos && !isVsComputer || (idx == cursorPos && !(isVsComputer && currentPlayer == 2)))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(board[idx]);
                    Console.ResetColor();
                    Console.ForegroundColor = origColor;
                }
                else
                {
                    Console.Write(board[idx]);
                }
                if (col < 2) Console.Write(" | ");
            }
            Console.WriteLine();
            if (row < 2) Console.WriteLine(" ---+---+---");
        }
    }

    private void DrawHelp()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"\nUse Arrow Keys ↑↓←→{(useWASD ? " or WASD" : "")} to navigate, Enter to place your symbol.\n");
        Console.ResetColor();
    }

    private void DrawTurn()
    {
        string curName = currentPlayer == 1 ? player1Name : player2Name;
        char curSign  = currentPlayer == 1 ? player1Sign : player2Sign;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Current Turn: {curName} ({curSign})\n");
        Console.ResetColor();
    }

    private void HandleNavigationAndMove()
    {
        while (true)
        {
            int tempCursor = cursorPos;
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.LeftArrow || (useWASD && key.Key == ConsoleKey.A))
                cursorPos = cursorPos % 3 == 0 ? cursorPos + 2 : cursorPos - 1;
            else if (key.Key == ConsoleKey.RightArrow || (useWASD && key.Key == ConsoleKey.D))
                cursorPos = cursorPos % 3 == 2 ? cursorPos - 2 : cursorPos + 1;
            else if (key.Key == ConsoleKey.UpArrow || (useWASD && key.Key == ConsoleKey.W))
                cursorPos = cursorPos < 3 ? cursorPos + 6 : cursorPos - 3;
            else if (key.Key == ConsoleKey.DownArrow || (useWASD && key.Key == ConsoleKey.S))
                cursorPos = cursorPos > 5 ? cursorPos - 6 : cursorPos + 3;
            else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                if (board[cursorPos] != 'X' && board[cursorPos] != 'O')
                {
                    board[cursorPos] = currentPlayer == 1 ? player1Sign : player2Sign;
                    break;
                }
                else
                {
                    int safeY = Math.Min( Console.WindowTop + Console.WindowHeight - 2, Console.BufferHeight - 1 );
                    Console.SetCursorPosition(0, safeY);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Cell already occupied!");
                    Console.ResetColor();
                    System.Threading.Thread.Sleep(650);
                    Console.SetCursorPosition(0, safeY);
                    Console.Write(new string(' ', 40));
                    Console.SetCursorPosition(0, 0);
                }
            }
            if (tempCursor != cursorPos)
            {
                Console.SetCursorPosition(0, 2);
                DrawBoard();
            }
        }
    }

    private int CheckWinStatus()
    {
        int[,] winLines = {
            {0,1,2}, {3,4,5}, {6,7,8},
            {0,3,6}, {1,4,7}, {2,5,8},
            {0,4,8}, {2,4,6}
        };
        for (int i = 0; i < winLines.GetLength(0); i++)
        {
            int a = winLines[i,0], b = winLines[i,1], c = winLines[i,2];
            if (board[a] == board[b] && board[b] == board[c])
                return 1;
        }
        foreach (var c in board)
            if (c != 'X' && c != 'O') return 0;
        return -1;
    }

    private void PrintResult(int status)
    {
        if (status == 1)
        {
            int winner = (currentPlayer == 2) ? 1 : 2;
            string winnerName = winner == 1 ? player1Name : player2Name;
            char winSign = winner == 1 ? player1Sign : player2Sign;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{winnerName} ({winSign}) wins!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nDraw!");
        }
        Console.ResetColor();
    }
    
    private int GetBestMove()
    {
        int bestScore = int.MinValue, move = -1;
        for (int i = 0; i < 9; i++)
        {
            if (board[i] != 'X' && board[i] != 'O')
            {
                char orig = board[i];
                board[i] = player2Sign;
                int score = Minimax(0, false);
                board[i] = orig;
                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }
        return move;
    }

    private int Minimax(int depth, bool isMax)
    {
        int status = CheckWinStatus();
        if (status == 1)
            return isMax ? (-10 + depth) : (10 - depth);
        if (status == -1) return 0;
        char mark = isMax ? player2Sign : player1Sign;
        int bestScore = isMax ? int.MinValue : int.MaxValue;
        for (int i = 0; i < 9; i++)
        {
            if (board[i] != 'X' && board[i] != 'O')
            {
                char orig = board[i];
                board[i] = mark;
                int score = Minimax(depth + 1, !isMax);
                board[i] = orig;
                if (isMax) bestScore = Math.Max(score, bestScore);
                else bestScore = Math.Min(score, bestScore);
            }
        }
        return bestScore;
    }
}