using System.Globalization;
using CsvSerializerApp.Csv;
using CsvSerializerApp.Models;

namespace CsvSerializerApp;

class Program
{
    static void Main(string[] args)
    {
        var products = new List<ProductRow>
        {
            new() { Sku = "TEA-1", Name = "Armenian Tea", Price = 4.50m, InStock = true,  WarehouseCode = "WH-A" }
            // new() { Sku = "COF-2", Name = "Coffee, Premium", Price = 9.99m, InStock = false, WarehouseCode = "WH-B" }
        };

        var csv = CsvSerializer.WriteAll(products);
        Console.WriteLine("=== EXPORTED CSV ===");
        Console.WriteLine(csv);

        var rows = CsvSerializer.ReadAll<ProductRow>(csv);
        Console.WriteLine("=== IMPORTED OBJECTS ===");
        foreach (var r in rows)
        {
            Console.WriteLine($"{r.Sku} | {r.Name} | {r.Price.ToString(CultureInfo.InvariantCulture)} | {r.InStock} | Warehouse={r.WarehouseCode ?? "null"}");
        }
    }
}