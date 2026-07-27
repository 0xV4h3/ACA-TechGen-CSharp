namespace Domain.Constants;

public class Context(string name)
{
    public string Name { get; } = name;
    
    private readonly Dictionary<ConstantKind, Dictionary<string, Constant>> _categories = new()
    {
        { ConstantKind.Type, new(StringComparer.OrdinalIgnoreCase) },
        { ConstantKind.State, new(StringComparer.OrdinalIgnoreCase) },
        { ConstantKind.Quality, new(StringComparer.OrdinalIgnoreCase) },
        { ConstantKind.Capacity, new(StringComparer.OrdinalIgnoreCase) }
    };

    internal void AddConstant(Constant constant, ConstantKind kind)
    {
        _categories[kind][constant.Value] = constant;
    }

    public Constant? Get(string constantValue, ConstantKind kind) 
        => _categories[kind].GetValueOrDefault(constantValue);

    public bool IsValid(string value, ConstantKind kind)
    {
        if (value == null) return false;
        return _categories[kind].ContainsKey(value);
    }

    public IEnumerable<Constant> GetAll(ConstantKind kind) 
        => _categories[kind].Values;
    
    public IEnumerable<string> GetAllString(ConstantKind kind) 
        => _categories[kind].Keys;
}