using System.Text;
using System.Text.RegularExpressions;
using AstroMultimedia.Core.Exceptions;
using AstroMultimedia.Core.Numbers;
using DecimalMath;

namespace AstroMultimedia.Numerics.Types;

/// <summary>
/// Software implementation of large decimal floating point value.
/// In this file:
/// - "B" means 1 billion.
/// - "billesimal" means base B.
/// - A "billit" is a "billesimal digit".
/// The number has two parts: a significand and a mantissa.
/// The maximum total size in memory is 60 bytes.
/// Significand: pointer (8 bytes) plus up to 12 ints (48 bytes)
/// Mantissa: 1 int (4 bytes)
/// Significand
/// ===========
/// The maximum size of the significand is 12 ints or 48 bytes,
/// which can encode up to 108 decimal digits.
/// Each int encodes 1 billit, equal to 9 digits, or a range of 0..999,999,999
/// The int at index 0 in the array thus encodes the first 9 digits of the
/// decimal significand, the int at index 1 encodes the second 9 digits, etc.
/// Each int in the significand also includes the sign. The overall sign of the
/// number will be the same for each billit in the significand.
/// Mantissa
/// ========
/// 4 bytes (1 int). Note, this is not a power of 10 but a power of B.
/// The decimal mantissa equals the value stored in the byte (the
/// billesimal mantissa) * 9.
/// The billesimal exponent range is -2,147,483,648 to 2,147,483,647, which
/// corresponds to a decimal exponent range of -19,327,352,832 to 19,327,352,823
/// Each number conceptually represents a value in scientific notation, i.e.
/// having the form [-]9.999.. * 10^[-]999.. (or [-]9.999..e[-]9.999..)
/// except the base is billesimal, i.e. the value is found from
/// S[0] * B^M + S[1] * B^(M - 1) + S[2] * B^(M - 2)...
/// where S is the significand, B is 10^9, and M is the mantissa.
/// </summary>
public class BigDecimal : IEquatable<BigDecimal>, IComparable<BigDecimal>
{
    #region Constants

    private const int _B = 1_000_000_000;

    private const int _DIGITS_PER_BILLIT = 9;

    private const byte _MAX_LENGTH = 12;

    #endregion Constants

    #region Properties

    public int[] Significand { get; init; }

    public int Mantissa { get; init; }

    public int Sign => GetSign(Significand);

    public bool IsZero => Length == 0;

    public bool IsOne => Length == 1 && Significand[0] == 1 && Mantissa == 0;

    public bool IsNegative => Sign == -1;

    public bool IsPositive => Sign == 1;

    public int Length => Significand.Length;

    #endregion Properties

    #region Constructors and creation methods

    // /// <summary>
    // /// Protected constructor.
    // /// Constructs a new object with a preset number of billits.
    // /// Used for preparing result objects for calculations.
    // /// </summary>
    // /// <param name="length"></param>
    // /// <param name="mantissa"></param>
    // protected BigDecimal(int length, int mantissa = 0)
    // {
    //     Significand = new int[length];
    //     Mantissa = mantissa;
    // }

    /// <summary>
    /// Construct from properties.
    /// </summary>
    public BigDecimal(int[]? significand = null, int mantissa = 0)
    {
        if (significand == null)
        {
            Significand = Array.Empty<int>();
        }
        else
        {
            significand = TrimLeadingZeros(significand, ref mantissa);
            significand = Round(significand, ref mantissa);
            Significand = TrimTrailingZeros(significand, ref mantissa);
        }

        Mantissa = mantissa;
    }

    /// <summary>
    /// Construct from string.
    /// </summary>
    public BigDecimal(string s)
    {
        // Default to 0.
        if (string.IsNullOrWhiteSpace(s) || s == "0")
        {
            Significand = Array.Empty<int>();
            Mantissa = 0;
            return;
        }

        Regex rx = new(@"^(-?)(\d+)(\.(\d+))?(e([-+]?\d+))?$", RegexOptions.IgnoreCase);
        MatchCollection matches = rx.Matches(s);

        // Check we got a match.
        if (matches.Count != 1)
        {
            throw new ArgumentFormatException(nameof(s),
                "Invalid string format for long decimal value.");
        }

        GroupCollection groups = matches[0].Groups;

        // Get the sign.
        string strSign = groups[1].Value;
        int sign = strSign == "-" ? -1 : 1;

        // Get the digits.
        string strDigitsBeforeDecimal = groups[2].Value;
        string strDigitsAfterDecimal = groups[4].Value;
        string strDigits = strDigitsBeforeDecimal + strDigitsAfterDecimal;

        // Get the decimal mantissa.
        string strMantissa = groups[6].Value;
        int decMantissa = (strMantissa == "" ? 0 : int.Parse(strMantissa))
            + strDigitsBeforeDecimal.Length - 1;

        // Trim zeros and adjust mantissa as needed.
        strDigits = TrimDigits(strDigits, ref decMantissa);

        // Calculate number of decimal places to shift.
        int bilMantissa = (int)Floor(decMantissa / (decimal)_DIGITS_PER_BILLIT);
        int nShift = decMantissa - bilMantissa * _DIGITS_PER_BILLIT;
        int nLeadingZeros = _DIGITS_PER_BILLIT - 1 - nShift;
        int nDigitsWithLeadingZeros = nLeadingZeros + strDigits.Length;
        int nBillits = (int)Ceiling(nDigitsWithLeadingZeros / (decimal)_DIGITS_PER_BILLIT);
        int nTrailingZeros = nBillits * _DIGITS_PER_BILLIT - nDigitsWithLeadingZeros;
        strDigits += new string('0', nTrailingZeros);

        // Create an array to store the significand.
        // int sigLen = strDigits.Length / DigitsPerBillit;
        int[] significand = new int[nBillits];
        for (int i = 0; i < nBillits; i++)
        {
            int start = i * _DIGITS_PER_BILLIT - nLeadingZeros;
            int end = start + _DIGITS_PER_BILLIT;
            if (start < 0)
            {
                start = 0;
            }
            significand[i] = sign * int.Parse(strDigits[start..end]);
        }

        Significand = Round(significand, ref bilMantissa);
        Mantissa = bilMantissa;
    }

    public static BigDecimal Clone(BigDecimal bd) => new(bd.Significand, bd.Mantissa);

    public static BigDecimal Zero() => new();

    public static BigDecimal One() => new(new[] { 1 });

    public static BigDecimal MinValue() =>
        new(new[]
        {
            -999999999, -999999999, -999999999, -999999999, -999999999,
            -999999999, -999999999, -999999999, -999999999, -999999999,
            -999999999, -999999999
        }, int.MaxValue);

    /// <summary>
    /// Minimum (largest negative) value.
    /// </summary>
    /// <returns></returns>
    public static BigDecimal MaxValue() =>
        new(new[]
        {
            999999999, 999999999, 999999999, 999999999, 999999999,
            999999999, 999999999, 999999999, 999999999, 999999999,
            999999999, 999999999
        }, int.MaxValue);

    /// <summary>
    /// Smallest positive value. B^-(2^31)
    /// </summary>
    /// <returns></returns>
    public static BigDecimal Epsilon() => new(new[] { 1 }, int.MinValue);

    #endregion Constructors and creation methods

    #region Overridden methods

    public override bool Equals(object? obj) => obj is BigDecimal bd && Equals(bd);

    public override int GetHashCode() => HashCode.Combine(Significand, Mantissa);

    public override string ToString()
    {
        // Optimization.
        if (Length == 0)
        {
            return "0";
        }

        // Sign.
        string strSign = IsNegative ? "-" : "";

        // Decimal mantissa.
        int decMantissa = Mantissa * _DIGITS_PER_BILLIT + (_DIGITS_PER_BILLIT - 1);

        // Decimal significand.
        StringBuilder sbSignificand = new();
        foreach (int billit in Significand)
        {
            sbSignificand.Append(Math.Abs(billit).ToString().PadLeft(_DIGITS_PER_BILLIT, '0'));
        }
        string strSignificand = TrimDigits(sbSignificand.ToString(), ref decMantissa);
        if (strSignificand.Length > 1)
        {
            strSignificand = $"{strSignificand[0]}.{strSignificand[1..]}";
        }

        // Create a substring for the mantissa.
        string strMantissa = decMantissa switch
        {
            < 0 => $"E{decMantissa}",
            > 0 => $"E+{decMantissa}",
            _ => ""
        };

        return $"{strSign}{strSignificand}{strMantissa}";
    }

    #endregion Overridden methods

    #region Interface methods

    public bool Equals(BigDecimal? other)
    {
        if (other is null)
        {
            return false;
        }

        // Compare flags and mantissa.
        if (IsNegative != other.IsNegative || Mantissa != other.Mantissa)
        {
            return false;
        }

        // Compare number of billits.
        if (Length != other.Length)
        {
            return false;
        }

        // Compare the billits in the significand.
        for (int i = 0; i < Length; i++)
        {
            if (Significand[i] != other.Significand[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public int CompareTo(BigDecimal? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other), "Cannot compare a BigDecimal and null.");
        }

        // Compare signs.
        if (Sign < other.Sign)
        {
            return -1;
        }
        if (Sign > other.Sign)
        {
            return 1;
        }

        // Compare mantissas.
        if (Mantissa < other.Mantissa)
        {
            return -1;
        }
        if (Mantissa > other.Mantissa)
        {
            return 1;
        }

        // Compare significands.
        int maxLen = Max(Length, other.Length);
        for (int i = 0; i < maxLen; i++)
        {
            int left = i < Length ? Significand[i] : 0;
            int right = i < other.Length ? other.Significand[i] : 0;
            if (left < right)
            {
                return -1;
            }
            if (left > right)
            {
                return 1;
            }
        }

        // The numbers are equal.
        return 0;
    }

    #endregion Interface methods

    #region Comparison operators

    public static bool operator ==(BigDecimal left, BigDecimal right) => left.Equals(right);

    public static bool operator !=(BigDecimal left, BigDecimal right) => !left.Equals(right);

    public static bool operator <(BigDecimal left, BigDecimal right) => left.CompareTo(right) < 0;

    public static bool operator >(BigDecimal left, BigDecimal right) => left.CompareTo(right) > 0;

    public static bool operator <=(BigDecimal left, BigDecimal right) =>
        left.CompareTo(right) is < 0 or 0;

    public static bool operator >=(BigDecimal left, BigDecimal right) =>
        left.CompareTo(right) is > 0 or 0;

    #endregion Comparison operators

    #region Arithmetic methods

    public static BigDecimal Abs(BigDecimal bd)
    {
        // Make every billit in the significand positive.
        int[] significand = bd.Significand.Select(Math.Abs).ToArray();
        return new BigDecimal(significand, bd.Mantissa);
    }

    /// <summary>
    /// Negation method.
    /// </summary>
    /// <param name="bd"></param>
    /// <returns></returns>
    public static BigDecimal Negate(BigDecimal bd)
    {
        // Negate every billit in the significand.
        int[] significand = bd.Significand.Select(i => -i).ToArray();
        return new BigDecimal(significand, bd.Mantissa);
    }

    /// <summary>
    /// Negation operator (unary minus).
    /// </summary>
    /// <param name="bd"></param>
    /// <returns></returns>
    public static BigDecimal operator -(BigDecimal bd) => Negate(bd);

    public static BigDecimal Add(BigDecimal bd1, BigDecimal bd2)
    {
        // Optimizations.
        if (bd1.IsZero)
        {
            return bd2;
        }
        if (bd2.IsZero)
        {
            return bd1;
        }
        if (bd1 == -bd2)
        {
            return Zero();
        }

        // Determine the min and max exponents of the operands.
        int maxExp1 = bd1.Mantissa;
        int len1 = bd1.Length;
        int minExp1 = maxExp1 - len1 + 1;

        int maxExp2 = bd2.Mantissa;
        int len2 = bd2.Length;
        int minExp2 = maxExp2 - len2 + 1;

        // Determine the min and max exponents of the result.
        // Add 1 extra to the max in case of overflow.
        int maxExp3 = Max(maxExp1, maxExp2) + 1;
        int minExp3 = Min(minExp1, minExp2);
        int len3 = maxExp3 - minExp3 + 1;

        // TODO Check for overflow/underflow here.

        // Cap at maximum length. Add 1 extra at the beginning to allow for
        // overflow and 1 at the end for rounding.
        const int MAX_LEN2 = _MAX_LENGTH + 2;
        if (len3 > MAX_LEN2)
        {
            len3 = MAX_LEN2;
            minExp3 = maxExp3 - len3 + 1;
        }

        // If one number is much greater than the other, and no digits overlap,
        // nothing to add.
        if (minExp3 > maxExp2)
        {
            return bd1;
        }
        if (minExp3 > maxExp1)
        {
            return bd2;
        }

        // Create array for the result.
        int[] significand3 = new int[len3];

        // Loop through the result digits, adding.
        for (int index3 = 1; index3 < len3; index3++)
        {
            // What exponent are we looking at?
            int exp = maxExp3 - index3;

            // Get the billit from bd1.
            int billit1 = GetBillit(bd1.Significand, maxExp1 - exp);

            // Get the billit from bd2.
            int billit2 = GetBillit(bd2.Significand, maxExp2 - exp);

            // Add the two billits and place in the result.
            SetBillit(significand3, index3, billit1 + billit2);
        }

        // Check for zero
        if (GetSign(significand3) == 0)
        {
            return Zero();
        }

        // Fix over/underflow.
        FixOverflow(ref significand3, ref maxExp3);

        // Construct the result object.
        return new BigDecimal(significand3, maxExp3);
    }

    public static BigDecimal operator +(BigDecimal bd1, BigDecimal bd2) => Add(bd1, bd2);

    public static BigDecimal Subtract(BigDecimal bd1, BigDecimal bd2) => Add(bd1, -bd2);

    public static BigDecimal operator -(BigDecimal bd1, BigDecimal bd2) => Subtract(bd1, bd2);

    public static BigDecimal Multiply(BigDecimal bd1, BigDecimal bd2)
    {
        // Optimizations.
        if (bd1.IsZero || bd2.IsZero)
        {
            return Zero();
        }
        if (bd1.IsOne)
        {
            return bd2;
        }
        if (bd2.IsOne)
        {
            return bd1;
        }

        // Determine the min and max exponents of the operands.
        int maxExp1 = bd1.Mantissa;
        int len1 = bd1.Length;
        int minExp1 = maxExp1 - len1 + 1;

        int maxExp2 = bd2.Mantissa;
        int len2 = bd2.Length;
        int minExp2 = maxExp2 - len2 + 1;

        // Determine the min and max exponents of the result.
        // Add 1 extra to the max in case of overflow.
        int maxExp3 = maxExp1 + maxExp2 + 1;
        int minExp3 = minExp1 + minExp2;
        int len3 = maxExp3 - minExp3 + 1;

        // Cap at maximum length. Add 1 extra at the beginning to allow for
        // overflow and 1 at the end for rounding.
        if (len3 > _MAX_LENGTH + 2)
        {
            len3 = _MAX_LENGTH + 2;
        }

        // Initialise result array.
        int[] significand3 = new int[len3];

        for (int index1 = 0; index1 < bd1.Length; index1++)
        {
            // Get the left-hand digit and exponent.
            int billit1 = bd1.Significand[index1];
            int exp1 = maxExp1 - index1;
            for (int index2 = 0; index2 < bd2.Length; index2++)
            {
                // Get the right-hand digit and exponent.
                int billit2 = bd2.Significand[index2];
                int exp2 = maxExp2 - index2;

                // Shortcut.
                if (billit1 == 0 || billit2 == 0)
                {
                    continue;
                }

                // Multiply.
                long product = (long)billit1 * billit2;
                int exp3 = exp1 + exp2;

                // Add to result.
                int index3 = maxExp3 - exp3;
                List<int> indices = Enumerable.Range(0, significand3.Length).ToList();
                bool index3InRange = indices.Contains(index3);
                bool index3Less1InRange = indices.Contains(index3 - 1);
                if (index3InRange)
                {
                    significand3[index3] += (int)(product % _B);
                }
                if (index3Less1InRange)
                {
                    significand3[index3 - 1] += (int)(product / _B);
                }

                // Check for overflow.
                if (index3InRange)
                {
                    FixOverflow(ref significand3, ref maxExp3, index3);
                }
                else if (index3Less1InRange)
                {
                    FixOverflow(ref significand3, ref maxExp3, index3 - 1);
                }
            } // for index2
        } // for index1

        return new BigDecimal(significand3, maxExp3);
    }

    public static BigDecimal operator *(BigDecimal bd1, BigDecimal bd2) => Multiply(bd1, bd2);

    /// <summary>
    /// Computes the division using the Goldschmidt algorithm.
    /// <see href="https://en.wikipedia.org/wiki/Division_algorithm#Goldschmidt_division" />
    /// </summary>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    /// <returns></returns>
    /// <exception cref="DivideByZeroException"></exception>
    public static BigDecimal Divide(BigDecimal numerator, BigDecimal denominator)
    {
        // Guard.
        if (denominator.IsZero)
        {
            throw new DivideByZeroException("Division by 0 is undefined.");
        }

        // Shortcuts.
        if (denominator.IsOne)
        {
            return numerator;
        }
        if (numerator == denominator)
        {
            return One();
        }

        // Get the approximate result mantissa.
        int resultMantissa = numerator.Mantissa - denominator.Mantissa;

        // Remove the mantissas from the operands. This enables us to calculate
        // f using the decimal type.
        BigDecimal n = new(numerator.Significand);
        BigDecimal d = new(denominator.Significand);

        // Get a good initial estimate of the multiplication factor.
        BigDecimal f = 1m / (decimal)d;

        // Get 2 as a BigDecimal to avoid doing the conversion every time.
        BigDecimal two = 2;

        while (true)
        {
            n = Multiply(n, f);
            d = Multiply(d, f);
            // If the denominator is 1, then n is the result.
            if (d.IsOne)
            {
                break;
            }

            f = Subtract(two, d);

            // If d is not 1, but is close to 1, then f can be 1 due to
            // rounding after the subtraction. If it is, there's no point
            // continuing.
            if (f.IsOne)
            {
                break;
            }
        }

        return new BigDecimal(n.Significand, n.Mantissa + resultMantissa);
    }

    public static BigDecimal operator /(BigDecimal bd1, BigDecimal bd2) => Divide(bd1, bd2);

    /// <summary>
    /// Compute the reciprocal of the given value.
    /// </summary>
    /// <param name="bd"></param>
    /// <returns></returns>
    public static BigDecimal Reciprocal(BigDecimal bd) => Divide(One(), bd);

    #endregion Arithmetic methods

    #region Conversion methods and cast operators

    /// <summary>
    /// Construct from ulong.
    /// </summary>
    /// <param name="l">An unsigned long value.</param>
    public static BigDecimal FromUInt64(ulong l)
    {
        ulong rem = l;
        List<int> billits = new();
        while (rem > 0)
        {
            int billit = (int)(rem % _B);
            billits.Insert(0, billit);
            rem /= _B;
        }

        return new BigDecimal(billits.ToArray(), billits.Count - 1);
    }

    /// <summary>
    /// Construct from long.
    /// </summary>
    /// <param name="l">A signed long value.</param>
    public static BigDecimal FromInt64(long l) =>
        l < 0 ? -FromUInt64(Integers.Abs(l)) : FromUInt64((ulong)l);

    public static BigDecimal FromDecimal(decimal d)
    {
        if (d == 0)
        {
            return Zero();
        }

        bool isNegative = d < 0;
        decimal abs = Math.Abs(d);
        int mantissa = (int)decimal.Floor(XDecimal.Log(abs, _B));
        decimal rem;

        switch (mantissa)
        {
            // Shift the decimal to something reasonable so we don't end up generating a bunch of
            // zeros.
            case < 0:
            {
                // Cap the mantissa so Pow() doesn't overflow.
                if (mantissa < -3)
                {
                    mantissa = -3;
                }
                rem = abs * DecimalEx.Pow(_B, -mantissa);
                break;
            }

            case > 0:
            {
                // Cap the mantissa so Pow() doesn't overflow.
                if (mantissa > 3)
                {
                    mantissa = 3;
                }
                rem = abs / DecimalEx.Pow(_B, mantissa);
                break;
            }

            default:
                // mantissa = 0
                rem = abs;
                break;
        }

        int[] billits = new int[_MAX_LENGTH];
        int cur = 0;

        while (rem > 0)
        {
            int billit = (int)Floor(rem);
            billits[cur++] = billit;
            decimal frac = rem - billit;
            rem = frac * _B;
        }

        BigDecimal bd = new(billits, mantissa);
        return isNegative ? -bd : bd;
    }

    /// <summary>
    /// Convert a BigDecimal to a decimal.
    /// NB: May involve loss of precision.
    /// </summary>
    /// <param name="bd"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException">
    /// If the magnitude of the BigDecimal
    /// is too large to be represented by a decimal.
    /// </exception>
    public static decimal ToDecimal(BigDecimal bd)
    {
        decimal m = 0;
        decimal scale = 0;

        for (int i = 0; i < bd.Length; i++)
        {
            try
            {
                // Get the scale for the billit.
                scale = i == 0
                    ? DecimalEx.Pow(_B, bd.Mantissa)
                    : scale / _B;
                // Add the billit value.
                m += bd.Significand[i] * scale;
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("The number is too large to convert to a decimal.", ex);
            }
        }

        return m;
    }

    public static BigDecimal FromDouble(double d)
    {
        if (d == 0)
        {
            return Zero();
        }

        bool isNegative = d < 0;
        double abs = Math.Abs(d);
        int mantissa = (int)Floor(Log(abs, _B));
        double rem;

        // Shift the decimal to something reasonable so we don't waste time// generating a bunch of
        // 0s.
        switch (mantissa)
        {
            case < 0:
            {
                // Cap the mantissa so Pow() doesn't overflow.
                if (mantissa < -34)
                {
                    mantissa = -34;
                }
                rem = abs * Pow(_B, -mantissa);
                break;
            }

            case > 0:
            {
                // Cap the mantissa so Pow() doesn't overflow.
                if (mantissa > 34)
                {
                    mantissa = 34;
                }
                rem = abs / Pow(_B, mantissa);
                break;
            }

            default:
                // mantissa = 0
                rem = abs;
                break;
        }

        int[] billits = new int[_MAX_LENGTH + 1];
        int cur = 0;

        while (rem > 0)
        {
            int billit = (int)Floor(rem);
            billits[cur++] = billit;
            if (cur == billits.Length)
            {
                break;
            }
            double frac = rem - billit;
            rem = frac * _B;
        }

        BigDecimal bd = new(billits, mantissa);
        return isNegative ? -bd : bd;
    }

    /// <summary>
    /// Convert a BigDecimal to a double.
    /// NB: May involve loss of precision.
    /// </summary>
    /// <param name="bd"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException">
    /// If the magnitude of the BigDecimal
    /// is too large to be represented by a double.
    /// </exception>
    public static double ToDouble(BigDecimal bd) => double.Parse(bd.ToString());

    public static implicit operator BigDecimal(ulong l) => FromUInt64(l);
    public static implicit operator BigDecimal(long l) => FromInt64(l);
    public static implicit operator BigDecimal(uint i) => FromUInt64(i);
    public static implicit operator BigDecimal(int i) => FromInt64(i);

    public static implicit operator BigDecimal(decimal m) => FromDecimal(m);
    public static explicit operator decimal(BigDecimal bd) => ToDecimal(bd);

    public static implicit operator BigDecimal(double d) => FromDouble(d);
    public static explicit operator double(BigDecimal bd) => ToDouble(bd);

    #endregion Conversion methods and cast operators

    #region Helper methods

    /// <summary>
    /// Round off an array of billits to MaxBillits billits.
    /// The rounding strategy employed is MidpointRounding.ToEven, the same as
    /// for Math.Round().
    /// </summary>
    /// <param name="significand"></param>
    /// <param name="mantissa"></param>
    /// <returns>A new array containing maximum MaxBillits billits.</returns>
    public static int[] Round(int[] significand, ref int mantissa)
    {
        if (significand.Length <= _MAX_LENGTH)
        {
            // No rounding to do, so just return a copy of the array.
            return significand[..];
        }

        const int MIDPOINT = 500000000;

        // Trim excess digits.
        int[] result = significand[.._MAX_LENGTH];

        // See if we need to round up the last digit.
        if (significand[_MAX_LENGTH] > MIDPOINT ||
            (significand[_MAX_LENGTH] == MIDPOINT && significand[_MAX_LENGTH - 1] % 2 == 1))
        {
            result[^1]++;
            FixOverflow(ref result, ref mantissa);

            // If an extra digit was prepended due to overflow, the last one
            // must be 0. Lop it off to get the right number of billits.
            if (result.Length > _MAX_LENGTH)
            {
                result = result[.._MAX_LENGTH];
            }
        }

        return result;
    }

    public static BigDecimal Round(BigDecimal bd)
    {
        int[] significand = bd.Significand;
        int mantissa = bd.Mantissa;
        significand = Round(significand, ref mantissa);
        return new BigDecimal(significand, mantissa);
    }

    /// <summary>
    /// Trim leading zeros, adjusting the mantissa if needed.
    /// </summary>
    /// <param name="significand">The array of billits.</param>
    /// <param name="mantissa">The exponent of the first billit.</param>
    /// <returns>A new array of billits with leading zeros removed.</returns>
    public static int[] TrimLeadingZeros(int[] significand, ref int mantissa)
    {
        // Find the first non-zero int in the array.
        int? first = null;
        for (int i = 0; i < significand.Length; i++)
        {
            if (significand[i] != 0)
            {
                first = i;
                break;
            }
        }

        // Check for zero.
        if (first == null)
        {
            mantissa = 0;
            return Array.Empty<int>();
        }

        // Adjust the mantissa.
        mantissa -= first.Value;

        // Copy the billits into the result without the leading zeros.
        return significand[first.Value..];
    }

    /// <summary>
    /// Trim trailing zeros.
    /// </summary>
    /// <param name="significand">The array of billits.</param>
    /// <param name="mantissa">The exponent of the first billit.</param>
    /// <returns>A new array of billits with trailing zeros removed.</returns>
    public static int[] TrimTrailingZeros(int[] significand, ref int mantissa)
    {
        // Find the last non-zero int in the array.
        int? last = null;
        for (int i = significand.Length - 1; i >= 0; i--)
        {
            if (significand[i] != 0)
            {
                last = i;
                break;
            }
        }

        // Check for zero.
        if (last == null)
        {
            mantissa = 0;
            return Array.Empty<int>();
        }

        // Copy the bytes into the result without the trailing zeros.
        return significand[..(last.Value + 1)];
    }

    /// <summary>
    /// Trim any leading or trailing zeros, adjusting the mantissa if needed.
    /// Same as above but for strings of decimal digits.
    /// </summary>
    /// <param name="digits">The string of decimal digits.</param>
    /// <param name="mantissa">The max power of 10.</param>
    /// <returns></returns>
    private static string TrimDigits(string digits, ref int mantissa)
    {
        // Find the first and last non-zero chars in the array.
        int? first = null;
        int? last = null;
        for (int i = 0; i < digits.Length; i++)
        {
            if (digits[i] != '0')
            {
                first ??= i;
                last = i;
            }
        }

        // Check for zero.
        if (first == null)
        {
            mantissa = 0;
            return "";
        }

        // Adjust the mantissa.
        mantissa -= first.Value;

        // Copy the chars into the result without the leading or trailing zeros.
        return digits[first.Value..(last!.Value + 1)];
    }

    public static int GetSign(int[] significand)
    {
        // Look for the first non-zero digit.
        foreach (int billit in significand)
        {
            switch (billit)
            {
                case > 0:
                    return 1;

                case < 0:
                    return -1;
            }
        }

        return 0;
    }

    public static void FixOverflow(ref int[] significand, ref int mantissa, int? startIndex = null)
    {
        // Get the min and max valid values for the billits.
        int sign = GetSign(significand);
        int min = sign == 1 ? 0 : -_B + 1;
        int max = sign == 1 ? _B - 1 : 0;

        // Spare billit if needed for overflow.
        int? extraBillit = null;

        // Set start index at the right-most billit if not provided.
        startIndex ??= significand.Length - 1;

        // Check billits for over/underflow. Go right to left.
        for (int i = startIndex.Value; i >= 0; i--)
        {
            if (significand[i] < min)
            {
                significand[i] += _B;
                if (i - 1 < 0)
                {
                    extraBillit = -1;
                }
                else
                {
                    significand[i - 1]--;
                }
            }
            else if (significand[i] > max)
            {
                significand[i] -= _B;
                if (i - 1 < 0)
                {
                    extraBillit = 1;
                }
                else
                {
                    significand[i - 1]++;
                }
            }
        }

        // Prepend the extra billit if necessary.
        if (extraBillit.HasValue)
        {
            int[] temp = significand;
            significand = new int[temp.Length + 1];
            significand[0] = extraBillit.Value;
            Array.Copy(temp, 0, significand, 1, temp.Length);
            mantissa++;
        }
    }

    public static int GetBillit(int[] significand, int index) =>
        index >= 0 && index < significand.Length ? significand[index] : 0;

    public static void SetBillit(int[] significand, int index, int value)
    {
        if (index >= 0 && index < significand.Length)
        {
            significand[index] = value;
        }
    }

    #endregion Helper methods
}
