namespace IEEE754;

public readonly struct FloatBits
{
    private readonly int _sign;
    private readonly int _exponent;
    private readonly uint _mantissa;

    private FloatBits(int sign, int exponent, uint mantissa)
    {
        _sign = sign;
        _exponent = exponent;
        _mantissa = mantissa;
    }

    public static FloatBits FromFloat(float value)
    {
        uint bits = FloatToBits(value);
        int sign = (int)(bits >> 31);
        int exponent = (int)((bits >> 23) & 0xFF);
        uint mantissa = bits & 0x7FFFFF;
        return new FloatBits(sign, exponent, mantissa);
    }

    public float ToFloat()
    {
        uint bits = ((uint)_sign << 31) | ((uint)_exponent << 23) | (_mantissa & 0x7FFFFF);
        return BitsToFloat(bits);
    }

    public string ToBitString(bool pretty = false)
    {
        string sSign = BitHelper.DecimalToBinary(_sign, 1);
        string sExp = BitHelper.DecimalToBinary(_exponent, 8);
        string sMan = BitHelper.DecimalToBinary(_mantissa, 23);
        return pretty ? $"{sSign} | {sExp} | {sMan}" : sSign + sExp + sMan;
    }

    public static FloatBits FromBitString(string bits)
    {
        string cleaned = bits.Replace(" ", "").Replace("|", "");
        if (cleaned.Length != 32)
            throw new ArgumentException("Must be 32 bits.");
        int sign = cleaned[0] - '0';
        int exponent = BitHelper.BinaryToDecimal(cleaned.Substring(1, 8));
        uint mantissa = BitHelper.BinaryToDecimalUnsigned(cleaned.Substring(9, 23));
        return new FloatBits(sign, exponent, mantissa);
    }

    private static uint FloatToBits(float value)
    {
        if (float.IsNaN(value)) return 0x7FC00000u;
        if (float.IsPositiveInfinity(value)) return 0x7F800000u;
        if (float.IsNegativeInfinity(value)) return 0xFF800000u;
        if (value == 0.0f) return float.IsNegative(value) ? 0x80000000u : 0u;

        int sign = float.IsNegative(value) ? 1 : 0;
        double abs = Math.Abs((double)value);

        int biasedExponent;
        double fraction;

        if (abs < 1.1754943508222875E-38)
        {
            biasedExponent = 0;
            fraction = abs * Math.Pow(2, 126);
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

            biasedExponent = exp + 127;
            fraction = scaled - 1.0;
        }

        uint mantissa = 0;
        for (int i = 0; i < 24; i++)
        {
            fraction *= 2.0;
            if (fraction >= 1.0)
            {
                mantissa |= (1u << (23 - i));
                fraction -= 1.0;
            }
        }

        uint finalMantissa = mantissa >> 1;
        bool roundBit = (mantissa & 1) != 0;
        bool stickyBit = fraction > 0.0;
        bool lsb = (finalMantissa & 1) != 0;

        if (roundBit && (stickyBit || lsb))
        {
            finalMantissa++;
            if (finalMantissa >= (1u << 23))
            {
                finalMantissa = 0;
                biasedExponent++;
            }
        }

        if (biasedExponent >= 0xFF)
        {
            biasedExponent = 0xFF;
            finalMantissa = 0;
        }

        return ((uint)sign << 31) | ((uint)biasedExponent << 23) | finalMantissa;
    }

    private static float BitsToFloat(uint bits)
    {
        int sign = (int)(bits >> 31);
        int exponent = (int)((bits >> 23) & 0xFF);
        uint mantissa = bits & 0x7FFFFF;

        if (exponent == 0xFF)
        {
            if (mantissa == 0)
                return sign == 1 ? float.NegativeInfinity : float.PositiveInfinity;
            return float.NaN;
        }

        float result;
        if (exponent == 0)
        {
            if (mantissa == 0)
                return sign == 1 ? -0.0f : 0.0f;

            result = mantissa / (float)(1 << 23);
            result *= MathF.Pow(2, -126);
        }
        else
        {
            float m = 1.0f + mantissa / (float)(1 << 23);
            int e = exponent - 127;
            result = m * MathF.Pow(2, e);
        }

        return sign == 1 ? -result : result;
    }
}