namespace GenericSpecifications.Specification;

public static class Specification
{
    public static ISpecification<T> Create<T>(Predicate<T> predicate)
    {
        return new PredicateSpecification<T>(predicate);
    }

    public static ISpecification<T> AllOf<T>(params ISpecification<T>[] specs)
    {
        if (specs == null || specs.Length == 0) throw new ArgumentException("At least one specification is required.", nameof(specs));

        ISpecification<T> current = specs[0];

        for (int i = 1; i < specs.Length; i++)
            current = new AndSpecification<T>(current, specs[i]);

        return current;
    }

    public static ISpecification<T> AnyOf<T>(params ISpecification<T>[] specs)
    {
        if (specs == null || specs.Length == 0) throw new ArgumentException("At least one specification is required.", nameof(specs));

        ISpecification<T> current = specs[0];

        for (int i = 1; i < specs.Length; i++)
            current = new OrSpecification<T>(current, specs[i]);

        return current;
    }
}