namespace GenericSpecifications.Specification;

public static class EnumerableSpecificationExtensions
{
    public static IEnumerable<T> Where<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        foreach (var item in source)
        {
            if (specification.IsSatisfiedBy(item))
                yield return item;
        }
    }

    public static bool Any<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        foreach (var item in source)
        {
            if (specification.IsSatisfiedBy(item))
                return true;
        }

        return false;
    }

    public static bool All<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        foreach (var item in source)
        {
            if (!specification.IsSatisfiedBy(item))
                return false;
        }

        return true;
    }

    public static T FirstOrDefault<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        foreach (var item in source)
        {
            if (specification.IsSatisfiedBy(item))
                return item;
        }

        return default;
    }

    public static int Count<T>(this IEnumerable<T> source, ISpecification<T> specification)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        int count = 0;

        foreach (var item in source)
        {
            if (specification.IsSatisfiedBy(item))
                count++;
        }

        return count;
    }
}