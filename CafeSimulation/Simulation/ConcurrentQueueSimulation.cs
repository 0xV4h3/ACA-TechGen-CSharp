using System.Collections.Concurrent;
using Simulation.Models;

namespace Simulation;

public static class ConcurrentQueueSimulation
{
    private static ConcurrentQueue<Order> _orderQueue = new ConcurrentQueue<Order>();
    private static bool _isCashierWorking = true;
    private static int _orderIdCounter = 0;

    public static void Run()
    {
        Console.WriteLine("=== START (ConcurrentQueue) ===");

        Thread cashier = new Thread(CashierWork) { IsBackground = true };
        cashier.Start();

        for (int i = 1; i <= 3; i++)
        {
            int baristaId = i;
            ThreadPool.QueueUserWorkItem(state => BaristaWork(baristaId));
        }

        Thread.Sleep(8000); 
        Console.WriteLine("\n[Cashier] Desk is closing.\n");
        _isCashierWorking = false;

        Thread.Sleep(5000);
    }
    
    private static void CashierWork()
    {
        Random rand = new Random();
        string[] menu = [ "Espresso", "Cappuccino", "Latte" ];

        while (_isCashierWorking)
        {
            Thread.Sleep(rand.Next(600, 1200));

            int id = Interlocked.Increment(ref _orderIdCounter);
            var products = new List<Product> { new Product(menu[rand.Next(menu.Length)], rand.Next(1000, 2000)) };
            
            Order order = new Order(id, products);
            _orderQueue.Enqueue(order);
            Console.WriteLine($"[Cashier] Added Order #{order.Id} to the queue.");
        }
    }

    private static void BaristaWork(int baristaId)
    {
        while (_isCashierWorking || !_orderQueue.IsEmpty)
        {
            if (_orderQueue.TryDequeue(out Order order))
            {
                Console.WriteLine($"[Barista #{baristaId}] Took Order #{order.Id}");
                foreach (var prod in order.Products)
                {
                    Thread.Sleep(prod.PreparationTimeMs);
                    Console.WriteLine($"[Barista #{baristaId}] Prepared {prod.Name} for Order #{order.Id}");
                }
            }
            else
            {
                Thread.Sleep(200);
            }
        }
        Console.WriteLine($"[Barista #{baristaId}] Shift ended.");
    }
}