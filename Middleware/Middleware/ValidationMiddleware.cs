namespace Middleware;

public class ValidationMiddleware : IMiddleware<string>
{
    public void Invoke(RequestContext<string> context, RequestDelegate<string> next)
    {
        Console.WriteLine("[Validator] Validating data...");

        if (string.IsNullOrWhiteSpace(context.RequestData))
        {
            Console.WriteLine("[Validator] Error! Data is empty.");
            context.ResponseMessage = "Error: Empty request";
            context.IsAborted = true;
            return;
        }

        Console.WriteLine("[Validator] Validation passed successfully.");
        next(context);
    }
}