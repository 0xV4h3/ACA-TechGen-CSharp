namespace CollectionOps;

public static class CollectionOps
{
    public static ICollection<T> Filter<T>(ICollection<T> source, Predicate<T> predicate)
    {
        var result = new List<T>();
        foreach (var item in source)
        {
            if(predicate(item))
                result.Add(item);
        }
        
        return result;
    }

    public static ICollection<TOut> Project<TIn, TOut>(ICollection<TIn> source, Func<TIn, TOut> transform)
    {
        var result = new List<TOut>();
        foreach (var item in source)
            result.Add(transform(item));

        return result;
    }
}