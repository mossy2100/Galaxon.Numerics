namespace AstroMultimedia.Numerics.Integers;

public class PolygonalNumbers
{
    #region Methods to get a polygonal number

    public static uint Get(uint s, uint n)
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

    public static uint GetTriangular(uint n) =>
        Get(3, n);

    public static uint GetSquare(uint n) =>
        Get(4, n);

    public static uint GetPentagonal(uint n) =>
        Get(5, n);

    public static uint GetHexagonal(uint n) =>
        Get(6, n);

    public static uint GetHeptagonal(uint n) =>
        Get(7, n);

    public static uint GetOctagonal(uint n) =>
        Get(8, n);

    #endregion Methods to get a polygonal number

    #region Methods to get all polygonal numbers up to a certain value

    /// <summary>
    /// Get all polygonal numbers of side s up to a certain maximum value.
    /// </summary>
    public static Dictionary<uint, uint> GetAllUpTo(uint s, uint max)
    {
        Dictionary<uint, uint> result = new ();
        uint n = 0;
        while (n <= max)
        {
            n++;
            result[n] = Get(s, n);
        }
        return result;
    }

    public static Dictionary<uint, uint> GetAllTriangularUpTo(uint max) =>
        GetAllUpTo(3, max);

    public static Dictionary<uint, uint> GetAllSquareUpTo(uint max) =>
        GetAllUpTo(4, max);

    public static Dictionary<uint, uint> GetAllPentagonalUpTo(uint max) =>
        GetAllUpTo(5, max);

    public static Dictionary<uint, uint> GetAllHexagonalUpTo(uint max) =>
        GetAllUpTo(6, max);

    public static Dictionary<uint, uint> GetAllHeptagonalUpTo(uint max) =>
        GetAllUpTo(7, max);

    public static Dictionary<uint, uint> GetAllOctagonalUpTo(uint max) =>
        GetAllUpTo(8, max);

    #endregion Methods to get all polygonal numbers up to a certain value

    #region Methods to test if a number is polygonal

    /// <summary>
    /// Test to see if a number is a polygonal number of side s.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="x"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static bool IsPolygonal(uint s, uint x, out uint? n)
    {
        // Guard.
        if (s < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(s),
                "The fewest number of sides a polygon can have is 3.");
        }
        if (x < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                "Must be at least 1.");
        }

        uint s2 = s - 2;
        uint s4 = s - 4;
        double dN = (Sqrt((8 * s2 * x) + (s4 * s4)) + s4) / (2 * s2);

        if (double.IsInteger(dN))
        {
            n = (uint)dN;
            return true;
        }

        n = null;
        return false;
    }

    public static bool IsTriangular(uint x, out uint? n) =>
        IsPolygonal(3, x, out n);

    public static bool IsSquare(uint x, out uint? n) =>
        IsPolygonal(4, x, out n);

    public static bool IsPentagonal(uint x, out uint? n) =>
        IsPolygonal(5, x, out n);

    public static bool IsHexagonal(uint x, out uint? n) =>
        IsPolygonal(6, x, out n);

    public static bool IsHeptagonal(uint x, out uint? n) =>
        IsPolygonal(7, x, out n);

    public static bool IsOctagonal(uint x, out uint? n) =>
        IsPolygonal(8, x, out n);

    #endregion Methods to test if a number is polygonal
}
