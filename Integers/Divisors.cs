using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.Numerics.Integers;

public static class Divisors
{
    /// <summary>
    /// Get the list of proper divisors of an integer.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static List<BigInteger> _GetProperDivisors(BigInteger n)
    {
        // Guard.
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");
        }

        List<BigInteger> divisors = new ();

        // Look for divisors up to the square root.
        for (BigInteger i = 1; i <= XBigInteger.Sqrt(n); i++)
        {
            if (n % i != 0)
            {
                continue;
            }

            divisors.Add(i);
            BigInteger j = n / i;
            if (j != i && j < n)
            {
                divisors.Add(j);
            }
        }

        divisors.Sort();
        return divisors;
    }

    public static readonly Func<BigInteger, List<BigInteger>> GetProperDivisors =
        Functions.Memoize<BigInteger, List<BigInteger>>(_GetProperDivisors);

    public static List<BigInteger> GetDivisors(BigInteger n)
    {
        List<BigInteger> divisors = GetProperDivisors(n);
        divisors.Add(n);
        return divisors;
    }

    public static BigInteger SumDivisors(BigInteger n) =>
        GetDivisors(n).Sum();

    /// <summary>
    /// Cache for GreatestCommonDivisor().
    /// </summary>
    private static readonly Dictionary<string, BigInteger> s_gcdCache = new ();

    /// <summary>
    /// Determine the greatest common divisor of two integers.
    /// Synonyms: greatest common factor, highest common factor.
    /// </summary>
    public static BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b)
    {
        // Make a and b non-negative, since the result will be the same for negative values.
        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);

        // Make a < b, to reduce the cache size by half and simplify terminating conditions.
        if (a > b)
        {
            (a, b) = (b, a);
        }

        // Optimization/terminating conditions.
        if (a == b || a == 0)
        {
            return b;
        }
        if (a == 1)
        {
            return 1;
        }

        // Check the cache.
        string key = $"{a}/{b}";
        if (s_gcdCache.ContainsKey(key))
        {
            return s_gcdCache[key];
        }

        // Get the result by recursion.
        BigInteger gcd = GreatestCommonDivisor(a, b % a);

        // Store the result in the cache.
        s_gcdCache[key] = gcd;

        return gcd;
    }

    public static BigInteger LeastCommonMultiple(BigInteger a, BigInteger b)
    {
        // Special case.
        if (a == 0 || b == 0)
        {
            return 0;
        }

        return a * (b / GreatestCommonDivisor(a, b));
    }

    /// <summary>
    /// Check if a number is perfect, deficient, or abundant.
    /// <see href="https://en.wikipedia.org/wiki/Perfect_number" />
    /// </summary>
    public static sbyte PerfectNumber(long n)
    {
        BigInteger spd = Divisors.GetProperDivisors(n).Sum();

        // Check if the number is perfect.
        if (spd == n)
        {
            return 0;
        }

        // Check if the number is deficient or abundant.
        return (sbyte)(spd < n ? -1 : 1);
    }
}
