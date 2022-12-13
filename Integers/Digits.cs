using System.Numerics;
using AstroMultimedia.Core.Numbers;

namespace AstroMultimedia.Numerics.Integers;

public static class Digits
{
    public static int NumDigits(BigInteger n)
    {
        // Avoid logarithm of 0.
        if (n == 0)
        {
            return 1;
        }

        // Avoid logarithm of a negative number.
        n = BigInteger.Abs(n);

        // Get the logarithm, which will be within 1 of the answer.
        double log = BigInteger.Log10(n);

        // Account for fuzziness in the double representation of the logarithm.
        double floor = Floor(log);
        double round = Round(log);
        double nDigits = floor + (round > floor && log.FuzzyEquals(round) ? 2 : 1);

        return (int)nDigits;
    }

    public static List<byte> GetDigits(long n) =>
        Abs(n).ToString().Select(c => (byte)(c - '0')).ToList();

    public static List<byte> GetDistinctDigits(long n) =>
        GetDigits(n).Distinct().ToList();

    public static bool IsPandigital(string digits)
    {
        // Sort the digits.
        char[] sortedDigits = digits.ToCharArray();
        Array.Sort(sortedDigits);
        string strSortedDigits = string.Join("", sortedDigits);

        // Check they look like 1, 2, 3...
        return !strSortedDigits.Where((t, i) => t != i + '1').Any();
    }

    public static bool IsPandigital(ulong digits) =>
        IsPandigital(digits.ToString());

    public static List<ulong> GetRotations(long n)
    {
        List<ulong> result = new ();
        string nString = n.ToString();
        int nDigits = nString.Length;
        for (int i = 1; i < nDigits; i++)
        {
            string newString = nString[i..] + nString[..i];
            result.Add(ulong.Parse(newString));
        }
        return result;
    }
}
