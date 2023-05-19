namespace Galaxon.Numerics.Geometry;

public static class Angle
{
    #region String methods

    /// <summary>
    /// Formats the angle as degrees, arcminutes, and arcseconds.
    /// e.g. The angle 4.0 radians is equal to 229.183118052329° which is converted to the string
    /// 229° 10′ 59.225″
    /// NB: No sign is shown for the arcminutes and arcseconds values.
    /// However, if the degrees value is negative, then the arcminutes and arcseconds values will
    /// also be negative (or zero).
    /// </summary>
    /// <param name="degrees">The angle in degrees.</param>
    /// <param name="scale">
    /// The number of decimal places to display for the arcseconds value.
    /// </param>
    /// <returns>A string with the formatted angle.</returns>
    public static string FormatDms(double degrees, byte scale = 0)
    {
        (var wholeDegrees, var arcminutes, var arcseconds) = DegToDms(degrees);
        var arcsecondsString = Abs(arcseconds).ToString($"F{scale}");
        return $"{wholeDegrees}° {Abs(arcminutes)}′ {arcsecondsString}″";
    }

    #endregion String methods

    #region Normalize methods

    /// <summary>
    /// Add or subtract multiples of τ so the angle fits within a standard range.
    /// <ul>
    ///     <li>For signed (default), the range will be [-PI..PI)</li>
    ///     <li>For unsigned, the range will be [0..TAU)</li>
    /// </ul>
    /// </summary>
    public static double NormalizeRadians(double radians, bool signed = true)
    {
        radians -= Floor(radians / Tau) * Tau;
        if (signed && radians >= PI)
        {
            radians -= Tau;
        }
        return radians;
    }

    /// <summary>
    /// Add or subtract multiples of 360° so the angle fits within a standard range.
    /// <ul>
    ///     <li>For signed (default), the range will be [-180..180)</li>
    ///     <li>For unsigned, the range will be [0..360)</li>
    /// </ul>
    /// </summary>
    public static double NormalizeDegrees(double degrees, bool signed = true)
    {
        degrees -= Floor(degrees / DegreesPerCircle) * DegreesPerCircle;
        if (signed && degrees >= DegreesPerSemicircle)
        {
            degrees -= DegreesPerCircle;
        }
        return degrees;
    }

    #endregion Normalize methods

    #region Conversion methods

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    /// <param name="degrees">An angle size in degrees.</param>
    /// <returns>The angle size in radians.</returns>
    public static double DegToRad(double degrees) => degrees * RadiansPerDegree;

    /// <summary>
    /// Convert radians to degrees.
    /// </summary>
    /// <param name="radians">An angle size in radians.</param>
    /// <returns>The angle size in degrees.</returns>
    public static double RadToDeg(double radians) => radians * DegreesPerRadian;

    /// <summary>
    /// Creates a new angle from degrees, arcminutes, and (optionally) arcseconds.
    /// In normal usage all values should have the same sign or be zero.
    /// Consider an angle expressed as -2° 20' 14". This really means -(2° 20' 14").
    /// So, although there's only one minus sign, this value is actually equal to -2° -20' -14".
    /// Therefore, if degrees are supplied as negative, but the arcminutes or arcseconds are
    /// positive, this is probably an error.
    /// However, no exception will be thrown, so beware of that.
    /// </summary>
    /// <param name="degrees">The number of degrees.</param>
    /// <param name="arcminutes">The number of arcminutes.</param>
    /// <param name="arcseconds">The number of arcseconds.</param>
    /// <returns>The angle in degrees.</returns>
    public static double DmsToDeg(double degrees, double arcminutes, double arcseconds = 0) =>
        degrees + arcminutes / ArcminutesPerDegree + arcseconds / ArcsecondsPerDegree;

    /// <summary>
    /// Convert degrees, arcminutes, and arcseconds to radians.
    /// </summary>
    /// <param name="degrees">The number of degrees.</param>
    /// <param name="arcminutes">The number of arcminutes.</param>
    /// <param name="arcseconds">The number of arcseconds.</param>
    /// <returns>The angle in radians.</returns>
    public static double DmsToRad(double degrees, double arcminutes, double arcseconds = 0) =>
        DegToRad(DmsToDeg(degrees, arcminutes, arcseconds));

    /// <summary>
    /// Convert an angle from degrees to degrees, arcminutes, and arcseconds.
    /// The degrees and arcminutes values will be whole numbers, but the arcseconds value could have
    /// a fractional part.
    /// The arcminutes and arcseconds values will have the same sign as the degrees value, or be
    /// zero.
    /// </summary>
    /// <param name="degrees">The angle in degrees.</param>
    /// <returns>
    /// A tuple containing 3 double values representing degrees, arcminutes, and arcseconds.
    /// </returns>
    public static (double degrees, double arcminutes, double arcseconds) DegToDms(double degrees)
    {
        var wholeDegrees = Truncate(degrees);
        var fracDegrees = degrees - wholeDegrees;
        var arcminutes = fracDegrees * ArcminutesPerDegree;
        var wholeArcminutes = Truncate(arcminutes);
        var fracArcminutes = arcminutes - wholeArcminutes;
        var arcseconds = fracArcminutes * ArcsecondsPerArcminute;
        return (degrees: wholeDegrees, arcminutes: wholeArcminutes, arcseconds);
    }

    /// <summary>
    /// Convert an angle from radians to degrees, arcminutes, and arcseconds.
    /// The degrees and arcminutes values will be whole numbers, but the arcseconds value could have
    /// a fractional part.
    /// The arcminutes and arcseconds values will have the same sign as the degrees value, or be
    /// zero.
    /// </summary>
    /// <param name="radians">The angle in radians.</param>
    /// <returns>
    /// A tuple containing 3 double values representing degrees, arcminutes, and arcseconds.
    /// </returns>
    public static (double degrees, double arcminutes, double arcseconds) RadToDms(double radians) =>
        DegToDms(RadToDeg(radians));

    #endregion Conversion methods

    #region Trigonometric methods

    /// <summary>
    /// The square of the sine of an angle in radians.
    /// </summary>
    /// <param name="radians">The size of an angle in radians.</param>
    /// <returns></returns>
    public static double Sin2(double radians) => Pow(Sin(radians), 2);

    /// <summary>
    /// The square of the sine of an angle in radians.
    /// </summary>
    /// <param name="radians">The size of an angle in radians.</param>
    /// <returns></returns>
    public static double Cos2(double radians) => Pow(Cos(radians), 2);

    /// <summary>
    /// The square of the tangent of an angle in radians.
    /// </summary>
    /// <param name="radians">The size of an angle in radians.</param>
    /// <returns></returns>
    public static double Tan2(double radians) => Pow(Tan(radians), 2);

    /// <summary>
    /// The sine of an angle in degrees.
    /// </summary>
    public static double SinDeg(double degrees) => Sin(DegToRad(degrees));

    /// <summary>
    /// The cosine of an angle in degrees.
    /// </summary>
    public static double CosDeg(double degrees) => Cos(DegToRad(degrees));

    /// <summary>
    /// The tangent of an angle in degrees.
    /// </summary>
    public static double TanDeg(double degrees) => Tan(DegToRad(degrees));

    #endregion Trigonometric methods

    #region Constants

    public const long DegreesPerCircle = 360;
    public const long DegreesPerSemicircle = 180;

    public const long ArcminutesPerDegree = 60;
    public const long ArcminutesPerCircle = ArcminutesPerDegree * DegreesPerCircle;

    public const long ArcsecondsPerArcminute = 60;
    public const long ArcsecondsPerDegree = ArcsecondsPerArcminute * ArcminutesPerDegree;
    public const long ArcsecondsPerCircle = ArcsecondsPerDegree * DegreesPerCircle;

    public const double RadiansPerCircle = Tau;
    public const double RadiansPerSemicircle = PI;

    public const double RadiansPerDegree = 1.745329251994329576923691e-2;
    public const double DegreesPerRadian = 57.29577951308232087679815;

    public const double RadiansPerArcsecond = 4.848136811095359935899141e-6;
    public const double ArcsecondsPerRadian = 206264.8062470963551564734;

    #endregion Constants
}
