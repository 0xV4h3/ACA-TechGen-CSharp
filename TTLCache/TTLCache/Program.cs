namespace TTLCache;

class Program
{
    static void Main(string[] args)
    {
        FootballPlayer Messi = new("Messi");
        FootballPlayer Vozinha = new("Vozinha");

        Cache<FootballPlayer> cache = new();
        cache.TryAdd(Messi.Name ,Messi, 5);
        cache.TryAdd(Vozinha.Name, Vozinha, 2);
        if(cache.TryGet(Messi.Name, out var lionelOne))
            Console.WriteLine(lionelOne?.Name);
        // Thread.Sleep(3000);
        Thread.Sleep(6000);
        if(cache.TryGet(Messi.Name, out var lionelTwo))
            Console.WriteLine(lionelTwo?.Name);
        else if(Messi.Name == "Messi")
            Console.WriteLine($"In {Messi.Name}'s case, your lifetime has expired");
        if(cache.TryGet(Vozinha.Name, out var mrWorldWide))
            Console.WriteLine(mrWorldWide?.Name);
        else
            Console.WriteLine($"Player {Vozinha.Name} lifetime has expired");
    }
}