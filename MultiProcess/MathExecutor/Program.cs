using System.Text.Json;
using Core;
using System.Diagnostics;

namespace MathExecutor;

class Program
{
    static void Main(string[] args)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = @"D:\Rider\ACA-TechGen-CSharp\MultiProcess\Math\bin\Debug\net10.0\Math.exe", 
            Arguments = $"{String.Join(" ", args)}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using Process process = Process.Start(startInfo);
        
        string stdOutput = process.StandardOutput.ReadToEnd();
        string errors = process.StandardError.ReadToEnd();

        process.WaitForExit();

        Result output;
        try 
        {
            output = JsonSerializer.Deserialize<Result>(stdOutput) 
                     ?? new Result { Success = false, Error = "Process returned empty result." };
        }
        catch (JsonException)
        {
            output = new Result { Success = false, Error = $"Failed to parse process output. Raw output: {stdOutput}" };
        }

        Console.WriteLine($"Result:");
        Console.WriteLine($"Value: {output.Value}");
        Console.WriteLine($"Success: {output.Success}");
        Console.WriteLine($"Error: {output.Error}");
        
        if (!string.IsNullOrEmpty(errors))
        {
            Console.WriteLine("System Errors:");
            Console.WriteLine(errors);
        }

        Console.WriteLine($"Exit Code: {process.ExitCode}");
    }
}