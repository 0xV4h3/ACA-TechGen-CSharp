using Core;

namespace Math;

public static class Math
{
    private delegate int Operation(int x, int y);
    private static Operation Add = (x, y) => x + y;
    private static Operation Sub = (x, y) => x - y;
    private static Operation Mult = (x, y) => x * y;
    private static Operation Div = (x, y) => x / y;
    
    private static Operation Resolve(string operation)
    {
        try
        {
            var op = operation switch
            {
                "add"  => Add,
                "sub"  => Sub,
                "mult" => Mult,
                "div"  => Div,
                _      => throw new Exception("Unknown operation")
            };
            return op;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static Result Calculate(string op, int operandOne, int operandTwo)
    {
        int result = 0;
        try
        {
            var operation = Resolve(op);
            result = operation(operandOne, operandTwo);
        }
        catch (Exception e)
        {
            return new Result {Success = false, Error = e.Message};
        }

        return new Result {Value = result, Success = true};
    }
}