using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.Numerics.Integers;

public static class Divisors
{
    public static readonly Func<BigInteger, List<BigInteger>> GetProperDivisors =
        Functions.Memoize<BigInteger, List<BigInteger>>(_GetProperDivisors);

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

        // Get the square root of the argument, then cast to BigInteger to truncate the result.
        BigDecimal.MaxSigFigs = Min(n.NumDigits() + 1, 30);
        var sqrt = (BigInteger)BigDecimal.Sqrt(n);

        // Look for divisors up to the square root.
        for (BigInteger i = 1; i <= sqrt; i++)
        {
            if (n % i != 0)
            {
                continue;
            }

            divisors.Add(i);
            var j = n / i;
            if (j != i && j < n)
            {
                divisors.Add(j);
            }
        }

        divisors.Sort();
        return divisors;
    }

    public static List<BigInteger> GetDivisors(BigInteger n)
    {
        var divisors = GetProperDivisors(n);
        divisors.Add(n);
        return divisors;
    }

    public static BigInteger SumDivisors(BigInteger n) => GetDivisors(n).Sum();

    /// <summary>
    /// Check if a number is perfect, deficient, or abundant.
    /// <see href="https://en.wikipedia.org/wiki/Perfect_number" />
    /// </summary>
    public static sbyte PerfectNumber(long n)
    {
        var spd = GetProperDivisors(n).Sum();

        // Check if the number is perfect.
        if (spd == n)
        {
            return 0;
        }

        // Check if the number is deficient or abundant.
        return (sbyte)(spd < n ? -1 : 1);
    }
}
