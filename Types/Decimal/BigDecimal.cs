using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using AstroMultimedia.Core.Exceptions;
using AstroMultimedia.Core.Strings;

namespace AstroMultimedia.Numerics.Types;

public struct BigDecimal : INumber<BigDecimal>
{
    #region Constructors

    public BigDecimal(BigInteger significand, BigInteger exponent)
    {
        (significand, exponent) = MakeCanonical(significand, exponent);
        Significand = significand;
        Exponent = exponent;
    }

    public BigDecimal(BigInteger significand) : this(significand, 0)
    {
    }

    public BigDecimal() : this(0, 0)
    {
    }

    #endregion Constructors

    #region Instance properties

    /// <summary>
    /// The part of a number in scientific notation or in floating-point representation, consisting
    /// of its significant digits.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Significand">Wikipedia: Significand</see>
    public BigInteger Significand { get; set; }

    /// <summary>The exponent on the base 10.</summary>
    public BigInteger Exponent { get; set; }

    #endregion Instance properties

    #region Static properties

    /// <inheritdoc />
    public static BigDecimal Zero { get; } = new (0, 0);

    /// <inheritdoc />
    public static BigDecimal One { get; } = new (1, 0);

    /// <inheritdoc />
    public static int Radix { get; } = 10;

    /// <inheritdoc />
    public static BigDecimal AdditiveIdentity { get; } = Zero;

    /// <inheritdoc />
    public static BigDecimal MultiplicativeIdentity { get; } = One;

    #endregion Static properties

    #region Arithmetic operators

    /// <inheritdoc />
    public static BigDecimal operator +(BigDecimal value) =>
        (BigDecimal)value.MemberwiseClone();

    /// <inheritdoc />
    public static BigDecimal operator +(BigDecimal left, BigDecimal right)
    {
        (BigDecimal x, BigDecimal y) = Align(left, right);
        BigDecimal result = new BigDecimal(x.Significand + y.Significand, x.Exponent);
        return result.MakeCanonical();
    }

    /// <inheritdoc />
    public static BigDecimal operator ++(BigDecimal value) =>
        value + One;

    /// <inheritdoc />
    public static BigDecimal operator -(BigDecimal value) =>
        new (-value.Significand, value.Exponent);

    /// <inheritdoc />
    public static BigDecimal operator -(BigDecimal left, BigDecimal right)
    {
        (BigDecimal x, BigDecimal y) = Align(left, right);
        BigDecimal result = new (x.Significand - y.Significand, x.Exponent);
        return result.MakeCanonical();
    }

    /// <inheritdoc />
    public static BigDecimal operator --(BigDecimal value) =>
        value - One;

    /// <inheritdoc />
    public static BigDecimal operator *(BigDecimal left, BigDecimal right)
    {
        BigDecimal result =
            new (left.Significand * right.Significand, left.Exponent + right.Exponent);
        return result.MakeCanonical();
    }

    public static BigDecimal operator /(BigDecimal left, BigDecimal right) =>
        throw new NotImplementedException();

    public static BigDecimal operator %(BigDecimal left, BigDecimal right) =>
        throw new NotImplementedException();

    #endregion Arithmetic operators

    #region Arithmetic methods

    /// <summary>
    /// Get the absolute value of the number.
    /// </summary>
    public static BigDecimal Abs(BigDecimal value) =>
        new (BigInteger.Abs(value.Significand), value.Exponent);

    /// <summary>
    /// Modify the provided significand and exponent as needed to find the canonical form.
    /// Static form of the method, for use in the constructor.
    /// </summary>
    /// <returns>The two updated BigIntegers.</returns>
    private static (BigInteger , BigInteger) MakeCanonical(BigInteger significand,
        BigInteger exponent)
    {
        // Canonical form of zero.
        if (significand == 0)
        {
            exponent = 0;
        }
        // Canonical form of other values.
        else
        {
            // Remove trailing 0s from the significand by incrementing the exponent.
            while (significand % 10 == 0)
            {
                significand /= 10;
                exponent++;
            }
        }
        return (significand, exponent);
    }

    /// <summary>
    /// Make the value into its canonical form.
    /// Any trailing 0s on the significand are removed, and this information is transferred to the
    /// exponent.
    /// This method mutates the object; it doesn't return a new object like most of the other
    /// methods, because no information is lost.
    /// </summary>
    /// <returns>The instance, which is useful for method chaining.</returns>
    private BigDecimal MakeCanonical()
    {
        (Significand, Exponent) = MakeCanonical(Significand, Exponent);
        return this;
    }

    /// <summary>
    /// Round off a value to a certain number of decimal places.
    /// </summary>
    /// <param name="bd"></param>
    /// <returns></returns>
    public static BigDecimal Round(BigDecimal bd, int nDecimalPlaces = 0) =>
        throw new NotImplementedException();

    /// <summary>
    /// Adjust the parts of one of the values so both have the same exponent.
    /// If necessary, a new object will be created rather than mutate the provided one.
    /// </summary>
    private static (BigDecimal, BigDecimal) Align(BigDecimal x, BigDecimal y)
    {
        // See if there's anything to do.
        if (x.Exponent == y.Exponent)
        {
            return (x, y);
        }

        // Get a and b as proxies for the operation so we don't mutate the original values.
        BigDecimal a;
        BigDecimal b;
        if (x.Exponent < y.Exponent)
        {
            a = x;
            b = (BigDecimal)y.MemberwiseClone();
        }
        else
        {
            a = (BigDecimal)y.MemberwiseClone();
            b = x;
        }

        // Shift b until they're aligned.
        while (b.Exponent > a.Exponent)
        {
            b.Significand *= 10;
            b.Exponent--;
        }

        return (a, b);
    }

    #endregion Arithmetic methods

    #region Inspection methods

    /// <summary>
    /// Checks if the value is in its canonical state.
    /// In this case, the value should not be evenly divisible by 10. In canonical form, a
    /// multiple of 10 should be shortened and the exponent increased.
    /// </summary>
    public static bool IsCanonical(BigDecimal value) =>
        value.Significand % 10 != 0;

    /// <summary>
    /// Check if the value is a complex number.
    /// </summary>
    public static bool IsComplexNumber(BigDecimal value) =>
        false;

    /// <summary>
    /// The value will be an integer if in canonical form and the exponent is >= 0.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsInteger(BigDecimal value) =>
        value.MakeCanonical().Exponent >= 0;

    /// <inheritdoc />
    public static bool IsOddInteger(BigDecimal value)
    {
        if (!IsInteger(value))
        {
            return false;
        }

        // If the exponent is > 0 then it's a multiple of 10, and therefore even.
        if (value.Exponent > 0)
        {
            return false;
        }

        // Check if it's odd.
        return value.Significand % 2 == 1;
    }

    /// <inheritdoc />
    public static bool IsEvenInteger(BigDecimal value)
    {
        if (!IsInteger(value))
        {
            return false;
        }

        // If the exponent is > 0 then it's a multiple of 10, and therefore even.
        if (value.Exponent > 0)
        {
            return true;
        }

        // Check if it's even.
        return value.Significand % 2 == 0;
    }

    /// <inheritdoc />
    public static bool IsZero(BigDecimal value) =>
        value.Significand == 0;

    /// <inheritdoc />
    public static bool IsNegative(BigDecimal value) =>
        value.Significand < 0;

    /// <inheritdoc />
    public static bool IsPositive(BigDecimal value) =>
        value.Significand > 0;

    /// <inheritdoc />
    public static bool IsFinite(BigDecimal value) =>
        true;

    /// <inheritdoc />
    public static bool IsInfinity(BigDecimal value) =>
        false;

    /// <inheritdoc />
    public static bool IsNegativeInfinity(BigDecimal value) =>
        false;

    /// <inheritdoc />
    public static bool IsPositiveInfinity(BigDecimal value) =>
        false;

    /// <inheritdoc />
    public static bool IsRealNumber(BigDecimal value) =>
        true;

    /// <inheritdoc />
    public static bool IsImaginaryNumber(BigDecimal value) =>
        false;

    /// <inheritdoc />
    public static bool IsNormal(BigDecimal value) =>
        true;

    /// <inheritdoc />
    public static bool IsSubnormal(BigDecimal value) =>
        false;

    /// <inheritdoc />
    public static bool IsNaN(BigDecimal value) =>
        false;

    #endregion Inspection methods

    #region Comparison methods

    public int CompareTo(object? obj) =>
        throw new NotImplementedException();

    public int CompareTo(BigDecimal other) =>
        throw new NotImplementedException();

    public bool Equals(BigDecimal other) =>
        Significand == other.Significand && Exponent == other.Exponent;

    public override bool Equals(object? obj)
    {
        if (obj is BigDecimal bd)
        {
            return Equals(bd);
        }
        return false;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Significand, Exponent);

    /// <inheritdoc />
    public static BigDecimal MaxMagnitude(BigDecimal x, BigDecimal y)
    {
        if (x.Exponent > y.Exponent)
        {
            return x;
        }
        if (x.Exponent < y.Exponent)
        {
            return y;
        }
        BigInteger absX = BigInteger.Abs(x.Significand);
        BigInteger absY = BigInteger.Abs(y.Significand);
        if (absX > absY)
        {
            return x;
        }
        return y;
    }

    /// <inheritdoc />
    public static BigDecimal MaxMagnitudeNumber(BigDecimal x, BigDecimal y) =>
        MaxMagnitude(x, y);

    /// <inheritdoc />
    public static BigDecimal MinMagnitude(BigDecimal x, BigDecimal y)
    {
        if (x.Exponent < y.Exponent)
        {
            return x;
        }
        if (x.Exponent > y.Exponent)
        {
            return y;
        }
        BigInteger absX = BigInteger.Abs(x.Significand);
        BigInteger absY = BigInteger.Abs(y.Significand);
        if (absX < absY)
        {
            return x;
        }
        return y;
    }

    /// <inheritdoc />
    public static BigDecimal MinMagnitudeNumber(BigDecimal x, BigDecimal y) =>
        MinMagnitude(x, y);

    #endregion Comparison methods

    #region Comparison operators

    public static bool operator ==(BigDecimal left, BigDecimal right) =>
        left.Equals(right);

    public static bool operator !=(BigDecimal left, BigDecimal right) =>
        !left.Equals(right);

    public static bool operator >(BigDecimal left, BigDecimal right) =>
        throw new NotImplementedException();

    public static bool operator >=(BigDecimal left, BigDecimal right) =>
        throw new NotImplementedException();

    public static bool operator <(BigDecimal left, BigDecimal right) =>
        throw new NotImplementedException();

    public static bool operator <=(BigDecimal left, BigDecimal right) =>
        throw new NotImplementedException();

    #endregion Comparison operators

    #region Methods for parsing and formatting

    /// <summary>
    /// Format the BigDecimal as a string.
    ///
    /// <todo>
    /// The goal is to support the usual standard numeric format strings:
    /// D, E, F, G, and N.
    /// </todo>
    ///
    /// The precision specifier is invalid for format "D".
    ///
    /// An additional code "U" is provided. This is essentially the same as "E" but uses "×10"
    /// instead of "E" and shows the exponent as superscript. Also, it doesn't use a + sign for
    /// positive exponents, or left-pad the exponent with 0s.
    /// </summary>
    /// <param name="specifier">The format specifier.</param>
    /// <param name="provider">The format provider.</param>
    /// <returns>The formatted string.</returns>
    /// <exception cref="ArgumentInvalidException">If the format specifier is invalid.</exception>
    public string ToString(string? specifier, IFormatProvider? provider)
    {
        // Set the default format parameters.
        string format = "D";
        int? precision = null;
        bool unicode = false;

        // Parse the format specifier.
        if (specifier != null)
        {
            var match = Regex.Match(specifier,
                @"^(?<format>[DEFGN])(?<precision>\d*)(?<unicode>U?)$", RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                throw new ArgumentInvalidException(nameof(specifier), "Invalid format specifier.");
            }

            format = match.Groups["format"].Value.ToUpper();
            precision = (match.Groups["precision"].Value == "")
                ? null
                : int.Parse(match.Groups["precision"].Value);
            unicode = match.Groups["unicode"].Value.ToUpper() == "U";
        }

        // Set the default format provider.
        if (provider == null)
        {
            provider = NumberFormatInfo.InvariantInfo;
        }

        // Check for special case, G, which will return the more compact of E and F.
        if (format == "G")
        {
            string strFormatE = ToString("E", provider);
            string strFormatF = ToString("F", provider);
            return (strFormatE.Length < strFormatF.Length) ? strFormatE : strFormatF;
        }

        // Get the NumberFormatInfo to use for special characters.
        NumberFormatInfo nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Format the significand.
        string strSignificand = "";
        string strAbsSignificand;
        string strSign;
        string strZeros;
        string strDecimalPart;
        BigInteger exponent = Exponent;
        switch (format)
        {
            case "D":
                strSignificand = Significand.ToString($"{format}{precision}", provider);
                break;

            case "E":
                strAbsSignificand = BigInteger.Abs(Significand).ToString();
                strSign = Significand < 0 ? nfi.NegativeSign : "";
                exponent = Exponent + strAbsSignificand.Length - 1;
                strSignificand = $"{strSign}{strAbsSignificand[..1]}{nfi.NumberDecimalSeparator}{strAbsSignificand[1..]}";
                break;

            case "F":
                if (Exponent == 0)
                {
                    strSignificand = Significand.ToString("D", provider);
                }
                else if (Exponent < 0)
                {
                    strAbsSignificand = BigInteger.Abs(Significand).ToString();
                    strSign = Significand < 0 ? nfi.NegativeSign : "";
                    strZeros = XString.Repeat("0", -Exponent);
                    strSignificand = $"{strZeros}{strAbsSignificand}";
                    strSignificand = $"{strSign}{strSignificand[..1]}{nfi.NumberDecimalSeparator}{strSignificand[1..]}";

                    // Get the decimal part.
                    int nPrecision = precision ?? nfi.NumberDecimalDigits;
                    strDecimalPart = strSignificand[1..];
                    if (strDecimalPart.Length < nPrecision)
                    {
                        strDecimalPart += XString.Repeat("0", nPrecision - strDecimalPart.Length);
                    }
                    else if (strDecimalPart.Length > nPrecision)
                    {
                        strDecimalPart = strDecimalPart[..(nPrecision + 1)];
                    }

                    exponent = 0;
                }
                else
                {
                    // Exponent > 0
                    strAbsSignificand = BigInteger.Abs(Significand).ToString();
                    strSign = Significand < 0 ? "-" : "";
                    strZeros = XString.Repeat("0", Exponent);
                    int nPrecision = precision ?? nfi.NumberDecimalDigits;
                    strDecimalPart = precision == 0
                        ? ""
                        : $"{nfi.NumberDecimalSeparator}{XString.Repeat("0", nPrecision)}";
                    strSignificand = $"{strSign}{strAbsSignificand}{strZeros}{strDecimalPart}";
                    exponent = 0;
                }
                break;

            case "N":
                strSignificand = Significand.ToString("N", provider);
                break;
        }

        // Format the exponent.
        // For now we just support format D (with precision unspecified), with or without the U
        // indicating if the exponent part should be displayed using unicode characters.
        string strExponent = "";
        if (exponent != 0)
        {
            strExponent = exponent.ToString(format == "N" ? "N" : "D", provider);
            strExponent = unicode
                ? $"×10{strExponent.ToSuperscript(1)}"
                : (exponent < 0) ? $"E{strExponent}" : $"E+{strExponent}";
        }

        return $"{strSignificand}{strExponent}";
    }

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static BigDecimal Parse(string s, IFormatProvider? provider) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static BigDecimal Parse(string s, NumberStyles style, IFormatProvider? provider) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static BigDecimal Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static BigDecimal Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static bool TryParse(string? s, IFormatProvider? provider, out BigDecimal result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider,
        out BigDecimal result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out BigDecimal result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider,
        out BigDecimal result) =>
        throw new NotImplementedException();

    #endregion Methods for parsing and formatting

    #region Conversion methods

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertFromChecked<TOther>(TOther value, out BigDecimal result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertFromSaturating<TOther>(TOther value, out BigDecimal result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertFromTruncating<TOther>(TOther value, out BigDecimal result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertToChecked<TOther>(BigDecimal value, out TOther result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertToSaturating<TOther>(BigDecimal value, out TOther result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertToTruncating<TOther>(BigDecimal value, out TOther result) =>
        throw new NotImplementedException();

    #endregion Conversion methods
}
