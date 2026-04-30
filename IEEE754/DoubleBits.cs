namespace IEEE754;

public readonly struct DoubleBits
{
    private readonly int _sign;
    private readonly int _exponent;
    private readonly ulong _mantissa;

    private DoubleBits(int sign, int exponent, ulong mantissa)
    {
        _sign = sign;
        _exponent = exponent;
        _mantissa = mantissa;
    }

    public static DoubleBits FromDouble(double value)
    {
        ulong bits = DoubleToBits(value);
        int sign = (int)(bits >> 63);
        int exponent = (int)((bits >> 52) & 0x7FF);
        ulong mantissa = bits & 0xFFFFFFFFFFFFFL;
        return new DoubleBits(sign, exponent, mantissa);
    }

    public double ToDouble()
    {
        ulong bits = ((ulong)_sign << 63) | ((ulong)_exponent << 52) | (_mantissa & 0xFFFFFFFFFFFFFL);
        return BitsToDouble(bits);
    }

    public string ToBitString(bool pretty = false)
    {
        string sSign = BitHelper.DecimalToBinary(_sign, 1);
        string sExp = BitHelper.DecimalToBinary(_exponent, 11);
        string sMan = BitHelper.DecimalToBinary(_mantissa, 52);
        return pretty ? $"{sSign} | {sExp} | {sMan}" : sSign + sExp + sMan;
    }

    public static DoubleBits FromBitString(string bits)
    {
        string cleaned = bits.Replace(" ", "").Replace("|", "");
        if (cleaned.Length != 64)
            throw new ArgumentException("Must be 64 bits.");
        int sign = cleaned[0] - '0';
        int exponent = BitHelper.BinaryToDecimal(cleaned.Substring(1, 11));
        ulong mantissa = BitHelper.BinaryToDecimalUnsigned64(cleaned.Substring(12, 52));
        return new DoubleBits(sign, exponent, mantissa);
    }

    public static ulong DoubleToBits(double value)
    {
        if (double.IsNaN(value)) return 0x7FF8000000000000UL;
        if (double.IsPositiveInfinity(value)) return 0x7FF0000000000000UL;
        if (double.IsNegativeInfinity(value)) return 0xFFF0000000000000UL;
        if (value == 0.0) return double.IsNegative(value) ? 0x8000000000000000UL : 0UL;

        int sign = double.IsNegative(value) ? 1 : 0;
        double abs = Math.Abs(value);

        int biasedExponent;
        double fraction;

        if (abs < 2.2250738585072014E-308)
        {
            biasedExponent = 0;
            fraction = abs * Math.Pow(2, 1022);
        }
        else
        {
            int exp = (int)Math.Floor(Math.Log2(abs));
            double scaled = abs * Math.Pow(2, -exp);
            
            if (scaled >= 2.0)
            {
                scaled /= 2.0;
                exp++;
            }
            if (scaled < 1.0)
            {
                scaled *= 2.0;
                exp--;
            }

            biasedExponent = exp + 1023;
            fraction = scaled - 1.0;
        }

        ulong mantissa = 0;
        for (int i = 0; i < 53; i++)
        {
            fraction *= 2.0;
            if (fraction >= 1.0)
            {
                mantissa |= (1UL << (52 - i));
                fraction -= 1.0;
            }
        }

        ulong finalMantissa = mantissa >> 1;
        bool roundBit = (mantissa & 1) != 0;
        bool stickyBit = fraction > 0.0;
        bool lsb = (finalMantissa & 1) != 0;

        if (roundBit && (stickyBit || lsb))
        {
            finalMantissa++;
            if (finalMantissa >= (1UL << 52))
            {
                finalMantissa = 0;
                biasedExponent++;
            }
        }

        if (biasedExponent >= 0x7FF)
        {
            biasedExponent = 0x7FF;
            finalMantissa = 0;
        }

        return ((ulong)sign << 63) | ((ulong)biasedExponent << 52) | finalMantissa;
    }

    public static double BitsToDouble(ulong bits)
    {
        int sign = (int)(bits >> 63);
        int exponent = (int)((bits >> 52) & 0x7FF);
        ulong mantissa = bits & 0xFFFFFFFFFFFFFL;

        if (exponent == 0x7FF)
        {
            if (mantissa == 0)
                return sign == 1 ? double.NegativeInfinity : double.PositiveInfinity;
            return double.NaN;
        }

        double result;
        if (exponent == 0)
        {
            if (mantissa == 0)
                return sign == 1 ? -0.0 : 0.0;
            
            result = mantissa / (double)(1UL << 52);
            result *= Math.Pow(2, -1022);
        }
        else
        {
            double m = 1.0 + mantissa / (double)(1UL << 52);
            int e = exponent - 1023;
            result = m * Math.Pow(2, e);
        }

        return sign == 1 ? -result : result;
    }
}