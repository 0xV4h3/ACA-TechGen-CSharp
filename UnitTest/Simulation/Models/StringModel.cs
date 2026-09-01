using Engine;

namespace Simulation.Models;

public class StringModel
{
    [Test("HELLO", "hello")]
    public string ToUpperText(string value) => value.ToUpperInvariant();

    [Test("hello world", "hello", "world")]
    public string JoinWithSpace(string a, string b) => $"{a} {b}";

    [Test(5, "abcde")]
    public int Length(string value) => value.Length;
}