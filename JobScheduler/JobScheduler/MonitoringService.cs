namespace JobScheduler;

public static class MonitoringService
{
    public static void Handle(object? sender, JobEventArgs e)
    {
        Console.WriteLine($"[MONITOR] {e.EventName} | Job {e.Job.Id} ({e.Job.Name}) | Status={e.Job.Status}");
    }
}