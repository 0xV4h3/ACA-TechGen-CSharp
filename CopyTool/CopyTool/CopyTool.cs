namespace CopyTool;

public static class CopyTool
{
    public static void Copy(string sourcePath, string destinationPath, int bufferSize = 65536, IProgressReporter? progressBar = null)
    {
        using var fileReader = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var fileWriter = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);

        ulong totalBytes = (ulong)fileReader.Length;
        
        progressBar?.Start();

        byte[] buffer = new byte[bufferSize];
        int bytesRead;
        
        string totalSizeStr = ByteFormatter.Format(totalBytes);

        while ((bytesRead = fileReader.Read(buffer, 0, buffer.Length)) > 0)
        {
            fileWriter.Write(buffer, 0, bytesRead);

            ulong currentPosition = (ulong)fileReader.Position;
            string currentSizeStr = ByteFormatter.Format(currentPosition);
            progressBar?.Report(currentPosition, $"{currentSizeStr} / {totalSizeStr}");
        }
    }
}