namespace IInitializable;

public interface IInitializable
{
    void Initialize();
}

public class DemoClass : IInitializable
{
    public bool IsInitialized { get; private set; }
    
    public void Initialize()
    {
        IsInitialized = true;
    }
}