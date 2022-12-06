using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using AstroMultimedia.Core.Exceptions;
using AstroMultimedia.Core.Numbers;
using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Types;

/// <summary>
/// Encapsulates a fraction.
///
/// Unlike Java's BigRational, the fractions are not automatically simplified (reduced) because
/// this is a bit slow and not always necessary. So, you have call Simplify() yourself after
/// when needed.
///
/// <see href="https://en.wikipedia.org/wiki/Fraction" />
/// <see href="https://introcs.cs.princeton.edu/java/92symbolic/BigRational.java.html" />
/// <see href="https://github.com/danm-de/Fractions" />
/// </summary>
public struct Fraction : IEquatable<Fraction>, IFormattable, IParsable<Fraction>,
    IUnaryNegationOperators<Fraction, Fraction>,
    IAdditionOperators<Fraction, Fraction, Fraction>,
    ISubtractionOperators<Fraction, Fraction, Fraction>,
    IMultiplyOperators<Fraction, Fraction, Fraction>,
    IDivisionOperators<Fraction, Fraction, Fraction>,
    IComparisonOperators<Fraction, Fraction, bool>
{
    #region Constructors

    /// <summary>
    /// Fractions are not automatically simplified, because Simplify() is slow.
    /// Simplify() must be called manually as needed.
    /// </summary>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Fraction(BigInteger numerator, BigInteger denominator)
    {
        // A fraction with a 0 denominator is undefined.
        if (denominator == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(denominator),
                "The denominator cannot be 0.");
        }

        // If the numerator is 0, set the denominator to 1 so it matches Zero.
        if (numerator == 0)
        {
            denominator = 1;
        }
        else if (denominator < 0)
        {
            // Ensure the denominator is positive.
            numerator = -numerator;
            denominator = -denominator;
        }

        Numerator = numerator;
        Denominator = denominator;
    }

    public Fraction(BigInteger numerator) : this(numerator, BigInteger.One)
    {
    }

    #endregion Constructors

    #region Properties

    public BigInteger Numerator { get; set; }

    public BigInteger Denominator { get; set; }

    public static Fraction Zero => new (0, 1);

    public static Fraction One => new (1, 1);

    #endregion Properties

    #region Equality methods

    public bool Equals(Fraction frac2)
    {
        // If both numerators are 0 then we don't care what the denominators are.
        if (Numerator == 0 && frac2.Numerator == 0)
        {
            return true;
        }

        // See if the numerators and denominators are equal.
        if (Numerator == frac2.Numerator && Denominator == frac2.Denominator)
        {
            return true;
        }

        // We could simplify the fractions here and compare numerators and denominators again,
        // but Simplify() is a little slow and I think comparing double equivalents will do for
        // most practical purposes.

        // Compare double equivalents.
        return ((double)this).FuzzyEquals(frac2);
    }

    public override bool Equals(object? obj) =>
        obj is Fraction frac2 && Equals(frac2);

    public override int GetHashCode() =>
        HashCode.Combine(Numerator, Denominator);

    #endregion Equality methods

    #region String methods

    /// <summary>
    /// Format the fraction as a string.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentFormatException"></exception>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        // Default to the Unicode version.
        if (String.IsNullOrEmpty(format))
        {
            format = "U";
        }

        switch (format.ToUpperInvariant())
        {
            // ASCII.
            case "A":
                return $"{Numerator}/{Denominator}";

            // Unicode.
            case "U":
                return $"{Numerator.ToSuperscriptString()}/{Denominator.ToSubscriptString()}";

            default:
                throw new ArgumentFormatException(nameof(format),
                    $"The provided format string is not supported.");
        }
    }

    /// <summary>
    /// Format the fraction as a string.
    /// </summary>
    public string ToString(string format) =>
        ToString(format, CultureInfo.CurrentCulture);

    /// <summary>
    /// Format the fraction as a string.
    /// The is the default override version, which uses Unicode characters for a nicer format.
    /// </summary>
    public override string ToString() =>
        ToString("U", CultureInfo.CurrentCulture);

    /// <summary>
    /// Parse a string into a fraction.
    /// This version of the method is required to implement IParsable[Fraction], but it's more
    /// likely people will call the version that doesn't have the provider parameter.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentFormatException"></exception>
    public static Fraction Parse(string s, IFormatProvider? provider)
    {
        // Check a value was provided.
        if (string.IsNullOrWhiteSpace(s))
        {
            throw new ArgumentNullException(nameof(s), "Cannot parse a null or empty string.");
        }

        // Just support ASCII for now.
        // I may support the superscript/subscript version later (as generated by ToString()), but
        // it's probably unnecessary.
        Match match = Regex.Match(s, @"^(?<numerator>-?\d+)/(?<denominator>-?\d+)$");
        if (!match.Success)
        {
            throw new ArgumentFormatException(nameof(s),
                "Incorrect format. The correct format is int/int, e.g. 22/7 or -3/4.");
        }

        BigInteger numerator = BigInteger.Parse(match.Groups["numerator"].Value);
        BigInteger denominator = BigInteger.Parse(match.Groups["denominator"].Value);
        return new Fraction(numerator, denominator);
    }

    /// <summary>
    /// Parse a string into a fraction.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentFormatException"></exception>
    public static Fraction Parse(string s) =>
        Parse(s, null);

    /// <summary>
    /// Try to parse a string into a fraction.
    /// This version of the method is required to implement IParsable[Fraction], but it's more
    /// likely people will call the version that doesn't have the provider parameter.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool TryParse(string? s, IFormatProvider? provider, out Fraction result)
    {
        // Check a value was provided.
        if (string.IsNullOrWhiteSpace(s))
        {
            throw new ArgumentNullException(nameof(s), "Cannot parse a null or empty string.");
        }

        // Try to parse the provided string.
        try
        {
            result = Parse(s, provider);
        }
        catch (Exception)
        {
            result = default(Fraction);
            return false;
        }

        // All good.
        return true;
    }

    /// <summary>
    /// Try to parse a string into a fraction.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool TryParse(string? s, out Fraction result) =>
        TryParse(s, null, out result);

    #endregion String methods

    #region Other methods

    /// <summary>
    /// Clone a fraction.
    /// </summary>
    public Fraction Clone() =>
        (Fraction)MemberwiseClone();

    /// <summary>
    /// Find a numerator and denominator that fits the given real value within a given tolerance.
    /// Warning: this can be slow.
    /// </summary>
    public static Fraction Find(double x, double tolerance = XDouble.DELTA)
    {
        // Optimizations.
        if (x == 0)
        {
            return Zero;
        }
        if (double.IsInteger(x))
        {
            return new Fraction((BigInteger)x);
        }

        // Get the sign of the input value as 1 or -1.
        sbyte sign = (sbyte)double.CopySign(1, x);

        // Make the value positive.
        x = Abs(x);

        // Start with a denominator of 1 and increment until we find a good match.
        BigInteger denominator = 1;
        double nRounded;
        while (true)
        {
            // Calculate the numerator for this denominator, and see if it's an integer (or very
            // close to).
            double numerator = x * (double)denominator;
            nRounded = Round(numerator);
            if (nRounded > 0 && numerator.FuzzyEquals(nRounded, tolerance))
            {
                break;
            }

            // Next.
            denominator++;
        }

        return new Fraction((BigInteger)nRounded * sign, denominator);
    }

    /// <summary>
    /// Simplify a fraction given as a numerator and denominator.
    /// </summary>
    public static (BigInteger, BigInteger) Simplify(BigInteger numerator, BigInteger denominator)
    {
        // Guard.
        if (denominator == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(denominator),
                "The denominator cannot be 0.");
        }

        // Optimizations.
        if (numerator == 0)
        {
            return (0, 1);
        }
        if (numerator == 1)
        {
            return (1, denominator);
        }
        if (denominator == 1)
        {
            return (numerator, 1);
        }
        if (numerator == denominator)
        {
            return (1, 1);
        }

        // Get the greatest common divisor.
        BigInteger gcd = Functions.GreatestCommonDivisor(numerator, denominator);

        // If we found it, divide.
        if (gcd > 1)
        {
            numerator /= gcd;
            denominator /= gcd;
        }

        return (numerator, denominator);
    }

    /// <summary>
    /// Simplify a fraction.
    /// </summary>
    public static Fraction Simplify(Fraction frac) =>
        new (frac.Numerator, frac.Denominator);

    /// <summary>
    /// Find the reciprocal.
    /// </summary>
    public static Fraction Reciprocal(Fraction frac) =>
        new (frac.Denominator, frac.Numerator);

    #endregion Miscellaneous methods

    #region Arithmetic methods

    /// <summary>
    /// Exponentiation (integer exponent).
    /// This version leverages BigInteger.Pow().
    /// </summary>
    public static Fraction Pow(Fraction frac, int exp)
    {
        // Optimizations.
        switch (exp)
        {
            case 0:
                return One;

            case 1:
                return frac.Clone();

            case -1:
                return Reciprocal(frac);
        }

        // Raise the numerator and denominator to the power of Abs(i).
        int sign = Sign(exp);
        exp = Abs(exp);
        BigInteger numerator = BigInteger.Pow(frac.Numerator, exp);
        BigInteger denominator = BigInteger.Pow(frac.Denominator, exp);

        // If the sign is negative, invert the fraction.
        return (sign < 0)
            ? new Fraction(denominator, numerator)
            : new Fraction(numerator, denominator);
    }

    /// <summary>
    /// Exponentiation (double exponent).
    /// There is a risk of producing an imperfect fraction due to lack of precision in converting
    /// doubles to Fractions.
    /// </summary>
    public static Fraction Pow(Fraction frac, double exp) =>
        Find(Math.Pow(frac, exp));

    /// <summary>
    /// Exponentiation (fraction exponent).
    /// There is a risk of producing an imperfect fraction due to lack of precision in converting
    /// doubles to Fractions.
    /// </summary>
    public static Fraction Pow(Fraction frac, Fraction exp) =>
        Pow(frac, (double)exp);

    /// <summary>
    /// Find the square root of a fraction as a fraction.
    /// </summary>
    public static Fraction Sqrt(Fraction frac) =>
        Find(Math.Sqrt(frac));

    #endregion Arithmetic methods

    #region Cast operators

    /// <summary>
    /// Implicitly cast an int to a fraction.
    /// </summary>
    public static implicit operator Fraction(int numerator) =>
        new (numerator);

    /// <summary>
    /// Implicitly cast an BigInteger to a fraction.
    /// </summary>
    public static implicit operator Fraction(BigInteger numerator) =>
        new (numerator);

    /// <summary>
    /// Implicitly cast a double to a fraction.
    /// </summary>
    public static implicit operator Fraction(double x) =>
        Find(x);

    /// <summary>
    /// Implicitly cast a fraction to a double.
    /// </summary>
    public static implicit operator double(Fraction frac) =>
        (double)frac.Numerator / (double)frac.Denominator;

    #endregion Cast operators

    #region Arithmetic operators

    /// <summary>
    /// Unary negation operator.
    /// </summary>
    public static Fraction operator -(Fraction frac) =>
        new (-frac.Numerator, frac.Denominator);

    /// <summary>
    /// Addition operator.
    /// </summary>
    public static Fraction operator +(Fraction frac, Fraction frac2)
    {
        BigInteger numerator =
            frac.Numerator * frac2.Denominator + frac2.Numerator * frac.Denominator;
        BigInteger denominator = frac.Denominator * frac2.Denominator;
        return new Fraction(numerator, denominator);
    }

    /// <summary>
    /// Subtraction operator.
    /// </summary>
    public static Fraction operator -(Fraction frac, Fraction frac2) =>
        frac + (-frac2);

    /// <summary>
    /// Reciprocal operator.
    /// </summary>
    public static Fraction operator ~(Fraction frac) =>
        Reciprocal(frac);

    /// <summary>
    /// Multiply a fraction by a fraction.
    /// </summary>
    public static Fraction operator *(Fraction frac, Fraction frac2) =>
        new (frac.Numerator * frac2.Numerator, frac.Denominator * frac2.Denominator);

    /// <summary>
    /// Divide a fraction by a fraction.
    /// </summary>
    public static Fraction operator /(Fraction frac, Fraction frac2) =>
        new (frac.Numerator * frac2.Denominator, frac.Denominator * frac2.Numerator);

    /// <summary>
    /// Exponentiation operator (integer exponent).
    /// </summary>
    public static Fraction operator ^(Fraction frac, int i) =>
        Pow(frac, i);

    /// <summary>
    /// Exponentiation operator (double exponent).
    /// </summary>
    public static Fraction operator ^(Fraction frac, double x) =>
        Pow(frac, x);

    /// <summary>
    /// Exponentiation operator (fraction exponent).
    /// </summary>
    public static Fraction operator ^(Fraction frac, Fraction frac2) =>
        Pow(frac, frac2);

    #endregion Arithmetic operators

    #region Comparison operators

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(Fraction frac, Fraction frac2) =>
        frac.Equals(frac2);

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(Fraction frac, Fraction frac2) =>
        !(frac == frac2);

    /// <summary>
    /// Less than operator.
    /// </summary>
    public static bool operator <(Fraction frac, Fraction frac2) =>
        (double)frac < (double)frac2;

    /// <summary>
    /// Greater than operator.
    /// </summary>
    public static bool operator >(Fraction frac, Fraction frac2) =>
        (double)frac > (double)frac2;

    /// <summary>
    /// Less than or equal to operator.
    /// </summary>
    public static bool operator <=(Fraction frac, Fraction frac2) =>
        frac == frac2 || frac < frac2;

    /// <summary>
    /// Greater than or equal to operator.
    /// </summary>
    public static bool operator >=(Fraction frac, Fraction frac2) =>
        frac == frac2 || frac > frac2;

    #endregion Comparison operators
}
