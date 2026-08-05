using Engine;

namespace Simulation.Models;

public class Math
{
    [Test(1, 2)]
    public int Sum(int a)
    {
         return a + 1;
    }
    
    [Test(2, 1)]
    public int Sub(int a)
    {
        return a - 1;
    }
}