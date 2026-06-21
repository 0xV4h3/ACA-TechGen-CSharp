using Domain.Constants;

namespace Domain.Registries;

public interface IRegistry
{
    bool Register(Constant constant, bool autoRegisterContext = false);
    bool RegisterContext(Context context);
    bool IsValid(string value, Context context);
    IEnumerable<Constant> GetAll(Context context);
    IEnumerable<string> GetAllString(Context context);
}