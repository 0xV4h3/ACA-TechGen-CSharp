namespace JobScheduler;

public static class Executors
{
    private static readonly Dictionary<int, int> RetryFailuresBeforeSuccess = new()
    {
        { 101, 2 },
        { 102, 1 }
    };

    public static void FastExecutor(Job job)
    {
        Thread.Sleep(100);
        if (job.Name.Contains("fail-fast", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"FastExecutor failed for job '{job.Name}'.");

        Thread.Sleep(100);
    }

    public static void SafeExecutor(Job job)
    {
        try
        {
            Thread.Sleep(200);
            if (job.Name.Contains("fail-safe", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"SafeExecutor encountered a failure for job '{job.Name}'.");

            Thread.Sleep(200);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"SafeExecutor wrapped error for job '{job.Name}'.", ex);
        }
    }

    public static void RetryExecutor(Job job)
    {
        int attempts = 3;
        Exception? lastError = null;

        for (int attempt = 1; attempt <= attempts; attempt++)
        {
            try
            {
                if (job.Name.Contains("fail-retry", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException($"RetryExecutor immediate failure on attempt {attempt} for job '{job.Name}'.");

                Thread.Sleep(100);

                int failuresLeft = RetryFailuresBeforeSuccess.TryGetValue(job.Id, out var remaining) ? remaining : 0;
                if (failuresLeft > 0)
                {
                    RetryFailuresBeforeSuccess[job.Id] = failuresLeft - 1;
                    throw new TimeoutException($"Transient failure on attempt {attempt} for job '{job.Name}'.");
                }

                Thread.Sleep(100);
                return;
            }
            catch (Exception ex)
            {
                lastError = ex;
                if (attempt == attempts)
                    break;
            }
        }

        throw new Exception($"RetryExecutor failed after {attempts} attempts for job '{job.Name}'.", lastError);
    }
}