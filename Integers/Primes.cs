using System.Numerics;

namespace AstroMultimedia.Numerics.Integers;

public static class Primes
{
    /// <summary>
    /// Static constructor.
    /// </summary>
    static Primes()
    {
        // Initialize the cache.
        Cache = new SortedSet<ulong>(SmallPrimes);
        MaxValueChecked = 1000;
    }

    #region Prime tests

    /// <summary>
    /// Run through some simple checks to see if a value is prime.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    private static bool? IsPrimeSimple(ulong n)
    {
        // 0 and 1 are not prime.
        if (n <= 1)
        {
            return false;
        }

        // Eliminate small primes.
        if (n is 2 or 3 or 5 or 7)
        {
            Cache.Add(n);
            return true;
        }

        // See if we already checked this value.
        if (n <= MaxValueChecked)
        {
            return Cache.Contains(n);
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
        ulong sqrt = (ulong)Floor(Sqrt(n));
        Eratosthenes(sqrt);

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

        // If the number is one of these bases then obviously it's prime, so we don't need to go any
        // further.
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
    /// The primes are processed in batches of up to Array.MaxLength values per batch.
    /// This is because ulong.MaxValue (the largest value of max) is greater than Array.MaxLength
    /// (the largest array size).
    /// </summary>
    /// <param name="max">The max value to check.</param>
    public static void Eratosthenes(ulong max)
    {
        // See if the cache already has all the values we want.
        if (max <= MaxValueChecked)
        {
            return;
        }

        // Get the minimum value to test.
        ulong min = MaxValueChecked + 1;

        // Set the minimum value for the first batch.
        ulong batchMin = min;

        while (true)
        {
            // Get the maximum value for this batch.
            ulong batchMax = Min(batchMin + (ulong)Array.MaxLength - 1, max);
            int batchSize = (int)(batchMax - batchMin + 1);

            // Create an array of booleans indicating if a value is prime.
            // Set the flag for 2 and all odd values greater than 2 to true.
            // Items 0, 1, and all even values greater than 2 will default to false.
            bool[] isPrime = new bool[batchSize];
            for (ulong n = batchMin; n <= batchMax; n++)
            {
                if (n > 1 && (n == 2 || n % 2 == 1))
                {
                    isPrime[n - batchMin] = true;
                }
            }

            // Remove all multiples of primes, 3 or greater, less than or equal to the square root
            // of batchMax.
            ulong sqrt = (ulong)Floor(Sqrt(batchMax));
            for (ulong p = 3; p <= sqrt; p += 2)
            {
                // If p is not prime, skip it.
                if (p >= batchMin && !isPrime[p - batchMin]
                    || p < batchMin && !Cache.Contains(p))
                {
                    continue;
                }

                // Start at the greater of p^2 or batchMin.
                ulong start = Max(p * p, batchMin);

                // Get the next odd multiple of p equal to or greater than start.
                ulong m = (ulong)Ceiling((double)start / p);
                if (m % 2 == 0)
                {
                    m++;
                }

                // Remove all odd multiples of p up to batchMax.
                for (ulong n = m * p; n <= batchMax; n += 2 * p)
                {
                    isPrime[n - batchMin] = false;
                }
            }

            // Update the cache.
            for (ulong n = batchMin; n <= batchMax; n++)
            {
                if (isPrime[n - batchMin])
                {
                    Cache.Add(n);
                }
            }
            MaxValueChecked = batchMax;

            // Check if we're done.
            if (batchMax == max)
            {
                break;
            }

            // Go to the next batch.
            batchMin += (ulong)Array.MaxLength;
        }
    }

    #endregion Prime tests

    #region Get primes methods

    public static List<ulong> GetPrimesUpTo(ulong n)
    {
        // Make sure we've checked all values up to and including n.
        Eratosthenes(n);

        // Return the requested primes.
        return Cache.Where(p => p <= n).ToList();
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
    private static List<ulong> _GetPrimeFactors(ulong n)
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

        // For composite numbers, find the prime factors.
        for (ulong factor = 2; factor <= Sqrt(n); factor++)
        {
            if (!IsPrime(factor) || n % factor != 0)
            {
                continue;
            }

            factors.Add(factor);
            factors.AddRange(GetPrimeFactors(n / factor));
            break;
        }
        return factors;
    }

    public static readonly Func<ulong, List<ulong>> GetPrimeFactors =
        Functions.Memoize<ulong, List<ulong>>(_GetPrimeFactors);

    private static int _NumDistinctPrimeFactors(ulong n) =>
        GetPrimeFactors(n).Distinct().Count();

    public static readonly Func<ulong, int> NumDistinctPrimeFactors =
        Functions.Memoize<ulong, int>(_NumDistinctPrimeFactors);

    #endregion Prime factors methods

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
    /// All primes up to 1000.
    /// </summary>
    public static readonly List<ulong> SmallPrimes = new ()
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
        MaxValueChecked = 0;
        Cache.Clear();
    }

    #endregion Cache stuff
}
