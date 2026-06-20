namespace Domain.Exceptions;

public class StateException : DomainException
{
    public string State { get; init; }

    public StateException(string message, string state) : base(message)
    {
        State = state;
    }
}