namespace Galaxon.Numerics.Integers;

public static class Functions
{
    /// <summary>
    /// Enables caching of the results of pure functions.
    /// </summary>
    /// <param name="f">The pure function.</param>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <returns>The memoized version of the pure function.</returns>
    public static Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> f) where T : notnull
    {
        Dictionary<T, TResult> cache = new ();
        return x =>
        {
            if (cache.TryGetValue(x, out TResult? result))
            {
                return result;
            }
            result = f(x);
            cache.Add(x, result);
            return result;
        };
    }

    /// <summary>
    /// Enables caching of the results of pure functions with 2 inputs and 1 output.
    /// </summary>
    /// <param name="f">The pure function.</param>
    /// <typeparam name="T1">First argument type.</typeparam>
    /// <typeparam name="T2">Second argument type.</typeparam>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <returns>The memoized version of the pure function.</returns>
    public static Func<T1, T2, TResult> Memoize2<T1, T2, TResult>(Func<T1, T2, TResult> f)
    {
        Dictionary<(T1, T2), TResult> cache = new ();
        return (x, y) =>
        {
            if (cache.TryGetValue((x, y), out TResult? result))
            {
                return result;
            }
            result = f(x, y);
            cache.Add((x, y), result);
            return result;
        };
    }
}
