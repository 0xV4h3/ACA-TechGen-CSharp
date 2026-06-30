namespace JobScheduler;

public class Scheduler
{
    private readonly JobQueue _queue;

    public event EventHandler<JobEventArgs>? JobStateChanged;

    public Scheduler(JobQueue queue)
    {
        _queue = queue;
    }

    public void ExecuteAll()
    {
        foreach (Job job in _queue)
        {
            try
            {
                job.Status = JobStatus.Running;
                OnJobStateChanged(new JobEventArgs(job, "JobStarted"));

                job.Executor(job);

                job.Status = JobStatus.Completed;
                OnJobStateChanged(new JobEventArgs(job, "JobCompleted"));
            }
            catch (Exception ex)
            {
                job.Status = JobStatus.Failed;
                OnJobStateChanged(new JobEventArgs(job, "JobFailed", ex));
            }
            finally
            {
                Console.WriteLine($"Scheduler final: Job {job.Id} [{job.Name}] => {job.Status}");
            }
        }
    }

    private void OnJobStateChanged(JobEventArgs args)
    {
        JobStateChanged?.Invoke(this, args);
    }
}