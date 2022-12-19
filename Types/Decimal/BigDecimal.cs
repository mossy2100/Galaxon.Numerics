using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using AstroMultimedia.Core.Exceptions;
using AstroMultimedia.Core.Strings;

namespace AstroMultimedia.Numerics.Types;

public struct BigDecimal : IFloatingPoint<BigDecimal>, ICloneable, IPowerFunctions<BigDecimal>,
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

    #region Arithmetic operators

    /// <inheritdoc />
    public static BigDecimal operator +(BigDecimal value) =>
        (BigDecimal)value.MemberwiseClone();

    /// <inheritdoc />
    public static BigDecimal operator +(BigDecimal left, BigDecimal right)
    {
        (BigDecimal x, BigDecimal y) = Align(left, right);
        BigDecimal result = new (x.Significand + y.Significand, x.Exponent);
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

    /// <inheritdoc />
    /// <remarks>
    /// Computes the division using the Goldschmidt algorithm.
    /// <see href="https://en.wikipedia.org/wiki/Division_algorithm#Goldschmidt_division" />
    /// </remarks>
    public static BigDecimal operator /(BigDecimal left, BigDecimal right)
    {
        // Guard.
        if (right == Zero)
        {
            throw new DivideByZeroException("Division by 0 is undefined.");
        }

        // Optimizations.
        if (right == One)
        {
            return left;
        }
        if (left == right)
        {
            return One;
        }

        // Get the operands with the exponents removed. This will enable us to calculate f using the
        // decimal type.
        BigDecimal a = new (left.Significand);
        BigDecimal b = new (right.Significand);

        // Find f, a good initial estimate of the multiplication factor.
        string strRightSig = right.Significand.ToString();
        decimal fSigInv = decimal.Parse(strRightSig.Length <= 28 ? strRightSig : strRightSig[..28]);
        int fExp = strRightSig.Length <= 28 ? 0 : 28 - strRightSig.Length;
        BigDecimal f = 1m / fSigInv * new BigDecimal(1, fExp);

        // Get 2 as a BigDecimal to avoid doing the conversion every time.
        BigDecimal two = 2;

        while (true)
        {
            a *= f;
            b *= f;

            // If the right is 1, then n is the result.
            if (b == One)
            {
                break;
            }

            f = two - b;

            // If d is not 1, but is close to 1, then f can be 1 due to rounding after the
            // subtraction. If it is, there's no point continuing.
            if (f == One)
            {
                break;
            }
        }

        return new BigDecimal(a.Significand, a.Exponent + left.Exponent - right.Exponent);
    }

    /// <inheritdoc />
    public static BigDecimal operator %(BigDecimal left, BigDecimal right)
    {
        BigDecimal div = Round(left / right, 0, MidpointRounding.ToZero);
        return left - (div * right);
    }

    /// <summary>
    /// Exponentiation operator.
    /// </summary>
    public static BigDecimal operator ^(BigDecimal left, BigDecimal right) =>
        Pow(left, right);

    #endregion Arithmetic operators

    #region Miscellaneous methods

    /// <inheritdoc />
    public object Clone() =>
        (BigDecimal)MemberwiseClone();

    /// <inheritdoc />
    public static BigDecimal Abs(BigDecimal value) =>
        new (BigInteger.Abs(value.Significand), value.Exponent);

    /// <inheritdoc />
    public static BigDecimal Round(BigDecimal x, int digits = 0,
        MidpointRounding mode = MidpointRounding.ToEven)
    {
        // If it's an integer, no rounding required.
        if (x.Exponent >= 0)
        {
            return x;
        }

        // Find out how many digits to discard.
        int nDigitsToCut = -digits - x.Exponent;
        BigInteger newSig;

        if (nDigitsToCut > 0)
        {
            // Separate the digits to keep from the ones to discard.
            int sign = x.Significand.Sign;
            BigInteger absSig = BigInteger.Abs(x.Significand);
            string strAbsSig = absSig.ToString();
            string strLeft = strAbsSig[..^nDigitsToCut];
            string strRight = strAbsSig[^nDigitsToCut..];
            BigInteger newAbsSig = strLeft == "" ? 0 : BigInteger.Parse(strLeft);

            // Round off according to mode.
            bool increment = false;
            switch (mode)
            {
                case MidpointRounding.ToEven:
                    increment = strRight[0] >= '5' && (strRight != "5" || newAbsSig % 2 == 1);
                    break;

                case MidpointRounding.AwayFromZero:
                    increment = strRight[0] >= '5';
                    break;

                case MidpointRounding.ToZero:
                    increment = false;
                    break;

                case MidpointRounding.ToNegativeInfinity:
                    increment = sign < 0;
                    break;

                case MidpointRounding.ToPositiveInfinity:
                    increment = sign > 0;
                    break;
            }

            // Adjust the significand.
            if (increment)
            {
                newAbsSig++;
            }
            newSig = sign * newAbsSig;
        }
        else
        {
            // No rounding necessary.
            newSig = x.Significand;

            // Add 0s if needed.
            if (nDigitsToCut < 0)
            {
                newSig *= BigInteger.Pow(10, -nDigitsToCut);
            }
        }

        return new BigDecimal(newSig, -digits);
    }

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
            Match match = Regex.Match(specifier,
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
        int exponent = Exponent;
        switch (format)
        {
            case "D":
                strSignificand = Significand.ToString($"{format}{precision}", provider);
                break;

            case "E":
                strAbsSignificand = BigInteger.Abs(Significand).ToString();
                strSign = Significand < 0 ? nfi.NegativeSign : "";
                exponent = Exponent + strAbsSignificand.Length - 1;
                strSignificand =
                    $"{strSign}{strAbsSignificand[..1]}{nfi.NumberDecimalSeparator}{strAbsSignificand[1..]}";
                break;

            case "F":
                if (Exponent == 0)
                {
                    strSignificand = Significand.ToString("D", provider);
                }
                else
                {
                    string strZeros;
                    string strDecimalPart;
                    if (Exponent < 0)
                    {
                        strAbsSignificand = BigInteger.Abs(Significand).ToString();
                        strSign = Significand < 0 ? nfi.NegativeSign : "";
                        strZeros = XString.Repeat("0", -Exponent);
                        strSignificand = $"{strZeros}{strAbsSignificand}";
                        strSignificand =
                            $"{strSign}{strSignificand[..1]}{nfi.NumberDecimalSeparator}{strSignificand[1..]}";

                        // Get the decimal part.
                        int nPrecision = precision ?? nfi.NumberDecimalDigits;
                        strDecimalPart = strSignificand[1..];
                        if (strDecimalPart.Length < nPrecision)
                        {
                            strDecimalPart +=
                                XString.Repeat("0", nPrecision - strDecimalPart.Length);
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
                : (exponent < 0)
                    ? $"E{strExponent}"
                    : $"E{nfi.PositiveSign}{strExponent}";
        }

        return $"{strSignificand}{strExponent}";
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

    #region Exponentiation and logarithm methods

    public static BigDecimal Pow(BigDecimal x, BigDecimal y) =>
        throw new NotImplementedException();

    public static BigDecimal Sqrt(BigDecimal x) =>
        throw new NotImplementedException();

    public static BigDecimal Cbrt(BigDecimal x) =>
        throw new NotImplementedException();

    public static BigDecimal Hypot(BigDecimal x, BigDecimal y) =>
        throw new NotImplementedException();

    public static BigDecimal RootN(BigDecimal x, int n) =>
        throw new NotImplementedException();

    public static BigDecimal Exp(BigDecimal x) =>
        throw new NotImplementedException();

    public static BigDecimal Exp2(BigDecimal x) =>
        throw new NotImplementedException();

    public static BigDecimal Exp10(BigDecimal x) =>
        throw new NotImplementedException();

    public static BigDecimal Log(BigDecimal x) =>
        throw new NotImplementedException();

    public static BigDecimal Log(BigDecimal x, BigDecimal newBase) =>
        throw new NotImplementedException();

    public static BigDecimal Log2(BigDecimal x) =>
        throw new NotImplementedException();

    public static BigDecimal Log10(BigDecimal x) =>
        throw new NotImplementedException();

    #endregion Exponentiation and logarithm methods

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

    #endregion Private methods
}
