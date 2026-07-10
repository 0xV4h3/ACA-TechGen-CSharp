namespace GenericSpecifications.Specification;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T candidate);
}

public class PredicateSpecification<T>(Predicate<T> predicate) : ISpecification<T>
{
    private readonly Predicate<T> _predicate = predicate;

    public bool IsSatisfiedBy(T candidate) => _predicate(candidate);
}

public class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right) : ISpecification<T>
{
    private readonly ISpecification<T> _left = left;
    private readonly ISpecification<T> _right = right;
    
    public bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) && _right.IsSatisfiedBy(candidate);
}

public class OrSpecification<T>(ISpecification<T> left, ISpecification<T> right) : ISpecification<T>
{
    private readonly ISpecification<T> _left = left;
    private readonly ISpecification<T> _right = right;

    public bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate);
}

public class NotSpecification<T>(ISpecification<T> inner) : ISpecification<T>
{
    private readonly ISpecification<T> _inner = inner;
    
    public bool IsSatisfiedBy(T candidate) => !_inner.IsSatisfiedBy(candidate);
}