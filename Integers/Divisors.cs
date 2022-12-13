using System.Numerics;
using AstroMultimedia.Core.Numbers;

namespace AstroMultimedia.Numerics.Integers;

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

    private static BigInteger _GreatestCommonDivisor(BigInteger a, BigInteger b)
    {
        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);

        // Optimizations.
        if (a == 0 && b == 0)
        {
            return 0;
        }

        if (a == 0)
        {
            return b;
        }

        if (b == 0 || a == b)
        {
            return a;
        }

        if (a == 1 || b == 1)
        {
            return 1;
        }

        // // Make a < b.
        // if (a > b)
        // {
        //     (a, b) = (b, a);
        // }

        // See if the smaller number evenly divides the larger.
        if (b > a && b % a == 0)
        {
            return a;
        }
        if (a > b && a % b == 0)
        {
            return b;
        }

        // If the larger number is prime then there won't be any common divisors greater than 1.
        // if (b <= ulong.MaxValue && Primes.IsPrime((ulong)b))
        // {
        //     return 1;
        // }

        return GreatestCommonDivisor(a, b % a);

        // Test potential divisors from b/2 down to 2.
        // for (BigInteger d = b / 2; d >= 2; d--)
        // {
        //     if (a % d == 0 && b % d == 0)
        //     {
        //         return d;
        //     }
        // }

        // None found.
        // return 1;
    }

    public static readonly Func<BigInteger, BigInteger, BigInteger> GreatestCommonDivisor =
        Functions.Memoize2<BigInteger, BigInteger, BigInteger>(_GreatestCommonDivisor);

    public static BigInteger LeastCommonMultiple(BigInteger a, BigInteger b)
    {
        // Special case.
        if (a == 0 || b == 0)
        {
            return 0;
        }

        return BigInteger.Abs(a) * (BigInteger.Abs(b) / GreatestCommonDivisor(a, b));
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
