namespace AreaFill;

public static class AreaFill
{
    public static void Fill(int[,] grid, int startX, int startY, int value, string mode = "recursive")
    {
        if (grid == null || grid.GetLength(0) == 0 || grid.GetLength(1) == 0) return;
        
        if (startX < 0 || startX >= grid.GetLength(0) || startY < 0 || startY >= grid.GetLength(1))
            return;
        
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
    
    private static void FillRecursive(int[,] grid, int startX, int startY, int value)
    {
        // if (grid == null || grid.GetLength(0) == 0 || grid.GetLength(1) == 0) return;
        
        // if (startX < 0 || startX >= grid.GetLength(0) || startY < 0 || startY >= grid.GetLength(1))
        //     return;

        int originalValue = grid[startX, startY];
        if(originalValue == value) return;
        
        FillRecursive(grid, startX, startY, originalValue, value);
    }

    private static void FillRecursive(int[,] grid, int x, int y, int originalValue, int newValue)
    {
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1))
            return;
        
        if (grid[x, y] != originalValue)
            return;
        
        grid[x, y] = newValue;
        
        FillRecursive(grid, x + 1, y, originalValue, newValue);
        FillRecursive(grid, x - 1, y, originalValue, newValue);
        FillRecursive(grid, x, y + 1, originalValue, newValue);
        FillRecursive(grid, x, y - 1, originalValue, newValue);
    }

    private static void FillIterative(int[,] grid, int startX, int startY, int value)
    {
        // if (grid == null || grid.GetLength(0) == 0 || grid.GetLength(1) == 0) return;
        
        // if (startX < 0 || startX >= grid.GetLength(0) || startY < 0 || startY >= grid.GetLength(1))
        //     return;

        int originalValue = grid[startX, startY];
        if (originalValue == value) return;

        Queue<(int x, int y)> queue = new();
        queue.Enqueue((startX, startY));
        grid[startX, startY] = value;

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            if (x + 1 < grid.GetLength(0) && grid[x + 1, y] == originalValue)
            {
                grid[x + 1, y] = value;
                queue.Enqueue((x + 1, y));
            }

            if (x - 1 >= 0 && grid[x - 1, y] == originalValue)
            {
                grid[x - 1, y] = value;
                queue.Enqueue((x - 1, y));
            }

            if (y + 1 < grid.GetLength(1) && grid[x, y + 1] == originalValue)
            {
                grid[x, y + 1] = value;
                queue.Enqueue((x, y + 1));
            }

            if (y - 1 >= 0 && grid[x, y - 1] == originalValue)
            {
                grid[x, y - 1] = value;
                queue.Enqueue((x, y - 1));
            }
        }
    }

    public static void Test(string mode = "recursive")
    {
        if (mode != "recursive" && mode != "iterative") return;
        
        int[,] grid = {
            { 1, 1, 1, 0, 0, 2, 2, 2 },
            { 1, 5, 5, 0, 0, 2, 3, 3 },
            { 1, 5, 5, 5, 0, 2, 3, 3 },
            { 0, 5, 5, 5, 0, 2, 2, 2 },
            { 0, 0, 0, 0, 0, 4, 4, 4 },
            { 7, 7, 0, 8, 8, 4, 6, 6 },
            { 7, 7, 0, 8, 8, 4, 6, 6 },
            { 7, 7, 0, 0, 0, 4, 6, 6 }
        };
        
        Fill(grid, 2, 2, 9, mode);

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.Write(grid[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}