using AuditDifferTool.Audit;
using AuditDifferTool.Models;

namespace AuditDifferTool;

class Program
{
    static void Main(string[] args)
    {
        Test1();
        Test2();
        Test3();
        Test4();
    }
    
    static void PrintDiff<T>(T before, T after)
    {
        var changes = AuditDiffer.Diff(before, after, typeof(T).Name);

        Console.WriteLine("Path | Old | New");
        foreach (var c in changes)
            Console.WriteLine($"{c.Path} | {c.Old} | {c.New}");
    }

    static void Test1()
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

        PrintDiff(beforeOrder, afterOrder);
    }

    static void Test2()
    {
        var beforeOrder = new Order 
        { 
            Id = Guid.NewGuid(), 
            CustomerName = "Anna", 
            Status = "Paid", 
            Total = new Money { Amount = 40m, Currency = "USD" }, 
            Lines = [ new() { Sku = "TEA-1", Quantity = 5 }, new OrderLine() { Sku = "MILK-9", Quantity = 1 } ], 
            Tags = [ "eco", "milk" ], 
            RowVersion = [ 4, 5, 6 ] 
        };

        var afterOrder = new Order
        { 
            Id = beforeOrder.Id, 
            CustomerName = "Anna", 
            Status = "Paid", 
            Total = new Money { Amount = 40m, Currency = "USD" }, 
            Lines = [ new() { Sku = "TEA-1", Quantity = 5 }, new OrderLine() ],
            Tags = [ "eco", "milk" ], 
            RowVersion = [ 7, 7, 7 ] 
        };

        PrintDiff(beforeOrder, afterOrder);
    }

    static void Test3()
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

        PrintDiff(beforeOrder, afterOrder);
    }

    static void Test4()
    {
        var n1 = new Node { Name = "draft" };
        n1.Next = n1;
        var n2 = new Node { Name = "final" };
        n2.Next = n2;

        PrintDiff(n1, n2);
    }
}