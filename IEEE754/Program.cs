namespace IEEE754;

class Program
{
    static void Main(string[] args)
    {
        float x = 12.345f;
        var fb = FloatBits.FromFloat(x);
        Console.WriteLine(fb.ToBitString(true));

        var fromString = FloatBits.FromBitString(fb.ToBitString());
        Console.WriteLine(fromString.ToFloat());

        double y = -15.5;
        var db = DoubleBits.FromDouble(y);
        Console.WriteLine(db.ToBitString(true));

        var db2 = DoubleBits.FromBitString(db.ToBitString());
        Console.WriteLine(db2.ToDouble());
        
    }
}