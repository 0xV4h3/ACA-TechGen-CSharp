namespace CopyTool;

public interface IProgressReporter : IDisposable
{
    void Report(ulong currentProgress, string message);
}