using Domain.Constants;

namespace Domain.Models.Abstractions;

public interface IEntity
{
    public TypeConstant Type { get; init; }
    public StateConstant State { get; }
}