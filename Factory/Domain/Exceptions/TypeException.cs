namespace Domain.Exceptions;

public class TypeException : DomainException
{
    public string Type { get; init; }

    public TypeException(string message, string type) : base(message)
    {
        Type = type;
    }
}