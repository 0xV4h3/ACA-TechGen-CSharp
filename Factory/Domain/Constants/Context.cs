namespace Domain.Constants;

public abstract class Context(string name)
{
    public string Name { get; } = name;
    
    private readonly Dictionary<string, Constant> _constants = new(StringComparer.OrdinalIgnoreCase);

    internal void AddConstant(Constant constant)
    {
        _constants[constant.Value] = constant;
    }

    public Constant? Get(string constantValue) => _constants.GetValueOrDefault(constantValue);

    public IEnumerable<Constant> GetAll() => _constants.Values;
    
    public IEnumerable<string> GetAllString() => _constants.Keys;
    
    public bool IsValid(string value)
    {
        if (value == null) return false;
        return _constants.ContainsKey(value);
    }
}

public class TypeContext(string name) : Context(name) { }
public class StateContext(string name) : Context(name) { }
public class QualityContext(string name) : Context(name) { }