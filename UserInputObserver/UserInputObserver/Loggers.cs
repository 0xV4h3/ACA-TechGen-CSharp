namespace UserInputObserver;

public class ConsoleUserInputLogger : UserInputLogger
{
    protected override void Log(string input)
    {
        Console.WriteLine($"[LOGGER] User Input received: {input}");
    }
}