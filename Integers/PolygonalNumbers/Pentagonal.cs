namespace AstroMultimedia.Numerics.Integers;

public static class Pentagonal
{
    public static readonly Dictionary<long, long> Cache = new();

    /// <summary>
    /// Expand the cache to include all pentagonal numbers up to a certain max value.
    /// Note: The largest pentagonal number in the cache may exceed max after this method runs.
    /// </summary>
    /// <param name="max">The max value.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void UpdateCache(long max)
    {
        // Guard.
        if (max < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(max), "Cannot be negative.");
        }

        // Get the current index and corresponding pentagonal number, and the next difference to add.
        long n = 0;
        long t = 0;
        long d = 1;
        if (Cache.Count > 0)
        {
            // Check if we need to do anything.
            KeyValuePair<long, long> last = Cache.Last();
            if (last.Value >= max)
            {
                return;
            }
            n = last.Key;
            t = last.Value;
            d = 3 * n + 1;
        }

        // Add new pentagonal numbers until done.
        while (t < max)
        {
            // Get the next pentagonal number.
            n++;
            t += d;

            // Add it to the cache.
            Cache[n] = t;

            // Go to next.
            d += 3;
        }
    }

    /// <summary>
    /// Get the nth pentagonal number.
    /// </summary>
    public static long Get(long n)
    {
        if (!Cache.ContainsKey(n))
        {
            UpdateCache(n * (3 * n - 1) / 2);
        }
        return Cache[n];
    }

    /// <summary>
    /// Get all pentagonal numbers up to a certain maximum value.
    /// </summary>
    /// <param name="max"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Dictionary<long, long> UpTo(long max)
    {
        UpdateCache(max);
        return Cache.Where(kvp => kvp.Value <= max)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    /// Check to see if a number is pentagonal.
    /// </summary>
    /// <param name="p">The number to check.</param>
    /// <returns>If the argument is a pentagonal number.</returns>
    public static bool IsPentagonal(long p)
    {
        UpdateCache(p);
        return Cache.ContainsValue(p);
    }
}
