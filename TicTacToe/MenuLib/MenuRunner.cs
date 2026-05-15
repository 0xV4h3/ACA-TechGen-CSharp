namespace MenuLib;

public static class MenuRunner
{
    private static readonly MenuStack _navigationStack = new MenuStack();

    public static void Run(Menu root)
    {
        _navigationStack.Push(root);

        while (_navigationStack.Count > 0)
        {
            var currentMenu = _navigationStack.Peek();
            currentMenu.Display();

            Console.WriteLine("\nSelect option: ");
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input)) continue;

            var result = currentMenu.ExecuteOption(input);
            switch (result.Type)
            {
                case NavigationResultType.None:
                    break;
                case NavigationResultType.GoTo:
                    _navigationStack.Push(result.Menu);
                    break;
                case NavigationResultType.Wait:
                    Console.WriteLine("Waiting..., Press any key to continue...");
                    Console.ReadKey();
                    break;
                case NavigationResultType.Back:
                    if (_navigationStack.Count > 1) _navigationStack.Pop();
                    break;
                case NavigationResultType.Exit:
                    return;
            }
        }
    }
}