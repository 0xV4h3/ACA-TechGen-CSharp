using GenericSpecifications.Specification;
using GenericSpecifications.Models;

namespace GenericSpecifications.Demo;

public class InStockSpecification : ISpecification<Product>
{
    public bool IsSatisfiedBy(Product candidate) => candidate.Stock > 0;
}

public class OutOfStockSpecification : ISpecification<Product>
{
    public bool IsSatisfiedBy(Product candidate) => candidate.Stock <= 0;
}

public class CategorySpecification(string category) : ISpecification<Product>
{
    private readonly string _category = category;
    
    public bool IsSatisfiedBy(Product candidate) => string.Equals(candidate.Category, _category, StringComparison.OrdinalIgnoreCase);
}

public class MinPriceSpecification(decimal minPrice) : ISpecification<Product>
{
    private readonly decimal _minPrice = minPrice;
    
    public bool IsSatisfiedBy(Product candidate) => candidate.Price >= _minPrice;
}

public class MaxPriceSpecification(decimal maxPrice) : ISpecification<Product>
{
    private readonly decimal _maxPrice = maxPrice;
    
    public bool IsSatisfiedBy(Product candidate) => candidate.Price <= _maxPrice;
}