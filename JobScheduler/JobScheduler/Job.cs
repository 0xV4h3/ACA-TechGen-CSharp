namespace JobScheduler;

public delegate void JobExecutor(Job job);

public enum JobStatus
{
    Pending,
    Running,
    Completed,
    Failed
}

public class Job
{
    public int Id { get; }
    public string Name { get; }
    public JobStatus Status { get; set; }
    public JobExecutor Executor { get; set; }

    public Job(int id, string name, JobExecutor executor)
    {
        Id = id;
        Name = name;
        Executor = executor;
        Status = JobStatus.Pending;
    }
}