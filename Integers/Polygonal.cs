namespace AstroMultimedia.Numerics.Integers;

/// <summary>
///     <see href="https://en.wikipedia.org/wiki/Polygonal_number" />
/// </summary>
public static class Polygonal
{
    #region Methods to get a polygonal number

    public static ulong Get(ulong s, ulong n)
    {
        // Guards.
        if (s < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(s),
                "The fewest number of sides a polygon can have is 3.");
        }
        if (n < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(n),
                "Must be at least 1.");
        }

        return (s - 2) * n * (n - 1) / 2 + n;
    }

    public static ulong GetTriangular(ulong n) =>
        Get(3, n);

    public static ulong GetSquare(ulong n) =>
        Get(4, n);

    public static ulong GetPentagonal(ulong n) =>
        Get(5, n);

    public static ulong GetHexagonal(ulong n) =>
        Get(6, n);

    public static ulong GetHeptagonal(ulong n) =>
        Get(7, n);

    public static ulong GetOctagonal(ulong n) =>
        Get(8, n);

    #endregion Methods to get a polygonal number

    #region Methods to get all polygonal numbers up to a certain value

    /// <summary>
    /// Get all polygonal numbers of side s up to a certain maximum value.
    /// </summary>
    public static Dictionary<ulong, ulong> GetAllUpTo(ulong s, ulong max)
    {
        Dictionary<ulong, ulong> result = new ();
        ulong n = 0;
        while (true)
        {
            n++;
            ulong pn = Get(s, n);
            if (pn > max)
            {
                break;
            }
            result[n] = pn;
        }
        return result;
    }

    public static Dictionary<ulong, ulong> GetAllTriangularUpTo(ulong max) =>
        GetAllUpTo(3, max);

    public static Dictionary<ulong, ulong> GetAllSquareUpTo(ulong max) =>
        GetAllUpTo(4, max);

    public static Dictionary<ulong, ulong> GetAllPentagonalUpTo(ulong max) =>
        GetAllUpTo(5, max);

    public static Dictionary<ulong, ulong> GetAllHexagonalUpTo(ulong max) =>
        GetAllUpTo(6, max);

    public static Dictionary<ulong, ulong> GetAllHeptagonalUpTo(ulong max) =>
        GetAllUpTo(7, max);

    public static Dictionary<ulong, ulong> GetAllOctagonalUpTo(ulong max) =>
        GetAllUpTo(8, max);

    #endregion Methods to get all polygonal numbers up to a certain value

    #region Methods to test if a number is polygonal

    /// <summary>
    /// Test to see if a number is a polygonal number of side s.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static bool IsPolygonal(int s, ulong x)
    {
        // Guard.
        if (s < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(s),
                "The fewest number of sides a polygon can have is 3.");
        }

        // Optimizations.
        if (x is 0 or 1)
        {
            // 0 and 1 are polygonal numbers for all side lengths.
            return true;
        }
        if (x == 2)
        {
            // 2 is not a polygonal number for all side lengths.
            return false;
        }

        int s2 = s - 2;
        int s4 = s - 4;
        double n = (Sqrt(8 * (ulong)s2 * x + (ulong)(s4 * s4)) + s4) / (2 * s2);
        return double.IsInteger(n);
    }

    public static bool IsTriangular(ulong x) =>
        IsPolygonal(3, x);

    public static bool IsSquare(ulong x) =>
        IsPolygonal(4, x);

    public static bool IsPentagonal(ulong x) =>
        IsPolygonal(5, x);

    public static bool IsHexagonal(ulong x) =>
        IsPolygonal(6, x);

    public static bool IsHeptagonal(ulong x) =>
        IsPolygonal(7, x);

    public static bool IsOctagonal(ulong x) =>
        IsPolygonal(8, x);

    #endregion Methods to test if a number is polygonal
}
