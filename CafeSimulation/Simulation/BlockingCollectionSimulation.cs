using System.Collections.Concurrent;
using Simulation.Models;

namespace Simulation;

public static class BlockingCollectionSimulation
{
    private static BlockingCollection<Order> _orderQueue = new BlockingCollection<Order>();
    private static int _orderIdCounter = 0;

    public static void Run()
    {
        Console.WriteLine("=== START (BlockingCollection) ===");

        Thread cashier = new Thread(CashierWork) { IsBackground = true };
        cashier.Start();

        for (int i = 1; i <= 3; i++)
        {
            int baristaId = i;
            ThreadPool.QueueUserWorkItem(state => BaristaWork(baristaId));
        }

        Thread.Sleep(8000);
        Console.WriteLine("\n[Cashier] Desk is closing.\n");
        _orderQueue.CompleteAdding(); 

        Thread.Sleep(5000); 
    }
    
    private static void CashierWork()
    {
        Random rand = new Random();
        string[] menu = [ "Espresso", "Cappuccino", "Latte" ];

        try
        {
            while (!_orderQueue.IsAddingCompleted)
            {
                Thread.Sleep(rand.Next(600, 1200));

                int id = Interlocked.Increment(ref _orderIdCounter);
                var products = new List<Product> { new Product(menu[rand.Next(menu.Length)], rand.Next(1000, 2000)) };

                Order order = new Order(id, products);
                _orderQueue.Add(order);
                Console.WriteLine($"[Cashier] Order #{order.Id} published.");
            }
        }
        catch (InvalidOperationException) { }
    }

    private static void BaristaWork(int baristaId)
    {
        foreach (Order order in _orderQueue.GetConsumingEnumerable())
        {
            Console.WriteLine($"[Barista #{baristaId}] Extracted Order #{order.Id} from BLOCKING queue.");
            foreach (var prod in order.Products)
            {
                Thread.Sleep(prod.PreparationTimeMs);
                Console.WriteLine($"[Barista #{baristaId}] Prepared {prod.Name} for Order #{order.Id}");
            }
        }
        Console.WriteLine($"[Barista #{baristaId}] No more orders. Going home.");
    }
}