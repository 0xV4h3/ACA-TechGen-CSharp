using Domain.Constants;

namespace Domain.Exceptions;

public class ContextException : DomainException
{
    public Context Context { get; init; }

    public ContextException(string message, Context context) : base(message)
    {
        Context = context;
    }
}