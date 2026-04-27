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
        byte[] bytes = new byte[4];
        Buffer.BlockCopy(new float[] { value }, 0, bytes, 0, 4);
        uint bits = ((uint)bytes[0]) | ((uint)bytes[1] << 8) | ((uint)bytes[2] << 16) | ((uint)bytes[3] << 24);
        int sign = (int)((bits >> 31) & 1);
        int exponent = (int)((bits >> 23) & 0xFF);
        uint mantissa = bits & 0x7FFFFF;
        return new FloatBits(sign, exponent, mantissa);
    }

    public float ToFloat()
    {
        uint bits = ((uint)_sign << 31) | ((uint)_exponent << 23) | (_mantissa & 0x7FFFFF);
        byte[] bytes = new byte[4];
        bytes[0] = (byte)(bits & 0xFF);
        bytes[1] = (byte)((bits >> 8) & 0xFF);
        bytes[2] = (byte)((bits >> 16) & 0xFF);
        bytes[3] = (byte)((bits >> 24) & 0xFF);

        float[] arr = new float[1];
        Buffer.BlockCopy(bytes, 0, arr, 0, 4);
        return arr[0];
    }
    
    public string ToBitString(bool pretty = false)
    {
        string sSign = BitHelper.DecimalToBinary(_sign, 1);
        string sExp = BitHelper.DecimalToBinary(_exponent, 8);
        string sMan = BitHelper.DecimalToBinary(_mantissa, 23);
        return pretty
            ? $"{sSign} | {sExp} | {sMan}"
            : sSign + sExp + sMan;
    }

    public static FloatBits FromBitString(string bits)
    {
        string cleaned = bits.Replace(" ", "").Replace("|", "");
        if (cleaned.Length != 32) throw new ArgumentException("Must be 32 bits.");
        int sign = cleaned[0] - '0';
        int exponent = BitHelper.BinaryToDecimal(cleaned.Substring(1, 8));
        uint mantissa = BitHelper.BinaryToDecimalUnsigned(cleaned.Substring(9, 23));
        return new FloatBits(sign, exponent, mantissa);
    }
}