namespace Domain.Ticks;

public interface ITickable
{
    void Tick(int tickNumber);
}

public sealed class TickEngine
{
    private readonly List<ITickable> _tickables = [];

    public TickEngine Register(ITickable tickable)
    {
        _tickables.Add(tickable ?? throw new ArgumentNullException(nameof(tickable)));
        return this;
    }

    public void RunTick(int tickNumber)
    {
        foreach (var tickable in _tickables)
            tickable.Tick(tickNumber);
    }
}
