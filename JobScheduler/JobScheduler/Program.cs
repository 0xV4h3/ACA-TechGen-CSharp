namespace JobScheduler;

class Program
{
    static void Main(string[] args)
    {
        var queue = new JobQueue();

        queue.Enqueue(new Job(1, "Fast Job", Executors.FastExecutor));
        queue.Enqueue(new Job(2, "Safe Job", Executors.SafeExecutor));
        queue.Enqueue(new Job(3, "Retry Job", Executors.RetryExecutor));
        queue.Enqueue(new Job(4, "fail-fast job", Executors.FastExecutor));
        queue.Enqueue(new Job(5, "fail-safe job", Executors.SafeExecutor));
        queue.Enqueue(new Job(6, "retry transient job", Executors.RetryExecutor));

        var scheduler = new Scheduler(queue);
        var stats = new StatisticsService();

        scheduler.JobStateChanged += MonitoringService.Handle;
        scheduler.JobStateChanged += LoggerService.Handle;
        scheduler.JobStateChanged += stats.Handle;

        scheduler.ExecuteAll();
        stats.Print();
    }
}