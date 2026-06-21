using Domain.Constants;

namespace Domain.Exceptions;

public class StateException : DomainException
{
    public StateConstant State { get; init; }

    public StateException(string message, StateConstant state) : base(message)
    {
        State = state;
    }
}