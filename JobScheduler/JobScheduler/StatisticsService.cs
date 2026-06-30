namespace JobScheduler;

public class StatisticsService
{
    private int _started;
    private int _completed;
    private int _failed;

    public void Handle(object? sender, JobEventArgs e)
    {
        switch (e.EventName)
        {
            case "JobStarted":
                _started++;
                break;
            case "JobCompleted":
                _completed++;
                break;
            case "JobFailed":
                _failed++;
                break;
        }
    }

    public void Print()
    {
        Console.WriteLine("=== Statistics ===");
        Console.WriteLine($"Started:   {_started}");
        Console.WriteLine($"Completed: {_completed}");
        Console.WriteLine($"Failed:    {_failed}");
    }
}