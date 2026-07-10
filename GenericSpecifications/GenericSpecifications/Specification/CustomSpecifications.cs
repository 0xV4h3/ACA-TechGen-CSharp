namespace GenericSpecifications.Specification;

public class PropertyInRangeSpecification<TEntity, TProperty>(Func<TEntity, TProperty> selector, TProperty min, TProperty max)
    : ISpecification<TEntity>
    where TProperty : IComparable<TProperty>
{
    private readonly Func<TEntity, TProperty> _selector = selector;
    private readonly TProperty _min = min;
    private readonly TProperty _max = max;
    
    public bool IsSatisfiedBy(TEntity candidate)
    {
        var value = _selector(candidate);
        return value.CompareTo(_min) >= 0 && value.CompareTo(_max) <= 0;
    }
}

public class PropertyContainsSpecification<TEntity>(Func<TEntity, string> selector, string search)
    : ISpecification<TEntity>
{
    private readonly Func<TEntity, string> _selector = selector;
    private readonly string _search = search;
    
    public bool IsSatisfiedBy(TEntity candidate)
    {
        var value = _selector(candidate);
        if (string.IsNullOrEmpty(value)) return false;
        return value.IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}

public class HasAnySpecification<TEntity, TItem>(Func<TEntity, IEnumerable<TItem>> collectionSelector, ISpecification<TItem> itemSpecification)
    : ISpecification<TEntity>
{
    private readonly Func<TEntity, IEnumerable<TItem>> _collectionSelector = collectionSelector;
    private readonly ISpecification<TItem> _itemSpecification = itemSpecification;
    
    public bool IsSatisfiedBy(TEntity candidate)
    {
        var items = _collectionSelector(candidate);
        if (items == null) return false;

        foreach (var item in items)
        {
            if (_itemSpecification.IsSatisfiedBy(item))
                return true;
        }

        return false;
    }
}

public class HasAllSpecification<TEntity, TItem>(Func<TEntity, IEnumerable<TItem>> collectionSelector, ISpecification<TItem> itemSpecification)
    : ISpecification<TEntity>
{
    private readonly Func<TEntity, IEnumerable<TItem>> _collectionSelector = collectionSelector;
    private readonly ISpecification<TItem> _itemSpecification = itemSpecification;
    
    public bool IsSatisfiedBy(TEntity candidate)
    {
        var items = _collectionSelector(candidate);
        if (items == null) return false;

        bool hasAny = false;

        foreach (var item in items)
        {
            hasAny = true;
            if (!_itemSpecification.IsSatisfiedBy(item))
                return false;
        }

        return hasAny;
    }
}