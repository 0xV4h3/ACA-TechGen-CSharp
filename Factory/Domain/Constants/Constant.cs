namespace Domain.Constants;

public abstract class Constant
{
    public string Value { get; }
    public string Context { get; }

    protected Constant(string value, string context)
    {
        if (string.IsNullOrWhiteSpace(value)) 
            throw new ArgumentException("Value cannot be empty.");
        Value = value;
        Context = context;
    }

    public override string ToString() => Value;
}

public abstract class StateConstant : Constant 
{
    protected StateConstant(string value, string context) : base(value, context) { }
}

public abstract class TypeConstant : Constant 
{
    protected TypeConstant(string value, string context) : base(value, context) { }
}