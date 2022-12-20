using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using AstroMultimedia.Core.Exceptions;
using AstroMultimedia.Core.Strings;
using AstroMultimedia.Core.Numbers;

namespace AstroMultimedia.Numerics.Types;

public partial struct BigDecimal : IFloatingPoint<BigDecimal>, ICloneable, IPowerFunctions<BigDecimal>,
    IRootFunctions<BigDecimal>, IExponentialFunctions<BigDecimal>, ILogarithmicFunctions<BigDecimal>
{
    #region Constructors

    public BigDecimal(BigInteger significand, int exponent = 0)
    {
        (significand, exponent) = MakeCanonical(significand, exponent);
        Significand = significand;
        Exponent = exponent;
    }

    public BigDecimal() : this(0)
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

    /// <summary>The power of 10.</summary>
    public int Exponent { get; set; }

    #endregion Instance properties

    #region Static properties

    /// <inheritdoc />
    public static BigDecimal Zero { get; } = new (0);

    /// <inheritdoc />
    public static BigDecimal One { get; } = new (1);

    /// <inheritdoc />
    public static BigDecimal NegativeOne { get; } = new (-1);

    /// <inheritdoc />
    public static int Radix => 10;

    /// <inheritdoc />
    public static BigDecimal AdditiveIdentity => Zero;

    /// <inheritdoc />
    public static BigDecimal MultiplicativeIdentity => One;

    #endregion Static properties

    #region Constants

    /// <inheritdoc />
    /// <remarks>
    /// Euler's number (e) to 100 decimal places.
    /// If you need more, you can get up to 10,000 decimal places here:
    /// <see href="https://www.math.utah.edu/~pa/math/e" />
    /// </remarks>
    public static BigDecimal E { get; } = Parse("2."
        + "7182818284 5904523536 0287471352 6624977572 4709369995 "
        + "9574966967 6277240766 3035354759 4571382178 5251664274", null);

    /// <inheritdoc />
    /// <remarks>
    /// The circle constant (π) to 100 decimal places.
    /// If you need more, you can get up to 10,000 decimal places here:
    /// <see href="https://www.math.utah.edu/~pa/math/pi" />
    /// </remarks>
    public static BigDecimal Pi { get; } = Parse("3."
        + "1415926535 8979323846 2643383279 5028841971 6939937510 "
        + "5820974944 5923078164 0628620899 8628034825 3421170679", null);

    /// <inheritdoc />
    /// <remarks>
    /// The other circle constant (τ = 2π) to 100 decimal places.
    /// If you need more, you can get up to 10,000 decimal places here:
    /// <see href="https://tauday.com/tau-digits" />
    /// </remarks>
    public static BigDecimal Tau { get; } = Parse("6."
        + "2831853071 7958647692 5286766559 0057683943 3879875021 "
        + "1641949889 1846156328 1257241799 7256069650 6842341360", null);

    /// <summary>
    /// The golden ratio to 100 decimal places.
    /// </summary>
    public static BigDecimal Phi { get; } = Parse("1."
        + "6180339887 4989484820 4586834365 6381177203 0917980576 "
        + "2862135448 6227052604 6281890244 9707207204 1893911374", null);

    /// <summary>
    /// The square root of 2 to 100 decimal places.
    /// </summary>
    public static BigDecimal Sqrt2 { get; } = Parse("1."
        + "4142135623 7309504880 1688724209 6980785696 7187537694 "
        + "8073176679 7379907324 7846210703 8850387534 3276415727", null);

    /// <summary>
    /// The square root of 10 to 100 decimal places.
    /// </summary>
    public static BigDecimal Sqrt10 { get; } = Parse("3."
        + "1622776601 6837933199 8893544432 7185337195 5513932521 "
        + "6826857504 8527925944 3863923822 1344248108 3793002952", null);

    /// <summary>
    /// The natural logarithm of 2 to 100 decimal places.
    /// </summary>
    public static BigDecimal Ln2 { get; } = Parse("0."
        + "6931471805 5994530941 7232121458 1765680755 0013436025 "
        + "5254120680 0094933936 2196969471 5605863326 9964186875", null);

    /// <summary>
    /// The natural logarithm of 10 to 100 decimal places.
    /// </summary>
    public static BigDecimal Ln10 { get; } = Parse("2."
        + "3025850929 9404568401 7991454684 3642076011 0148862877 "
        + "2976033327 9009675726 0967735248 0235997205 0895982983", null);

    #endregion Constants

    #region Cast operators

    public static implicit operator BigDecimal(sbyte n) =>
        new (n);

    public static implicit operator BigDecimal(byte n) =>
        new (n);

    public static implicit operator BigDecimal(short n) =>
        new (n);

    public static implicit operator BigDecimal(ushort n) =>
        new (n);

    public static implicit operator BigDecimal(int n) =>
        new (n);

    public static implicit operator BigDecimal(uint n) =>
        new (n);

    public static implicit operator BigDecimal(long n) =>
        new (n);

    public static implicit operator BigDecimal(ulong n) =>
        new (n);

    public static implicit operator BigDecimal(Int128 n) =>
        new (n);

    public static implicit operator BigDecimal(UInt128 n) =>
        new (n);

    public static implicit operator BigDecimal(BigInteger n) =>
        new (n);

    public static implicit operator BigDecimal(float n) =>
        Parse(n.ToString("G", NumberFormatInfo.InvariantInfo));

    public static implicit operator BigDecimal(double n) =>
        Parse(n.ToString("G", NumberFormatInfo.InvariantInfo));

    public static implicit operator BigDecimal(decimal n) =>
        Parse(n.ToString("G", NumberFormatInfo.InvariantInfo));

    #endregion Cast operators

    #region Miscellaneous methods

    /// <inheritdoc />
    public object Clone() =>
        (BigDecimal)MemberwiseClone();

    /// <inheritdoc />
    public int GetSignificandByteCount() =>
        Significand.GetByteCount();

    /// <inheritdoc />
    public int GetSignificandBitLength() =>
        GetSignificandByteCount() * 8;

    /// <inheritdoc />
    public int GetExponentByteCount() =>
        4;

    /// <inheritdoc />
    public int GetExponentShortestBitLength() =>
        32;

    /// <inheritdoc />
    public bool TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten) =>
        TryWriteBigInteger(Significand, destination, out bytesWritten, true);

    /// <inheritdoc />
    public bool TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten) =>
        TryWriteBigInteger(Significand, destination, out bytesWritten, false);

    /// <inheritdoc />
    public bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten) =>
        TryWriteInt(Exponent, destination, out bytesWritten, true);

    /// <inheritdoc />
    public bool TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten) =>
        TryWriteInt(Exponent, destination, out bytesWritten, false);

    #endregion Miscellaneous methods

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

        // If the exponent is 0, check if it's odd.
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

        // If the exponent is 0, check if it's even.
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

    /// <inheritdoc />
    public int CompareTo(BigDecimal other)
    {
        int compareExponents = Exponent.CompareTo(other.Exponent);
        return compareExponents == 0 ? Significand.CompareTo(other.Significand) : compareExponents;
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is not BigDecimal other)
        {
            throw new ArgumentInvalidException(nameof(obj), "Must be a BigDecimal.");
        }
        return CompareTo(other);
    }

    /// <inheritdoc />
    public bool Equals(BigDecimal other) =>
        Significand == other.Significand && Exponent == other.Exponent;

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is BigDecimal bd)
        {
            return Equals(bd);
        }
        return false;
    }

    /// <inheritdoc />
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
        return absX > absY ? x : y;
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
        return absX < absY ? x : y;
    }

    /// <inheritdoc />
    public static BigDecimal MinMagnitudeNumber(BigDecimal x, BigDecimal y) =>
        MinMagnitude(x, y);

    #endregion Comparison methods

    #region Comparison operators

    /// <inheritdoc />
    public static bool operator ==(BigDecimal left, BigDecimal right) =>
        left.Equals(right);

    /// <inheritdoc />
    public static bool operator !=(BigDecimal left, BigDecimal right) =>
        !left.Equals(right);

    /// <inheritdoc />
    public static bool operator <(BigDecimal left, BigDecimal right) =>
        left.CompareTo(right) < 0;

    /// <inheritdoc />
    public static bool operator <=(BigDecimal left, BigDecimal right) =>
        left.CompareTo(right) <= 0;

    /// <inheritdoc />
    public static bool operator >(BigDecimal left, BigDecimal right) =>
        left.CompareTo(right) > 0;

    /// <inheritdoc />
    public static bool operator >=(BigDecimal left, BigDecimal right) =>
        left.CompareTo(right) >= 0;

    #endregion Comparison operators

    #region Methods for parsing and formatting

    /// <summary>
    /// Format the BigDecimal as a string.
    ///
    /// Supported formats are the usual: D, E, F, G, N, P, and R.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings" />
    ///
    /// Although "D" is normally only used by integral types, in this case both the significand and
    /// exponent will be formatted as integers.
    ///
    /// An secondary code "U" is provided.
    ///   - If omitted, the exponent (if present) will be formatted with the usual E[-+]999 format.
    ///   - If present, the exponent is formatted with "×10" instead of "E" and the exponent digits
    ///     will be rendered as superscript. Also, a "+" sign is not used for positive exponents.
    ///
    /// In either case, unlike with "E" and "G" when used with float, double, and decimal, exponent
    /// digits are not left-padded with 0s. I don't see this as necessary.
    ///
    /// Codes "R" and "D" will produce the same output. However, the Unicode flag is undefined with
    /// "R", because Parse() doesn't support superscript exponents.
    /// </summary>
    /// <param name="specifier">The format specifier (default "G").</param>
    /// <param name="provider">The format provider (default null).</param>
    /// <returns>The formatted string.</returns>
    /// <exception cref="ArgumentInvalidException">If the format specifier is invalid.</exception>
    public string ToString(string? specifier = "E", IFormatProvider? provider = null)
    {
        // Set defaults.
        string format = "G";
        NumberFormatInfo nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;
        int? precision = null;
        bool unicode = false;

        // Parse the format specifier.
        if (specifier != null)
        {
            Match match = Regex.Match(specifier,
                @"^(?<format>[DEFGNPR])(?<precision>\d*)(?<unicode>U?)$", RegexOptions.IgnoreCase);

            // Check format is valid.
            if (!match.Success)
            {
                throw new ArgumentInvalidException(nameof(specifier),
                    $"Invalid format specifier \"{specifier}\".");
            }

            // Extract parts.
            format = match.Groups["format"].Value.ToUpper();
            string strPrecision = match.Groups["precision"].Value;
            precision = strPrecision == "" ? null : int.Parse(strPrecision);
            unicode = match.Groups["unicode"].Value.ToUpper() == "U";

            // Check if Unicode flag is present when it shouldn't be.
            if (unicode && format is "F" or "N" or "P" or "R")
            {
                throw new ArgumentInvalidException(nameof(specifier),
                    $"The Unicode flag is invalid with format \"{format}\".");
            }
        }

        // Format the significand.
        string strSig = "";
        int exp = Exponent;
        switch (format)
        {
            case "D" or "R":
                strSig = Significand.ToString($"D", provider);
                break;

            case "E":
                string strAbsSig = BigInteger.Abs(Significand).ToString();
                BigDecimal sig = new (Significand, -strAbsSig.Length + 1);
                strSig = sig.FormatFixedPoint(format, precision, provider);
                exp = Exponent - sig.Exponent;
                break;

            case "F" or "N":
                return FormatFixedPoint(format, precision, provider);

            case "G":
            {
                // Return the more compact of E and F.
                string strUnicode = unicode ? "U" : "";
                string strFormatE = ToString($"E{precision}{strUnicode}", provider);
                string strFormatF = FormatFixedPoint("F", precision, provider);
                return (strFormatE.Length < strFormatF.Length) ? strFormatE : strFormatF;
            }

            case "P":
                return (this * 100).FormatFixedPoint("F", precision, provider) + "%";
        }

        // Format the exponent.
        string strExp = "";
        if (format == "E" || format is "D" or "N" or "R" && exp != 0)
        {
            strExp = XUint.Abs(exp).ToString(format == "N" ? "N" : "D", provider);
            string strSign;
            if (unicode)
            {
                strSign = exp < 0 ? nfi.NegativeSign : "";
                strExp = "×10" + $"{strSign}{strExp}".ToSuperscript(1);
            }
            else
            {
                strSign = exp < 0 ? nfi.NegativeSign : nfi.PositiveSign;
                strExp = $"E{strSign}{strExp}";
            }
        }

        return $"{strSig}{strExp}";
    }

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        string formattedValue = ToString(new string(format), provider);
        try
        {
            formattedValue.CopyTo(destination);
            charsWritten = formattedValue.Length;
            return true;
        }
        catch
        {
            charsWritten = 0;
            return false;
        }
    }

    /// <inheritdoc />
    public static BigDecimal Parse(string s, IFormatProvider? provider)
    {
        // Get a NumberFormatInfo object so we know what characters to look for.
        NumberFormatInfo nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Remove any whitespace, underscore, or group separator characters from the string.
        s = Regex.Replace(s, $@"[\s_{nfi.NumberGroupSeparator}]", "");

        // Check the string format and extract salient info.
        string strSign = $"[{nfi.NegativeSign}{nfi.PositiveSign}]?";
        Match match = Regex.Match(s,
            $@"^(?<integer>{strSign}\d+)({nfi.NumberDecimalSeparator}(?<fraction>\d+))?(e(?<exponent>{strSign}\d+))?$",
            RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            throw new ArgumentFormatException(nameof(s), "Invalid format.");
        }

        // Get the parts.
        string strInteger = match.Groups["integer"].Value;
        string strFraction = match.Groups["fraction"].Value;
        string strExponent = match.Groups["exponent"].Value;

        // Construct the result object.
        BigInteger significand = BigInteger.Parse(strInteger + strFraction, provider);
        int exponent = strExponent == "" ? 0 : int.Parse(strExponent, provider);
        exponent -= strFraction.Length;
        return new BigDecimal(significand, exponent);
    }

    /// <summary>
    /// More convenient version of Parse().
    /// </summary>
    public static BigDecimal Parse(string s) =>
        Parse(s, NumberFormatInfo.InvariantInfo);

    /// <inheritdoc />
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigDecimal Parse(string s, NumberStyles style, IFormatProvider? provider) =>
        Parse(s, provider);

    /// <inheritdoc />
    public static BigDecimal Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
        Parse(new string(s), provider);

    /// <inheritdoc />
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigDecimal Parse(ReadOnlySpan<char> s, NumberStyles style,
        IFormatProvider? provider) =>
        Parse(new string(s), provider);

    /// <inheritdoc />
    public static bool TryParse(string? s, IFormatProvider? provider, out BigDecimal result)
    {
        if (s == null)
        {
            result = Zero;
            return false;
        }

        try
        {
            result = Parse(s, provider);
            return true;
        }
        catch (Exception)
        {
            result = Zero;
            return false;
        }
    }

    /// <summary>
    /// More convenient version of TryParse().
    /// </summary>
    public static bool TryParse(string? s, out BigDecimal result) =>
        TryParse(s, NumberFormatInfo.InvariantInfo, out result);

    /// <inheritdoc />
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider,
        out BigDecimal result) =>
        TryParse(s, provider, out result);

    /// <inheritdoc />
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider,
        out BigDecimal result) =>
        TryParse(new string(s), provider, out result);

    /// <inheritdoc />
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider,
        out BigDecimal result) =>
        TryParse(new string(s), provider, out result);

    #endregion Methods for parsing and formatting

    #region Conversion methods

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertFromChecked<TOther>(TOther value,
        out BigDecimal result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertFromSaturating<TOther>(TOther value,
        out BigDecimal result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertFromTruncating<TOther>(TOther value,
        out BigDecimal result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertToChecked<TOther>(BigDecimal value,
        out TOther result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertToSaturating<TOther>(BigDecimal value,
        out TOther result) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    static bool INumberBase<BigDecimal>.TryConvertToTruncating<TOther>(BigDecimal value,
        out TOther result) =>
        throw new NotImplementedException();

    #endregion Conversion methods

    #region Private methods

    /// <summary>
    /// Multiply the significand by 10 and decrement the exponent to maintain the same value,
    /// <paramref name="nPlaces"/> times.
    /// </summary>
    /// <param name="nPlaces"></param>
    private void Shift(int nPlaces)
    {
        Significand *= BigInteger.Pow(10, nPlaces);
        Exponent -= nPlaces;
    }

    /// <summary>
    /// Adjust the parts of one of the values so both have the same exponent.
    /// Two new objects will be returned.
    /// </summary>
    private static (BigDecimal, BigDecimal) Align(BigDecimal x, BigDecimal y)
    {
        // See if there's anything to do.
        if (x.Exponent == y.Exponent)
        {
            return (x, y);
        }

        // Get a and b as proxies for the operation so we don't mutate the original values.
        BigDecimal a = x;
        BigDecimal b = y;

        // Make sure b has the larger exponent.
        if (a.Exponent > b.Exponent)
        {
            (a, b) = (b, a);
        }

        // Shift b so the exponents are the same.
        b.Shift(b.Exponent - a.Exponent);

        return (a, b);
    }

    /// <summary>
    /// Modify the provided significand and exponent as needed to find the canonical form.
    /// Static form of the method, for use in the constructor.
    /// </summary>
    /// <returns>The two updated BigIntegers.</returns>
    private static (BigInteger, int) MakeCanonical(BigInteger significand, int exponent)
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
    /// Shared logic for:
    /// <see cref="TryWriteBigInteger" />
    /// <see cref="TryWriteInt" />
    /// </summary>
    private static bool TryWrite(byte[] bytes, Span<byte> destination, out int bytesWritten)
    {
        try
        {
            bytes.CopyTo(destination);
            bytesWritten = bytes.Length;
            return true;
        }
        catch
        {
            bytesWritten = 0;
            return false;
        }
    }

    /// <summary>
    /// Shared logic for:
    /// <see cref="TryWriteSignificandBigEndian" />
    /// <see cref="TryWriteSignificandLittleEndian" />
    /// </summary>
    private static bool TryWriteBigInteger(BigInteger bi, Span<byte> destination,
        out int bytesWritten,
        bool isBigEndian)
    {
        byte[] bytes = bi.ToByteArray(false, isBigEndian);
        return TryWrite(bytes, destination, out bytesWritten);
    }

    /// <summary>
    /// Shared logic for:
    /// <see cref="TryWriteExponentBigEndian" />
    /// <see cref="TryWriteExponentLittleEndian" />
    /// </summary>
    private static bool TryWriteInt(int i, Span<byte> destination, out int bytesWritten,
        bool isBigEndian)
    {
        // Get the bytes.
        byte[] bytes = BitConverter.GetBytes(i);

        // Check if the requested endianness matches the architecture. If not, reverse the array.
        if (BitConverter.IsLittleEndian && isBigEndian
            || !BitConverter.IsLittleEndian && !isBigEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }

        return TryWrite(bytes, destination, out bytesWritten);
    }

    /// <summary>
    /// From a BigDecimal, extract two strings of digits that would appear if the number was written
    /// in fixed-point format, i.e. without an exponent. Sign is ignored.
    /// </summary>
    /// <returns></returns>
    public (string digitsBefore, string digitsAfter) GetDigitStrings()
    {
        string digits = BigInteger.Abs(Significand).ToString();

        if (Exponent == 0)
        {
            return (digits, "");
        }

        if (Exponent > 0)
        {
            return (digits + "0".Repeat(Exponent), "");
        }

        if (-Exponent == digits.Length)
        {
            return ("0", digits);
        }

        if (-Exponent > digits.Length)
        {
            return ("0", "0".Repeat(-Exponent - digits.Length) + digits);
        }

        return (digits[..^(-Exponent)], digits[^(-Exponent)..]);
    }

    public string FormatFixedPoint(string format, int? precision, IFormatProvider? provider = null)
    {
        // Get a NumberFormatInfo we can use for special characters.
        NumberFormatInfo nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Get the parts of the string.
        BigDecimal bd = precision.HasValue ? Round(this, precision.Value) : this;
        string strSign = bd.Significand < 0 ? nfi.NegativeSign : "";
        (string strBefore, string strAfter) = bd.GetDigitStrings();

        // Add group separators if necessary.
        if (format == "N")
        {
            strBefore = BigInteger.Parse(strBefore).ToString("N", provider);
        }

        // Format the fractional part.
        if (strAfter != "" || precision is > 0)
        {
            // Add trailing 0s only if the precision was specified.
            if (precision != null && precision.Value > strAfter.Length)
            {
                strAfter += "0".Repeat(precision.Value - strAfter.Length);
            }
            strAfter = nfi.NumberDecimalSeparator + strAfter;
        }

        return $"{strSign}{strBefore}{strAfter}";
    }

    #endregion Private methods
}
