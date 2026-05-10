namespace MyList;

class Program
{
    static void Main(string[] args)
    {
        MyList list = new MyList();
        Console.WriteLine(list.Count);
        Console.WriteLine(list.Capacity);
        list.Add(1);
        list.Add(2);
        Console.WriteLine(list.Count);
        list.Add(3);
        list.Add(4);
        list.Add(5);
        Console.WriteLine(list.Capacity);
        Console.WriteLine(list.IndexOf(5));
        list.Remove(5);
        Console.WriteLine(list.Count);
        Console.WriteLine(list.IndexOf(5));
        if (list.TryGet(2, out int item)) ;
        Console.WriteLine(item);
        int[] array = [5, 6,  7, 8, 9];
        list.AddRange(array);
        Console.WriteLine(list.Count);
        Console.WriteLine(list.Capacity);
        list.Clear();
        Console.WriteLine(list.Count);
    }
}