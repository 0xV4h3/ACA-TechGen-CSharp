namespace MenuLib;

public struct Option
{
    public string Key { get; }
    public string Value { get; }

    public Option(string key, string value)
    {
        Key = key;
        Value = value;
    }
}

public abstract class Menu
{
    public string Title { get; }

    private Option[] _options;
    private int _optionIndex;

    public Menu(string title)
    {
        Title = title;
    }

    protected void ConfigureOptionSize(int count)
    {
        _options = new Option[count];
        _optionIndex = 0;
    }

    protected void AddOption(string key, string value)
    {
        _options[_optionIndex] = new Option(key, value);
        _optionIndex++;
    }

    private bool ContainsOption(string key)
    {
        foreach (var option in _options)
        {
            if (option.Key == key)
            {
                return true;
            }
        }

        return false;
    }

    public void Display()
    {
        Console.Clear();
        Console.WriteLine($"=== {Title} ===");

        if (_options != null)
        {
            foreach (var option in _options)
            {
                Console.WriteLine($"{option.Key} - {option.Value}");
            }
        }

        InternalDisplay();

        Console.WriteLine("\n--- Navigation ---");
        Console.WriteLine("Type 'back' to go back.");
        Console.WriteLine("Type 'exit' to exit.");
    }

    protected virtual void InternalDisplay()
    {
        
    }

    public NavigationResult ExecuteOption(string option)
    {
        if (option == "back")
        {
            return NavigationResult.Back();
        }

        if (option == "exit")
        {
            return NavigationResult.Exit();
        }

        if (ContainsOption(option))
        {
            Console.Clear();
            return HandleOption(option);
        }

        Console.WriteLine("Invalid option.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);

        return NavigationResult.None();
    }

    protected abstract NavigationResult HandleOption(string option);
}