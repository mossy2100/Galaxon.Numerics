using Galaxon.Core.Numbers;

namespace Galaxon.Numerics.Geometry;

public class Ellipsoid : IShape3D
{
    #region Constructor

    public Ellipsoid(double radiusA, double radiusB, double radiusC)
    {
        RadiusA = radiusA;
        RadiusB = radiusB;
        RadiusC = radiusC;
    }

    #endregion Constructor

    #region Radii

    private double _radiusA;

    public double RadiusA
    {
        get => _radiusA;

        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RadiusA), "Must be positive.");
            }
            _radiusA = value;
        }
    }

    private double _radiusB;

    public double RadiusB
    {
        get => _radiusB;

        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RadiusB), "Must be positive.");
            }
            _radiusB = value;
        }
    }

    private double _radiusC;

    public double RadiusC
    {
        get => _radiusC;

        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RadiusC), "Must be positive.");
            }
            _radiusC = value;
        }
    }

    public (double, double, double) SortedRadii()
    {
        double[] radii = { RadiusA, RadiusB, RadiusC };
        // Sort radii in ascending order (much simpler than descending).
        Array.Sort(radii);
        // Return them in descending order, matching convention in formulae.
        return (radii[2], radii[1], radii[0]);
    }

    #endregion Radii

    #region Ellipsoid Types

    public bool IsSphere() => RadiusA.FuzzyEquals(RadiusB) && RadiusB.FuzzyEquals(RadiusC);

    public bool IsOblate()
    {
        var (a, b, c) = SortedRadii();
        return a.FuzzyEquals(b) && b > c;
    }

    public bool IsProlate()
    {
        var (a, b, c) = SortedRadii();
        return a > b && b.FuzzyEquals(c);
    }

    public bool IsSpheroid() => IsOblate() || IsProlate();

    public bool IsScalene()
    {
        var (a, b, c) = SortedRadii();
        return a > b && b > c;
    }

    #endregion Ellipsoid Types

    #region Calculated properties

    /// <summary>
    /// Get the surface area in square metres.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Ellipsoid#Surface_area"/>
    /// <see href="https://www.web-formulas.com/Math_Formulas/Geometry_Surface_of_Ellipsoid.aspx"/>
    /// <returns>The surface area.</returns>
    public virtual double SurfaceArea
    {
        get
        {
            // Check for simple cases to get a quicker answer.
            // Calculate intermediate variables as needed.
            var (a, b, c) = SortedRadii();
            var a2 = a * a;

            if (IsSphere())
            {
                // Formula: https://en.wikipedia.org/wiki/Sphere#Surface_area
                return 4 * PI * a2;
            }

            var c2 = c * c;
            var e2 = 1 - c2 / a2;
            var e = Sqrt(e2);

            if (IsOblate())
            {
                // Formula: https://en.wikipedia.org/wiki/Ellipsoid#Surface_area
                return Tau * a2 * (1 + (1 - e2) / e * Atanh(e));
            }

            var phi = Asin(e);

            if (IsProlate())
            {
                // Formula: https://en.wikipedia.org/wiki/Ellipsoid#Surface_area
                return Tau * c2 * (1 + a / (c * e) * phi);
            }

            // The ellipsoid is scalene.
            // Formula: https://keisan.casio.com/exec/system/1223392149
            // (This is more efficient than the formula in Wikipedia).
            var k = Sqrt(1 - c2 / (b * b)) / e;
            var (F, E) = EllipticIntegrals.FE(phi, k);
            return Tau * (c2 + a * b * e * E + b * c2 / (a * e) * F);
        }
    }

    /// <summary>
    /// The volume in cubic metres.
    /// </summary>
    public double Volume => 4 * PI / 3 * RadiusA * RadiusB * RadiusC;

    /// <summary>
    /// The volumetric mean radius in metres.
    /// </summary>
    public double VolumetricMeanRadius => double.RootN(RadiusA * RadiusB * RadiusC, 3);

    #endregion Calculated properties
}
