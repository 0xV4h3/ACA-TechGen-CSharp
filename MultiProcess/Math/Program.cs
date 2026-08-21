using Core;

namespace Math;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length < 3)
            {
                Console.WriteLine(new Result { Success = false, Error = "Arguments missing. Usage: Math.exe <operation> <operand1> <operand2>" });
                return;
            }
            
            string operation = args[0];
            
            if (!int.TryParse(args[1], out var operandOne))
            {
                Console.WriteLine(new Result { Success = false, Error = "[operand1] is not a number" });
                return;
            }
            if (!int.TryParse(args[2], out var operandTwo))
            {
                Console.WriteLine(new Result { Success = false, Error = "[operand2] is not a number" });
                return;
            }

            Console.WriteLine(Math.Calculate(operation, operandOne, operandTwo));
        }
        catch (Exception ex)
        {
            Console.WriteLine(new Result { Success = false, Error = $"Critical error: {ex.Message}" });
        }
    }
}