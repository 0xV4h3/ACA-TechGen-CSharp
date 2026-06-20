namespace Domain.Registries;

public interface IRegistry
{
    void Register(string value, string context);
    bool IsValid(string value, string context);
    IEnumerable<string> GetAll(string context);
}

