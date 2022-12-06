namespace AstroMultimedia.Numerics.Integers;

public static class Hexagonal
{
    public static readonly Dictionary<long, long> Cache = new();

    /// <summary>
    /// Expand the cache to include all hexagonal numbers up to a certain max value.
    /// Note: The largest hexagonal number in the cache may exceed max after this method runs.
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

        // Get the current index and corresponding hexagonal number, and the next difference to add.
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
            d = 4 * n + 1;
        }

        // Add new hexagonal numbers until done.
        while (t < max)
        {
            // Get the next hexagonal number.
            n++;
            t += d;

            // Add it to the cache.
            Cache[n] = t;

            // Go to next.
            d += 4;
        }
    }

    /// <summary>
    /// Get the nth hexagonal number.
    /// </summary>
    public static long Get(long n)
    {
        if (!Cache.ContainsKey(n))
        {
            UpdateCache(n * (2 * n - 1));
        }
        return Cache[n];
    }

    /// <summary>
    /// Get all hexagonal numbers up to a certain maximum value.
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
    /// Check to see if a number is hexagonal.
    /// </summary>
    /// <param name="h">The number to check.</param>
    /// <returns>If the argument is a hexagonal number.</returns>
    public static bool IsHexagonal(long h)
    {
        UpdateCache(h);
        return Cache.ContainsValue(h);
    }
}
