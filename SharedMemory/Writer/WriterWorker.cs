using System.Text;
using Domain.Config;

namespace Writer;

public static class WriterWorker
{
    public static void Start(string mode, string filePath)
    {
        if (File.Exists(filePath)) File.Delete(filePath);

        try
        {
            using var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            using var sw = new StreamWriter(fs, Encoding.UTF8);
        }
        finally
        {
            
        }
    }

    private static bool Exit(string input) => input.Equals("exit", StringComparison.OrdinalIgnoreCase);
}