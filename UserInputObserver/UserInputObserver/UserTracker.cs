namespace UserInputObserver;

public class UserTracker
{
    private readonly List<IUserInputObserver> _observers = [];

    public void Subscribe(IUserInputObserver observer) => _observers.Add(observer);
    public void Unsubscribe(IUserInputObserver observer) => _observers.Remove(observer);

    public List<Result<string>> NotifyUserInput(string input)
    {
        var results = new List<Result<string>>();

        foreach (var observer in _observers)
            results.Add(observer.Handle(input));

        return results;
    }
}