namespace AreaFill;

class Program
{
    static void Main(string[] args)
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
        
        AreaFill.Fill(grid, 2, 2, 9);

        foreach (int[] row in grid)
        {
            Console.WriteLine(string.Join(" ", row));
        }

    }
}