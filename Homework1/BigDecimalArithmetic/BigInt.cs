using System.Text;
namespace BigDecimalArithmetic;

public static class BigInt
{
    private static (int sign, string num) Parse(string s)
    {
        if (string.IsNullOrEmpty(s)) return (1, "0");
        int sign = 1, i = 0;
        if (s[0] == '-')
        {
            sign = -1;
            i = 1;
        }
        else if (s[0] == '+')
            i = 1;

        int start = i;
        while (i < s.Length && s[i] == '0') i++;
        if (i == s.Length) return (1, "0");
        for (int j = start; j < s.Length; j++)
            if (s[j] < '0' || s[j] > '9')
                return (1, "0");
        return (sign, s.Substring(i));
    }

    public static string Add(string a, string b)
    {
        var (signA, aa) = Parse(a);
        var (signB, bb) = Parse(b);

        if (signA == signB)
        {
            string res = PureAdd(aa, bb);
            if (signA == -1 && res != "0") return "-" + res;
            return res;
        }

        int cmp = CompareAbs(aa, bb);
        if (cmp == 0) return "0";
        if (cmp > 0)
        {
            string res = PureSubtract(aa, bb);
            return signA == 1 ? res : "-" + res;
        }
        else
        {
            string res = PureSubtract(bb, aa);
            return signB == 1 ? res : "-" + res;
        }
    }

    public static string Subtract(string a, string b)
    {
        if (string.IsNullOrEmpty(b)) return Add(a, "0");
        var sign = b[0] == '-' ? '+' : '-';
        string arg = b[0] == '-' || b[0] == '+' ? $"{sign}{b.Substring(1)}" : $"{sign}{b}";
        return Add(a, arg);
    }

    public static string Multiply(string a, string b)
    {
        var (signA, aa) = Parse(a);
        var (signB, bb) = Parse(b);
        string res = PureMultiply(aa, bb);
        int sign = (aa == "0" || bb == "0") ? 1 : signA * signB;
        return (sign == -1 && res != "0") ? "-" + res : res;
    }

    private static int CompareAbs(string a, string b)
    {
        if (a.Length != b.Length)
            return a.Length > b.Length ? 1 : -1;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
                return a[i] > b[i] ? 1 : -1;
        }
        return 0;
    }

    private static string PureAdd(string a, string b)
    {
        int n = Math.Max(a.Length, b.Length);
        var sb = new StringBuilder();
        int carry = 0;
        for (int i = 0; i < n; i++)
        {
            int ai = (i < a.Length) ? a[a.Length - 1 - i] - '0' : 0;
            int bi = (i < b.Length) ? b[b.Length - 1 - i] - '0' : 0;
            int sum = ai + bi + carry;
            sb.Insert(0, (char)('0' + (sum % 10)));
            carry = sum / 10;
        }
        if (carry > 0)
            sb.Insert(0, (char)('0' + carry));
        return sb.ToString();
    }

    private static string PureSubtract(string a, string b)
    {
        int n = a.Length;
        var sb = new StringBuilder();
        int borrow = 0;
        for (int i = 0; i < n; i++)
        {
            int ai = a[a.Length - 1 - i] - '0';
            int bi = (i < b.Length) ? b[b.Length - 1 - i] - '0' : 0;
            int diff = ai - bi - borrow;
            if (diff < 0)
            {
                diff += 10;
                borrow = 1;
            }
            else
            {
                borrow = 0;
            }
            sb.Insert(0, (char)('0' + diff));
        }
        int p = 0;
        while (p < sb.Length - 1 && sb[p] == '0') p++;
        return sb.ToString(p, sb.Length - p);
    }

    private static string PureMultiply(string a, string b)
    {
        if (a == "0" || b == "0") return "0";
        int[] res = new int[a.Length + b.Length];
        for (int i = a.Length - 1; i >= 0; i--)
        {
            int ai = a[i] - '0';
            for (int j = b.Length - 1; j >= 0; j--)
            {
                int bi = b[j] - '0';
                int pos = i + j + 1;
                res[pos] += ai * bi;
            }
        }

        for (int i = res.Length - 1; i > 0; i--)
        {
            if (res[i] >= 10)
            {
                res[i - 1] += res[i] / 10;
                res[i] %= 10;
            }
        }
        int k = 0;
        while (k < res.Length - 1 && res[k] == 0) k++;
        var sb = new StringBuilder();
        for (; k < res.Length; k++)
            sb.Append((char)('0' + res[k]));
        return sb.ToString();
    }
}