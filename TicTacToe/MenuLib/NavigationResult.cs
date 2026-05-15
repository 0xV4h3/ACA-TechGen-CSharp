namespace MenuLib;

public class NavigationResult
{
    public NavigationResultType Type { get; }
    public Menu? Menu { get; }

    private NavigationResult(NavigationResultType type, Menu? menu = null)
    {
        Type = type;
        Menu = menu;
    }

    public static NavigationResult None()
    {
        return new NavigationResult(NavigationResultType.None);
    }


    public static NavigationResult GoTo(Menu next)
    {
        return new NavigationResult(NavigationResultType.GoTo, next);
    }

    public static NavigationResult Wait()
    {
        return new NavigationResult(NavigationResultType.Wait);
    }

    public static NavigationResult Back()
    {
        return new NavigationResult(NavigationResultType.Back);
    }

    public static NavigationResult Exit()
    {
        return new NavigationResult(NavigationResultType.Exit);
    }
}