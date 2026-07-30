using AuditDifferTool.Audit;
using AuditDifferTool.Models;

namespace AuditDifferTool;

class Program
{
    static void Main(string[] args)
    {
        var (beforeOne, afterOne) = Test1();
        PrintOrderDiff(beforeOne, afterOne);
        var (beforeTwo, afterTwo) = Test2();
        PrintOrderDiff(beforeTwo, afterTwo);
        var (beforeThree, afterThree) = Test3();
        PrintOrderDiff(beforeThree, afterThree);
    }
    static void PrintOrderDiff(Order before, Order after)
    {
        var changes = AuditDiffer.Diff(before, after, "Order");

        Console.WriteLine("Path | Old | New");
        foreach (var c in changes)
            Console.WriteLine($"{c.Path} | {c.Old} | {c.New}");
    }

    static (Order Before, Order After) Test1()
    {
        var beforeOrder = new Order 
        { 
            Id = Guid.NewGuid(), 
            CustomerName = "Anna", 
            Status = "Pending", 
            Total = new Money { Amount = 40m, Currency = "USD" }, 
            Lines = [ new() { Sku = "TEA-1", Quantity = 2 } ], 
            Tags = [ "vip", "tea" ], 
            RowVersion = [ 1, 2, 3 ] 
        };

        var afterOrder = new Order
        { 
            Id = beforeOrder.Id, 
            CustomerName = "Anna Smith", 
            Status = "Paid", 
            Total = new Money { Amount = 49.90m, Currency = "USD" }, 
            Lines = [ new() { Sku = "TEA-1", Quantity = 5 } ], 
            Tags = [ "vip", "coffee", "sale" ], 
            RowVersion = [ 9, 9, 9 ] 
        };

        return (beforeOrder, afterOrder);
    }

    static (Order before, Order after) Test2()
    {
        var beforeOrder = new Order 
        { 
            Id = Guid.NewGuid(), 
            CustomerName = "Anna", 
            Status = "Paid", 
            Total = new Money { Amount = 40m, Currency = "USD" }, 
            Lines = [ new() { Sku = "MILK-9", Quantity = 1 } ], 
            Tags = [ "eco", "milk" ], 
            RowVersion = [ 4, 5, 6 ] 
        };

        var afterOrder = new Order
        { 
            Id = beforeOrder.Id, 
            CustomerName = "Anna", 
            Status = "Paid", 
            Total = new Money { Amount = 40m, Currency = "USD" }, 
            Lines = [], 
            Tags = [ "eco", "milk" ], 
            RowVersion = [ 7, 7, 7 ] 
        };

        return (beforeOrder, afterOrder);
    }

    static (Order before, Order after) Test3()
    {
        var beforeOrder = new Order 
        { 
            Id = Guid.NewGuid(), 
            CustomerName = "Joan Laporta", 
            Status = "Pending", 
            Total = new Money { Amount = 130_000_000m, Currency = "USD" }, 
            Lines = [ new() { Sku = "Alvarez-9", Quantity = 1 } ], 
            Tags = [ "top", "footballer" ], 
            RowVersion = [ 9, 9, 9 ] 
        };

        var afterOrder = new Order
        { 
            Id = beforeOrder.Id, 
            CustomerName = "Joan Laporta", 
            Status = "Pending", 
            Total = new Money { Amount = 130_000_000m, Currency = "EUR" }, 
            Lines = [ new() { Sku = "Alvarez-9", Quantity = 1 } ],  
            Tags = [ "top", "footballer" ], 
            RowVersion = [ 7, 7, 7 ] 
        };

        return (beforeOrder, afterOrder);
    }
}