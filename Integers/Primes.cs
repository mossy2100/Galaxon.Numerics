using System.Numerics;
using AstroMultimedia.Core.Collections;

namespace AstroMultimedia.Numerics.Integers;

public static class Primes
{
    /// <summary>
    /// Static constructor.
    /// </summary>
    static Primes()
    {
        // Initialize the primes cache with all primes up to MaxKnown.
        Cache = new SortedSet<ulong>(KnownPrimes);
        MaxValueChecked = MaxKnown;
    }

    #region Prime tests

    /// <summary>
    /// Run through some simple checks to see if a value is prime.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    private static bool? IsPrimeSimple(ulong n)
    {
        // Quick check for small values.
        if (n <= MaxKnown)
        {
            return KnownPrimes.Contains(n);
        }

        // See if it's in the cache.
        if (Cache.Contains(n))
        {
            return true;
        }

        // If the cache doesn't contain this value but we've already checked it, then it mustn't be
        // prime.
        if (n <= MaxValueChecked)
        {
            return false;
        }

        // Eliminate multiples of small primes.
        if (n % 2 == 0 || n % 3 == 0 || n % 5 == 0 || n % 7 == 0)
        {
            return false;
        }

        // Eliminate perfect squares.
        if (double.IsInteger(Sqrt(n)))
        {
            return false;
        }

        // We don't know.
        return null;
    }

    /// <summary>
    /// Check if a given integer is prime.
    /// Slow/naive version that looks for prime divisors.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static bool IsPrimeSlow(ulong n)
    {
        // Run through some simple checks first.
        bool? isPrimeSimple = IsPrimeSimple(n);
        if (isPrimeSimple.HasValue)
        {
            return isPrimeSimple.Value;
        }

        // Update the cache to ensure we know all the primes less than or equal to the square root,
        // which are the divisors we'll check for.
        ulong sqrt = (ulong)Floor(double.Sqrt(n));

        // If n == ulong.MaxValue then sqrt will be uint.MaxValue + 1, which is too large for the
        // Eratosthenes function. So, we'll subtract one, which will give the same result, because
        // uint.MaxValue is odd.
        Eratosthenes(sqrt > uint.MaxValue ? uint.MaxValue : (uint)sqrt);

        // Check if n is prime by checking all known primes up to the square root to see if it's
        // a divisor.
        if (Cache.Any(prime => prime <= sqrt && n % prime == 0))
        {
            return false;
        }

        // The value is prime.
        Cache.Add(n);
        return true;
    }

    /// <summary>
    /// Fast primality test using Miller-Rabin method.
    /// This method is conclusive because the range is limited to ulong.
    /// If the type is changed to BigInteger, then the method name would be IsProbablyPrime().
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static bool IsPrime(ulong n) =>
        IsPrimeSimple(n) ?? MillerRabin(n);

    /// <summary>
    /// Check if a number is composite.
    /// </summary>
    public static bool IsComposite(ulong n) =>
        n > 1 && !IsPrime(n);

    /// <summary>
    /// Use the Miller-Rabin test to see if a number is composite or probably prime.
    /// <see href="https://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test" />
    /// </summary>
    /// <param name="n">The number to test.</param>
    /// <returns>True if the number is probably prime.</returns>
    public static bool MillerRabin(ulong n)
    {
        // According to Wikipedia if we use the following bases no composites less than or equal to
        // 2^64 (ulong.MaxValue) will pass the test.
        // i.e. for ulong values it detects primes correctly, not just probable primes, and won't
        // find any pseudoprimes.
        ulong[] bases = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37 };

        // If the number is one of these bases then it's prime, so we don't need to continue.
        if (bases.Contains(n))
        {
            return true;
        }

        // Get s, d such that n - 1 = 2^s * d and d is odd.
        ulong s = 0;
        ulong d = n - 1;
        while (d % 2 == 0)
        {
            d /= 2;
            s++;
        }

        // Run the test with each base.
        foreach (ulong a in bases)
        {
            ulong x = (ulong)BigInteger.ModPow(a, d, n);
            ulong y = 0;
            for (ulong i = 0; i < s; i++)
            {
                y = (ulong)BigInteger.ModPow(x, 2, n);
                if (y == 1 && x != 1 && x != n - 1)
                {
                    return false;
                }
                x = y;
            }
            if (y != 1)
            {
                return false;
            }
        }

        // n is prime.
        Cache.Add(n);
        return true;
    }

    /// <summary>
    /// Use the Sieve of Eratosthenes to update the cache for all prime numbers less than or equal
    /// to a given maximum.
    /// <see href="https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes" />
    ///
    /// I've limited the value of max to uint.MaxValue (4,294,967,295) as I think this will be
    /// adequate for practical purposes, and it means the isComposite cache will not need to be
    /// larger than Array.MaxSize. Nor will we need to use batches or multiple arrays for
    /// isComposite.
    /// </summary>
    /// <param name="max">The max value to check.</param>
    public static void Eratosthenes(uint max)
    {
        // To support any value of max up to uint.MaxValue, we need at least 57 values in the cache.
        // We can easily add a bunch of small primes.
        if (MaxValueChecked < MaxKnown)
        {
            Cache.UnionWith(KnownPrimes);
            MaxValueChecked = MaxKnown;
        }

        // See if the cache already has all the values we want.
        if (max <= MaxValueChecked)
        {
            return;
        }

        // Get the minimum value to test.
        uint min = (uint)MaxValueChecked + 1;

        // Ensure min is odd.
        if (min % 2 == 0)
        {
            min++;
        }

        // Ensure max is odd.
        if (max % 2 == 0)
        {
            max--;
        }

        // Calculate the array size.
        int arraySize = (int)((max - min) / 2 + 1);

        // Create an array of booleans indicating if a value is composite.
        // This is the reverse of the usual setup, but it saves the time of initializing every value
        // in the array to true. All values in the array will default to false.
        bool[] isComposite = new bool[arraySize];

        // Remove all multiples of primes, 3 or greater, less than or equal to the square root
        // of max.
        uint sqrt = (uint)Floor(Sqrt(max));
        for (uint p = 3; p <= sqrt; p += 2)
        {
            // If p is not prime, skip it.
            if (p >= min && isComposite[(p - min) / 2])
            {
                continue;
            }

            // Start at the greater of p^2 or the next odd multiple of p equal to or greater
            // than min.
            uint start = p * p;
            if (min > start)
            {
                uint m = (uint)Ceiling((double)min / p);
                if (m % 2 == 0)
                {
                    m++;
                }
                start = m * p;
            }

            // Remove all odd multiples of p up to max.
            for (uint i = (start - min) / 2; i < (uint)arraySize; i += p)
            {
                isComposite[i] = true;
            }
        }

        // Update the cache.
        for (int i = 0; i < arraySize; i++)
        {
            if (!isComposite[i])
            {
                Cache.Add(2 * (uint)i + min);
            }
        }

        // We can set the maximum value checked to the even above max, because we know it's
        // composite.
        MaxValueChecked = max + 1;
    }

    #endregion Prime tests

    #region Get primes methods

    /// <summary>
    /// This method only supports up to uint.MaxValue, which is a limitation of Eratosthenes.
    /// </summary>
    /// <param name="max"></param>
    /// <returns></returns>
    public static List<ulong> GetPrimesUpTo(uint max)
    {
        // Ensure all values up to and including max have been checked.
        Eratosthenes(max);

        // Return the requested primes.
        return Cache.Where(p => p <= max).ToList();
    }

    /// <summary>
    /// Get the closest prime number greater than n.
    /// </summary>
    public static ulong GetNext(ulong n)
    {
        // Check for 2, as we'll only test odd values.
        if (n < 2)
        {
            return 2;
        }

        // Initialize p to closest odd number less than or equal to n.
        ulong p = n;
        if (p % 2 == 0)
        {
            p--;
        }

        // Increment and test until we find one.
        while (true)
        {
            // Check for overflow.
            if (p == ulong.MaxValue)
            {
                throw new OverflowException(
                    $"There are no prime numbers > {n} but <= {ulong.MaxValue}, the largest value supported.");
            }

            // Check next value.
            p += 2;
            if (IsPrime(p))
            {
                return p;
            }
        }
    }

    /// <summary>
    /// Get the closest prime number less than n.
    /// </summary>
    public static ulong GetPrevious(ulong n)
    {
        // Guard.
        if (n == 2)
        {
            throw new OverflowException("There are no prime numbers < 2.");
        }

        // Check for 2, as we'll only test odd values.
        if (n == 3)
        {
            return 2;
        }

        // Initialize p to closest odd number greater than or equal to n.
        ulong p = n;
        if (p % 2 == 0)
        {
            p++;
        }

        // Decrement and test odd numbers until we find one.
        while (true)
        {
            p -= 2;
            if (IsPrime(p))
            {
                return p;
            }
        }
    }

    #endregion Get primes methods

    #region Prime factors methods

    /// <summary>
    /// Get all prime factors of n.
    /// </summary>
    private static List<ulong> _PrimeFactors(ulong n)
    {
        // Result array.
        List<ulong> factors = new ();

        // Don't check 0 or 1, which are not prime and have no factors.
        if (n <= 1)
        {
            return factors;
        }

        // If it's prime then include itself.
        if (IsPrime(n))
        {
            factors.Add(n);
            return factors;
        }

        // Check 2 separately, to reduce the number of loop iterations by half.
        if (n % 2 == 0)
        {
            factors.Add(2);
            factors.AddRange(PrimeFactors(n / 2));
            return factors;
        }

        // Check odd primes.
        for (ulong factor = 3; factor <= Sqrt(n); factor += 2)
        {
            if (!IsPrime(factor) || n % factor != 0)
            {
                continue;
            }

            factors.Add(factor);
            factors.AddRange(PrimeFactors(n / factor));
            break;
        }

        return factors;
    }

    public static readonly Func<ulong, List<ulong>> PrimeFactors =
        Functions.Memoize<ulong, List<ulong>>(_PrimeFactors);

    private static List<ulong> _DistinctPrimeFactors(ulong n) =>
        PrimeFactors(n).Distinct().ToList();

    public static readonly Func<ulong, List<ulong>> DistinctPrimeFactors =
        Functions.Memoize<ulong, List<ulong>>(_DistinctPrimeFactors);

    private static int _NumDistinctPrimeFactors(ulong n) =>
        DistinctPrimeFactors(n).Count();

    public static readonly Func<ulong, int> NumDistinctPrimeFactors =
        Functions.Memoize<ulong, int>(_NumDistinctPrimeFactors);

    #endregion Prime factors methods

    #region Coprime methods

    public static bool AreCoprime(ulong n1, ulong n2) =>
        Functions.GreatestCommonDivisor(n1, n2) == 1;

    public static ulong Totient(ulong n)
    {
        List<ulong> factors = DistinctPrimeFactors(n);
        double product = factors
            .Select(factor => (double)factor)
            .Product(factor => 1.0 - 1.0 / factor);
        return (ulong)Round(product * n);
    }

    #endregion Coprime methods

    #region Cache stuff

    /// <summary>
    /// All numbers up to and including this value have been tested and, if prime, added to the
    /// cache.
    /// </summary>
    public static ulong MaxValueChecked;

    /// <summary>
    /// Cache of primes found so far.
    /// </summary>
    public static readonly SortedSet<ulong> Cache;

    /// <summary>
    /// Primes up to MaxKnown are hard-coded in the class, as an optimization.
    /// </summary>
    public const int MaxKnown = 1000;

    /// <summary>
    /// Primes up to MaxKnown.
    /// </summary>
    public static readonly List<ulong> KnownPrimes = new ()
    {
        2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89,
        97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181,
        191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281,
        283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397,
        401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503,
        509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619,
        631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743,
        751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863,
        877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997
    };

    /// <summary>
    /// Clear the primes cache.
    /// </summary>
    public static void ClearCache()
    {
        Cache.Clear();
        MaxValueChecked = 1;
    }

    #endregion Cache stuff
}
