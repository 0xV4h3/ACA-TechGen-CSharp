namespace IInitializable;

class Program
{
    static void Main(string[] args)
    {
        var instance = InitializeInstance<DemoClass>();
        Console.WriteLine(instance.IsInitialized);
    }
    
    public static T InitializeInstance<T>() where T : IInitializable, new()
    {
        var instance = new T();
        instance.Initialize();
        return instance;
    }
}