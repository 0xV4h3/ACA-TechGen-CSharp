namespace Domain.Constants;

public enum ConstantKind
{
    Type, 
    State, 
    Quality,
    Capacity
}

public abstract class Constant
{
    public string Value { get; }
    public Context Context { get; }
    public ConstantKind Kind { get; }

    protected Constant(string value, Context context, ConstantKind kind)
    {
        if (string.IsNullOrWhiteSpace(value)) 
            throw new ArgumentException("Value cannot be empty.");
        Value = value;
        Context = context;
        Kind = kind;
        
        Context.AddConstant(this, kind);
    }

    public override string ToString() => Value;
}

public abstract class TypeConstant(string value, Context context) : Constant(value, context, ConstantKind.Type);
public abstract class StateConstant(string value, Context context) : Constant(value, context, ConstantKind.State);
public abstract class QualityConstant(string value, Context context) : Constant(value, context, ConstantKind.Quality);
public abstract class CapacityConstant(string value, Context context) : Constant(value, context, ConstantKind.Capacity);