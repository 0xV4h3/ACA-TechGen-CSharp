namespace CopyTool;

public interface IProgressReporter
{
    void Start();
    void Report(ulong currentProgress, string message);
}