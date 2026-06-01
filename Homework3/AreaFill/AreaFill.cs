namespace AreaFill;

public static class AreaFill
{
    public static void Fill(int[][] grid, int startX, int startY, int value)
    {
        if (grid.Length == 0) return;

        int originalValue = grid[startX][startY];
        if(originalValue == value) return;
        
        Fill(grid, startX, startY, originalValue, value);
    }

    private static void Fill(int[][] grid, int x, int y, int originalValue, int newValue)
    {
        if (x < 0 || x >= grid.Length || y < 0 || y >= grid[0].Length)
            return;
        
        if (grid[x][y] != originalValue)
            return;
        
        grid[x][y] = newValue;
        
        Fill(grid, x + 1, y, originalValue, newValue);
        Fill(grid, x - 1, y, originalValue, newValue);
        Fill(grid, x, y + 1, originalValue, newValue);
        Fill(grid, x, y - 1, originalValue, newValue);
    }

    public static void RecursiveTest()
    {
        int[][] grid = [
            [1, 1, 1, 0, 0, 2, 2, 2],
            [1, 5, 5, 0, 0, 2, 3, 3],
            [1, 5, 5, 5, 0, 2, 3, 3],
            [0, 5, 5, 5, 0, 2, 2, 2],
            [0, 0, 0, 0, 0, 4, 4, 4],
            [7, 7, 0, 8, 8, 4, 6, 6],
            [7, 7, 0, 8, 8, 4, 6, 6],
            [7, 7, 0, 0, 0, 4, 6, 6]
        ];
        
        Fill(grid, 2, 2, 9);

        foreach (int[] row in grid)
        {
            Console.WriteLine(string.Join(" ", row));
        }
    }
}