namespace Buffer;

public class TopNBuffer<T>(int capacity, IComparer<T> comparer)
{
    private readonly int _capacity = capacity <= 0 ? throw new ArgumentOutOfRangeException(nameof(capacity)) : capacity;
    private readonly IComparer<T> _comparer = comparer;
    private readonly T[] _items = new T[capacity]; 
    private int _count = 0;

    public void Add(T value)
    {
        if (_count == _capacity)
            if (_comparer.Compare(_items[_count - 1], value) <= 0)
                return;
        
        int i = _count - 1;
        
        if (_count == _capacity)
            i = _capacity - 2;
        
        while (i >= 0 && _comparer.Compare(_items[i], value) > 0)
        {
            _items[i + 1] = _items[i];
            i--;
        }
        
        _items[i + 1] = value;
        
        if (_count < _capacity)
            _count++;
    }

    public IEnumerable<T> Snapshot()
    {
        var result = new T[_count];
        for (int i = 0; i < _count; i++)
        {
            result[i] = _items[i];
        }
        return result;
    }
}