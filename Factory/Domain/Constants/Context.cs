namespace Domain.Constants;

public abstract class Context(string name)
{
    public string Name { get; } = name;
    private readonly List<Constant> _constants = [];
    
    internal void AddConstant(Constant constant)
    {
        if (!_constants.Any(c => c.Value.Equals(constant.Value, StringComparison.OrdinalIgnoreCase)))
        {
            _constants.Add(constant);
        }
    }

    public IEnumerable<Constant> GetAll() => _constants;
    public IEnumerable<string> GetAllString() => _constants.Select(c => c.Value);
    public bool IsValid(string value) => _constants.Any(c => c.Value.Equals(value, StringComparison.OrdinalIgnoreCase));
}

public class TypeContext(string name) : Context(name) { }
public class StateContext(string name) : Context(name) { }