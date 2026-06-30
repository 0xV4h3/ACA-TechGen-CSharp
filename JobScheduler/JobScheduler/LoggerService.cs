namespace JobScheduler;

public static class LoggerService
{
    public static void Handle(object? sender, JobEventArgs e)
    {
        if (e.Error != null)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {e.EventName} | Job {e.Job.Id} | {e.Job.Name} | Status={e.Job.Status} | Error={e.Error.Message}");
        }
        else
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {e.EventName} | Job {e.Job.Id} | {e.Job.Name} | Status={e.Job.Status}");
        }
    }
}