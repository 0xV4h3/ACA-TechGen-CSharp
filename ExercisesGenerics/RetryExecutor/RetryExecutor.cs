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
        
        while (attempts < maxAttempts)
        {
            attempts++;

            try
            {
                var value = operation();
                return new Result<T>
                {
                    Success = true,
                    Value = value,
                    Attempts = attempts
                };
            }
            catch (Exception ex)
            {
                bool canRetry = attempts < maxAttempts && (shouldRetry?.Invoke(ex) ?? true);

                if (!canRetry)
                {
                    return new Result<T>
                    {
                        Success = false,
                        Error = ex,
                        Attempts = attempts
                    };
                }
            }
        }
        throw new InvalidOperationException("Unexpected exit from retry loop.");
    }
}