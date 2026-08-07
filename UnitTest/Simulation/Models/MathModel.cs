using Engine;

namespace Simulation.Models;

public class MathModel
{
    [Test(2, 1)]
    public int Sum(int a) => a + 1;

    [Test(1, 2)]
    public int Sub(int a) => a - 1;

    [Test(6, 2, 3)]
    public int Mul(int a, int b) => a * b;

    [Test(2.5, 5.0, 2.0)]
    public double Div(double a, double b) => a / b;
}