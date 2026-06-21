using Domain.Constants;

namespace Domain.Exceptions;

public class TypeException : DomainException
{
    public TypeConstant Type { get; init; }

    public TypeException(string message, TypeConstant type) : base(message)
    {
        Type = type;
    }
}