namespace CopyTool;

public interface IProgressReporter
{
    void Report(ulong currentProgress, string message);
}