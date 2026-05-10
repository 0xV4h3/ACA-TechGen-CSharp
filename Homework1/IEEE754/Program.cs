namespace IEEE754;

class Program
{
    static void Main(string[] args)
    {
        float x = 13.625f;
        var fb = FloatBits.FromFloat(x);
        Console.WriteLine(fb.ToFloat());
        Console.WriteLine(fb.ToBitString(true));

        var fromString = FloatBits.FromBitString(fb.ToBitString());
        Console.WriteLine(fromString.ToFloat());

        double y = -15.5;
        var db = DoubleBits.FromDouble(y);
        Console.WriteLine(db.ToDouble());
        Console.WriteLine(db.ToBitString(true));

        var db2 = DoubleBits.FromBitString(db.ToBitString());
        Console.WriteLine(db2.ToDouble());
    }
}