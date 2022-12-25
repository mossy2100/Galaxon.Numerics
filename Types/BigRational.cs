using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using AstroMultimedia.Core.Exceptions;
using AstroMultimedia.Core.Numbers;
using AstroMultimedia.Core.Strings;
using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Types;

/// <summary>
/// Encapsulates a rational.
/// Unlike Java's BigRational, the rationals are not automatically reduced (reduced) because
/// this is a bit slow and not always required. So, you can pass true to the constructor, or call
/// Reduce() yourself when needed.
/// <see href="https://en.wikipedia.org/wiki/Rational_number" />
/// <see href="https://introcs.cs.princeton.edu/java/92symbolic/BigRational.java.html" />
/// <see href="https://github.com/danm-de/BigRationals" />
/// </summary>
public struct BigRational : IEquatable<BigRational>, IFormattable, IParsable<BigRational>,
    IUnaryNegationOperators<BigRational, BigRational>,
    IAdditionOperators<BigRational, BigRational, BigRational>,
    ISubtractionOperators<BigRational, BigRational, BigRational>,
    IMultiplyOperators<BigRational, BigRational, BigRational>,
    IDivisionOperators<BigRational, BigRational, BigRational>,
    IComparisonOperators<BigRational, BigRational, bool>
{
    #region Constructors

    /// <summary>
    /// BigRationals are not automatically reduced, because Reduce() is slow.
    /// Either set reduce = true, or call Reduce() as needed.
    /// </summary>
    /// <param name="num"></param>
    /// <param name="den"></param>
    /// <param name="reduce"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public BigRational(BigInteger num, BigInteger den, bool reduce = false)
    {
        // A rational with a 0 denominator is undefined.
        if (den == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(den), "A rational with a denominator of 0 is undefined.");
        }

        // If the numerator is 0, set the denominator to 1 so it matches Zero.
        if (num == 0)
        {
            den = 1;
        }
        else if (den < 0)
        {
            // Ensure the denominator is positive.
            num = -num;
            den = -den;
        }

        // Reduce if necessary.
        if (reduce)
        {
            (num, den) = Reduce(num, den);
        }

        Numerator = num;
        Denominator = den;
    }

    public BigRational(BigInteger num) : this(num, 1)
    {
    }

    #endregion Constructors

    #region Properties

    public BigInteger Numerator { get; set; }

    public BigInteger Denominator { get; set; }

    public static BigRational Zero => new (0, 1);

    public static BigRational One => new (1, 1);

    #endregion Properties

    #region Equality methods

    public bool Equals(BigRational br2)
    {
        // If both numerators are 0 then we don't care what the denominators are.
        if (Numerator == 0 && br2.Numerator == 0)
        {
            return true;
        }

        // See if the numerators and denominators are equal.
        if (Numerator == br2.Numerator && Denominator == br2.Denominator)
        {
            return true;
        }

        // We could reduce the rationals here and compare numerators and denominators again,
        // but Reduce() is a little slow and I think comparing double equivalents will do for
        // most practical purposes.

        // Compare double equivalents.
        return ((double)this).FuzzyEquals((double)br2);
    }

    public override bool Equals(object? obj) =>
        obj is BigRational br2 && Equals(br2);

    public override int GetHashCode() =>
        HashCode.Combine(Numerator, Denominator);

    #endregion Equality methods

    #region String methods

    /// <summary>
    /// Format the rational as a string.
    /// </summary>
    /// <todo>
    /// Update to support standard format strings for integers, namely D, N, R, with the optional U
    /// code, same as for BigDecimal. Remove "A", keep "M" for mixed.
    /// </todo>
    /// <param name="format"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentFormatException"></exception>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        // Default to the Unicode version.
        if (string.IsNullOrEmpty(format))
        {
            format = "U";
        }

        // If the denominator is 1 then just return the numerator as a string.
        if (Denominator == 1)
        {
            return Numerator.ToString();
        }

        // Get the parts we need.
        string sign = this < 0 ? "-" : "";
        BigInteger num = BigInteger.Abs(Numerator);
        BigInteger den = BigInteger.Abs(Denominator);

        switch (format.ToUpper())
        {
            // ASCII.
            case "A":
                return $"{sign}{num}/{den}";

            // Unicode.
            case "U":
                return $"{sign}{num.ToSuperscript()}/{den.ToSubscript()}";

            // Mixed.
            case "M":
                // Format improper fractions (or rationals that have a numerator with a larger
                // magnitude than the denominator) with a quotient and remainder, e.g 2³/₄
                if (num > den)
                {
                    BigInteger quot = num / den;
                    BigInteger rem = num % den;
                    return $"{sign}{quot}{rem.ToSuperscript()}/{den.ToSubscript()}";
                }
                return $"{sign}{num.ToSuperscript()}/{den.ToSubscript()}";

            default:
                throw new ArgumentFormatException(nameof(format),
                    "The provided format string is not supported.");
        }
    }

    /// <summary>
    /// Format the rational as a string.
    /// </summary>
    public string ToString(string format) =>
        ToString(format, CultureInfo.CurrentCulture);

    /// <summary>
    /// Format the rational as a string.
    /// The is the default override version, which uses Unicode characters for a nicer format.
    /// </summary>
    public override string ToString() =>
        ToString("U", CultureInfo.CurrentCulture);

    /// <summary>
    /// Parse a string into a rational.
    /// This version of the method is required to implement IParsable[BigRational], but it's more
    /// likely people will call the version that doesn't have the provider parameter.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentFormatException"></exception>
    public static BigRational Parse(string s, IFormatProvider? provider)
    {
        // Check a value was provided.
        if (string.IsNullOrWhiteSpace(s))
        {
            throw new ArgumentNullException(nameof(s), "Cannot parse a null or empty string.");
        }

        // Just support ASCII for now.
        // I may support the superscript/subscript version later (as generated by ToString()), but
        // it's probably unnecessary.
        Match match = Regex.Match(s, @"^(?<num>-?\d+)/(?<den>-?\d+)$");
        if (!match.Success)
        {
            throw new ArgumentFormatException(nameof(s),
                "Incorrect format. The correct format is int/int, e.g. 22/7 or -3/4.");
        }

        BigInteger num = BigInteger.Parse(match.Groups["num"].Value);
        BigInteger den = BigInteger.Parse(match.Groups["den"].Value);
        return new BigRational(num, den);
    }

    /// <summary>
    /// Parse a string into a rational.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentFormatException"></exception>
    public static BigRational Parse(string s) =>
        Parse(s, null);

    /// <summary>
    /// Try to parse a string into a rational.
    /// This version of the method is required to implement IParsable[BigRational], but it's more
    /// likely people will call the version that doesn't have the provider parameter.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool TryParse(string? s, IFormatProvider? provider, out BigRational result)
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
            result = default(BigRational);
            return false;
        }

        // All good.
        return true;
    }

    /// <summary>
    /// Try to parse a string into a rational.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool TryParse(string? s, out BigRational result) =>
        TryParse(s, null, out result);

    #endregion String methods

    #region Other methods

    /// <summary>
    /// Clone a rational.
    /// </summary>
    public BigRational Clone() =>
        (BigRational)MemberwiseClone();

    /// <summary>
    /// Find a numerator and denominator that fits the given real value within a given tolerance.
    /// Warning: this can be slow.
    /// </summary>
    public static BigRational Find(double x, double tolerance = XDouble.Delta)
    {
        // Optimizations.
        if (x == 0)
        {
            return Zero;
        }
        if (double.IsInteger(x))
        {
            return new BigRational((BigInteger)x);
        }

        // Get the sign of the input value as 1 or -1.
        sbyte sign = (sbyte)double.CopySign(1, x);

        // Make the value positive.
        x = Abs(x);

        // Start with a denominator of 1 and increment until we find a good match.
        BigInteger den = 1;
        double nRounded;
        while (true)
        {
            // Calculate the numerator for this denominator, and see if it's an integer (or very
            // close to).
            double num = x * (double)den;
            nRounded = Round(num);
            if (nRounded > 0 && num.FuzzyEquals(nRounded, tolerance))
            {
                break;
            }

            // Next.
            den++;
        }

        return new BigRational((BigInteger)nRounded * sign, den);
    }

    /// <summary>
    /// Reduce a rational given as a numerator and denominator.
    /// I've made this version, which doesn't receive or return a BigRational object, so it can be
    /// called from the constructor.
    /// </summary>
    public static (BigInteger, BigInteger) Reduce(BigInteger num, BigInteger den)
    {
        // Guard.
        if (den == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(den),
                "The denominator cannot be 0.");
        }

        // Optimizations.
        if (num == 0)
        {
            return (0, 1);
        }
        if (num == 1)
        {
            return (1, den);
        }
        if (den == 1)
        {
            return (num, 1);
        }
        if (num == den)
        {
            return (1, 1);
        }

        // Get the greatest common divisor.
        BigInteger gcd = Divisors.GreatestCommonDivisor(num, den);

        // If we found one greater than 1, divide.
        if (gcd > 1)
        {
            num /= gcd;
            den /= gcd;
        }

        return (num, den);
    }

    /// <summary>
    /// Reduce a rational.
    /// </summary>
    public static BigRational Reduce(BigRational br) =>
        new (br.Numerator, br.Denominator);

    /// <summary>
    /// Find the reciprocal.
    /// Slightly faster than using 1/br.
    /// </summary>
    public static BigRational Reciprocal(BigRational br) =>
        new (br.Denominator, br.Numerator);

    #endregion Miscellaneous methods

    #region Arithmetic methods

    /// <summary>
    /// Exponentiation (integer exponent).
    /// This version leverages BigInteger.Pow().
    /// </summary>
    public static BigRational Pow(BigRational br, int exp)
    {
        // Optimizations.
        switch (exp)
        {
            case 0:
                return One;

            case 1:
                return br.Clone();

            case -1:
                return Reciprocal(br);
        }

        // Raise the numerator and denominator to the power of Abs(i).
        int sign = Sign(exp);
        exp = Abs(exp);
        BigInteger num = BigInteger.Pow(br.Numerator, exp);
        BigInteger den = BigInteger.Pow(br.Denominator, exp);

        // If the sign is negative, invert the rational.
        return sign < 0
            ? new BigRational(den, num)
            : new BigRational(num, den);
    }

    /// <summary>
    /// Exponentiation (double exponent).
    /// There is a risk of producing an imperfect rational due to lack of precision in converting
    /// doubles to BigRationals.
    /// </summary>
    public static BigRational Pow(BigRational br, double exp) =>
        Find(Math.Pow((double)br, exp));

    /// <summary>
    /// Exponentiation (rational exponent).
    /// There is a risk of producing an imperfect rational due to lack of precision in converting
    /// doubles to BigRationals.
    /// </summary>
    public static BigRational Pow(BigRational br, BigRational exp) =>
        Pow(br, (double)exp);

    /// <summary>
    /// Find the square root of a rational as a rational.
    /// </summary>
    public static BigRational Sqrt(BigRational br) =>
        Find(Math.Sqrt((double)br));

    #endregion Arithmetic methods

    #region Cast operators

    /// <summary>
    /// Implicitly cast an int to a rational.
    /// </summary>
    public static implicit operator BigRational(int num) =>
        new (num);

    /// <summary>
    /// Implicitly cast an BigInteger to a rational.
    /// </summary>
    public static implicit operator BigRational(BigInteger num) =>
        new (num);

    /// <summary>
    /// Explicitly cast a double to a rational.
    /// </summary>
    public static explicit operator BigRational(double x) =>
        Find(x);

    /// <summary>
    /// Explicitly cast a rational to a double.
    /// </summary>
    public static explicit operator double(BigRational br) =>
        (double)br.Numerator / (double)br.Denominator;

    #endregion Cast operators

    #region Arithmetic operators

    /// <summary>
    /// Unary negation operator.
    /// </summary>
    public static BigRational operator -(BigRational br) =>
        new (-br.Numerator, br.Denominator);

    /// <summary>
    /// Addition operator.
    /// </summary>
    public static BigRational operator +(BigRational br, BigRational br2)
    {
        BigInteger num =
            br.Numerator * br2.Denominator + br2.Numerator * br.Denominator;
        BigInteger den = br.Denominator * br2.Denominator;
        return new BigRational(num, den);
    }

    /// <summary>
    /// Subtraction operator.
    /// </summary>
    public static BigRational operator -(BigRational br, BigRational br2) =>
        br + -br2;

    /// <summary>
    /// Reciprocal operator.
    /// Slightly faster than using 1/br.
    /// </summary>
    public static BigRational operator ~(BigRational br) =>
        Reciprocal(br);

    /// <summary>
    /// Multiply a rational by a rational.
    /// </summary>
    public static BigRational operator *(BigRational br, BigRational br2) =>
        new (br.Numerator * br2.Numerator, br.Denominator * br2.Denominator);

    /// <summary>
    /// Divide a rational by a rational.
    /// </summary>
    public static BigRational operator /(BigRational br, BigRational br2) =>
        new (br.Numerator * br2.Denominator, br.Denominator * br2.Numerator);

    /// <summary>
    /// Exponentiation operator (integer exponent).
    /// </summary>
    public static BigRational operator ^(BigRational br, int i) =>
        Pow(br, i);

    /// <summary>
    /// Exponentiation operator (double exponent).
    /// </summary>
    public static BigRational operator ^(BigRational br, double x) =>
        Pow(br, x);

    /// <summary>
    /// Exponentiation operator (rational exponent).
    /// </summary>
    public static BigRational operator ^(BigRational br, BigRational br2) =>
        Pow(br, br2);

    #endregion Arithmetic operators

    #region Comparison operators

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(BigRational br, BigRational br2) =>
        br.Equals(br2);

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(BigRational br, BigRational br2) =>
        !(br == br2);

    /// <summary>
    /// Less than operator.
    /// </summary>
    public static bool operator <(BigRational br, BigRational br2) =>
        (double)br < (double)br2;

    /// <summary>
    /// Greater than operator.
    /// </summary>
    public static bool operator >(BigRational br, BigRational br2) =>
        (double)br > (double)br2;

    /// <summary>
    /// Less than or equal to operator.
    /// </summary>
    public static bool operator <=(BigRational br, BigRational br2) =>
        br == br2 || br < br2;

    /// <summary>
    /// Greater than or equal to operator.
    /// </summary>
    public static bool operator >=(BigRational br, BigRational br2) =>
        br == br2 || br > br2;

    #endregion Comparison operators
}
