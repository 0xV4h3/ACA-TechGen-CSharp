using Simulation.Models;
using Simulation.Utils;
namespace Simulation;

class Program
{
    static void Main(string[] args)
    {
        var users = Data.Users();
        var page2 = users.Where(u => u.Active).GetPage(2, 2).Select(u => u.Name);
        var activePage2 = users.GetPageActive(2, 2).Select(u => u.Name);
        
        foreach (var user in page2)
        {
            Console.WriteLine(user);
        }
        
        foreach (var user in activePage2)
        {
            Console.WriteLine(user);
        }
    }
}