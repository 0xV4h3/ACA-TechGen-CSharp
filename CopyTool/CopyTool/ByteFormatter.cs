namespace CopyTool;

public static class ByteFormatter
{
    private static readonly string[] Suffices = [ "B", "KB", "MB", "GB", "TB", "PB" ];

    public static string Format(double bytes, string format = "F1")
    {
        int counter = 0;
        double value = bytes;
        
        while (Math.Round(value / 1024) >= 1 && counter < Suffices.Length - 1)
        {
            value /= 1024;
            counter++;
        }
        
        if (counter == 0) return $"{value:N0} {Suffices[counter]}";
        
        return $"{value.ToString(format)} {Suffices[counter]}";
    }
}