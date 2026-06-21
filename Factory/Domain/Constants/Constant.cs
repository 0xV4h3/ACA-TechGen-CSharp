namespace Domain.Constants;

public abstract class Constant
{
    public string Value { get; }
    public Context Context { get; }

    protected Constant(string value, Context context)
    {
        if (string.IsNullOrWhiteSpace(value)) 
            throw new ArgumentException("Value cannot be empty.");
        Value = value;
        Context = context;
        
        Context.AddConstant(this);
    }

    public override string ToString() => Value;
}

public abstract class TypeConstant(string value, TypeContext context) : Constant(value, context) { }
public abstract class StateConstant(string value, StateContext context) : Constant(value, context) { }