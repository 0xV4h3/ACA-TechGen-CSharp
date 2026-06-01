namespace AreaFill;

public static class AreaFill
{
    public static void Fill(int[][] grid, int startX, int startY, int value, string mode = "recursive")
    {
        switch (mode)
        {
           case "recursive":
               FillRecursive(grid, startX, startY, value);
               break;
           case "iterative":
               FillIterative(grid, startX, startY, value);
               break;
           default:
               Console.WriteLine("Unknown mode");
               break;
        }
    }
    
    public static void FillRecursive(int[][] grid, int startX, int startY, int value)
    {
        if (grid.Length == 0) return;

        int originalValue = grid[startX][startY];
        if(originalValue == value) return;
        
        FillRecursive(grid, startX, startY, originalValue, value);
    }

    private static void FillRecursive(int[][] grid, int x, int y, int originalValue, int newValue)
    {
        if (x < 0 || x >= grid.Length || y < 0 || y >= grid[0].Length)
            return;
        
        if (grid[x][y] != originalValue)
            return;
        
        grid[x][y] = newValue;
        
        FillRecursive(grid, x + 1, y, originalValue, newValue);
        FillRecursive(grid, x - 1, y, originalValue, newValue);
        FillRecursive(grid, x, y + 1, originalValue, newValue);
        FillRecursive(grid, x, y - 1, originalValue, newValue);
    }

    public static void FillIterative(int[][] grid, int startX, int startY, int value)
    {
        if (grid.Length == 0) return;

        int originalValue = grid[startX][startY];
        if(originalValue == value) return;

        Queue<(int, int)> queue = new();
        queue.Enqueue((startX, startY));

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            
            if (x < 0 || x >= grid.Length || y < 0 || y >= grid[0].Length)
                continue;
            
            if (grid[x][y] != originalValue)
                continue;

            grid[x][y] = value;
            
            queue.Enqueue((x + 1, y));
            queue.Enqueue((x - 1, y));
            queue.Enqueue((x, y + 1));
            queue.Enqueue((x, y - 1));
        }
    }

    public static void Test(string mode = "recursive")
    {
        if (mode != "recursive" && mode != "iterative") return;
        
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
        
        Fill(grid, 2, 2, 9, mode);

        foreach (int[] row in grid)
        {
            Console.WriteLine(string.Join(" ", row));
        }
        Console.WriteLine();
    }
}