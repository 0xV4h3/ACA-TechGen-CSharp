namespace ValidParentheses;

class Program
{
    static void Main(string[] args)
    {
        Parantheses.BracketTest(method:"stack");
        Console.WriteLine();
        Parantheses.BracketTest(method:"replace");
    }
}