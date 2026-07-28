namespace UserInputObserver;

public interface IUserInputObserver
{
    public Result<string> Handle(string input);
}

public abstract class UserInputValidator : IUserInputObserver
{
    public Result<string> Handle(string input) => Validate(input);
    protected abstract Result<string> Validate(string input);
}

public abstract class UserInputLogger : IUserInputObserver
{
    public Result<string> Handle(string input)
    {
        try
        {
            Log(input);
            return Result<string>.Ok(input);
        }
        catch (Exception ex)
        {
            return Result<string>.Fail($"[Logger Error] Failed to log input: {ex.Message}");
        }
    }

    protected abstract void Log(string input);
}