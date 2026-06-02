namespace ValidParentheses;

public static class Parantheses
{
    public static bool IsValid(string s, string method = "stack")
    {
        if (s.Length == 0 || s.Length % 2 == 1) return false;
        
        switch (method)
        {
            case "stack":
                return IsValidStack(s);
            case "replace":
                return IsValidReplace(s);
            default:
                Console.WriteLine("Unknown method");
                return false;
        }
    }
    
    private static bool IsValidStack(string s) {
        Stack<char> stack = new();

        for(int i = 0; i < s.Length; ++i) {
            if(s[i] == '(' || s[i] == '{' || s[i] == '[')
                stack.Push(s[i]);
            else {
                if(stack.Count == 0) return false;

                char lastOpening = stack.Pop();
                if((lastOpening == '(' && s[i] != ')') ||
                   (lastOpening == '{' && s[i] != '}') ||
                   (lastOpening == '[' && s[i] != ']'))
                    return false;
            }
        }

        return stack.Count == 0;
    }

    private static bool IsValidReplace(string s) {
        while (s.Contains("()") || s.Contains("[]") || s.Contains("{}"))
        {
            s = s.Replace("()", "").Replace("[]", "").Replace("{}", "");
        }
        return s.Length == 0;
    }
    
    public static void BracketTest(string method = "stack")
    {
        Console.WriteLine($"{"()"} → result: {IsValid("()", method)} | expected: {true}");
        Console.WriteLine($"{"([])"} → result: {IsValid("([])", method)} | expected: {true}");
        Console.WriteLine($"{"([)]"} → result: {IsValid("([)]", method)} | expected: {false}");
        Console.WriteLine($"{"{[()()]}"} → result: {IsValid("{[()()]}", method)} | expected: {true}");
        Console.WriteLine($"{"((("} → result: {IsValid("(((", method)} | expected: {false}");
    }
}

