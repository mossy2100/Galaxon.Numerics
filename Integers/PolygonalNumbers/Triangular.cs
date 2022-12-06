namespace AstroMultimedia.Numerics.Integers;

public static class Triangular
{
    public static readonly Dictionary<long, long> Cache = new();

    /// <summary>
    /// Expand the cache to include all triangular numbers up to a certain max value.
    /// Note: The largest triangular number in the cache may exceed this max after this method runs.
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

        // Get the current index and corresponding triangular number, and the next difference to add.
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
            d = n + 1;
        }

        // Add new triangular numbers until done.
        while (t < max)
        {
            // Get the next triangular number.
            n++;
            t += d;

            // Add it to the cache.
            Cache[n] = t;

            // Go to next.
            d++;
        }
    }

    /// <summary>
    /// Get the nth triangular number.
    /// </summary>
    public static long Get(long n)
    {
        if (!Cache.ContainsKey(n))
        {
            UpdateCache(n * (n + 1) / 2);
        }
        return Cache[n];
    }

    /// <summary>
    /// Get all triangular numbers up to a certain maximum value.
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
    /// Check to see if a number is triangular.
    /// </summary>
    /// <param name="t">The number to check.</param>
    /// <returns>If the argument is a triangular number.</returns>
    public static bool IsTriangular(long t)
    {
        UpdateCache(t);
        return Cache.ContainsValue(t);
    }
}
