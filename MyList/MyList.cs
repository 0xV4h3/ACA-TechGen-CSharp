namespace MyList;

public class MyList
{
    private int[] _items = new int[4];
    public int Capacity => _items.Length;
    public int Count { get; private set; }

    public int this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _items[index];
        }
        set
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            _items[index] = value;
        }
    }

    public void Add(int item)
    {
        if (Count == _items.Length)
            Resize(_items.Length * 2);
        _items[Count++] = item;
    }

    public void AddRange(int[]? items)
    {
        if (items == null) return;
        if (Count + items.Length > _items.Length)
        {
            int newCapacity = _items.Length;
            while (newCapacity < Count + items.Length)
                newCapacity *= 2;
            Resize(newCapacity);
        }
        foreach (var x in items)
            _items[Count++] = x;
    }

    public bool Remove(int item)
    {
        int index = IndexOf(item);
        if (index == -1) return false;

        for (int i = index; i < Count - 1; i++)
            _items[i] = _items[i + 1];
        
        _items[--Count] = 0;
        return true;
    }

    public bool TryGet(int index, out int item)
    {
        item = -1;
        if (index < 0 || index >= Count) return false;
        item = _items[index];
        return true;
    }

    public int IndexOf(int item)
    {
        for (int i = 0; i < Count; i++)
            if (_items[i] == item) return i;
        return -1;
    }

    public bool Contains(int item) => IndexOf(item) != -1;

    public void Clear()
    {
        for (int i = 0; i < Count; i++) 
            _items[i] = 0;
        Count = 0;
    }
    
    private void Resize(int newCapacity)
    {
        int[] newArr = new int[newCapacity];
        for (int i = 0; i < Count; i++)
            newArr[i] = _items[i];
        _items = newArr;
    }
}