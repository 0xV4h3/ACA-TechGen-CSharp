using System.Text.Json;

namespace Core;

public class Result
{
   public int? Value { get; init; }
   public bool Success { get; init; }
   public string? Error { get; init; }

   public override string ToString()
   {
      return JsonSerializer.Serialize(this);
   }
}