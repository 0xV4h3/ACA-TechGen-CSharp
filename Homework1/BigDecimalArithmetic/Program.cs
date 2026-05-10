namespace BigDecimalArithmetic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(BigInt.Add("9999", "1"));            // 10000
        Console.WriteLine(BigInt.Add("-50", "123"));           // 73
        Console.WriteLine(BigInt.Add("-123", "-9999"));        // -10122
        Console.WriteLine(BigInt.Subtract("10000", "1"));      // 9999
        Console.WriteLine(BigInt.Subtract("123", "456"));      // -333
        Console.WriteLine(BigInt.Subtract("-123", "-456"));    // 333
        Console.WriteLine(BigInt.Multiply("13", "-13"));       // -169
        Console.WriteLine(BigInt.Multiply("-11", "-11"));      // 121
        Console.WriteLine(BigInt.Multiply("-9999", "0"));      // 0
    }
}