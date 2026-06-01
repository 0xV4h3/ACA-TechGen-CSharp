namespace ValidParentheses;

public static class Parantheses
{
    public static bool IsValid(string s) {
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

    public static void BracketTest()
    {
        Console.WriteLine($"{"()"} → result: {IsValid("()")} | expected: {true}");
        Console.WriteLine($"{"([])"} → result: {IsValid("([])")} | expected: {true}");
        Console.WriteLine($"{"([)]"} → result: {IsValid("([)]")} | expected: {false}");
        Console.WriteLine($"{"{[()()]}"} → result: {IsValid("{[()()]}")} | expected: {true}");
        Console.WriteLine($"{"((("} → result: {IsValid("(((")} | expected: {false}");
    }
}

