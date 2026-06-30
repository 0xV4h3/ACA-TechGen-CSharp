namespace RetryExecutor;

public static class RetryExecutor
{
    public static Result<T> Execute<T>(
        Func<T> operation,
        int maxAttempts,
        Func<Exception, bool>? shouldRetry = null)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        if (maxAttempts <= 0) throw new ArgumentOutOfRangeException(nameof(maxAttempts));

        int attempts = 0;
        Exception? lastException = null;
        
        while (attempts < maxAttempts)
        {
            attempts++;
            try
            {
                return new Result<T>
                {
                    Success = true,
                    Value = operation(),
                    Attempts = attempts
                };
            }
            catch (Exception ex)
            {
                lastException = ex;
                if (shouldRetry != null && !shouldRetry(ex))
                    break;
            }
        }
        return new Result<T>
        {
            Success = false,
            Error = lastException,
            Attempts = attempts
        };
    }
}