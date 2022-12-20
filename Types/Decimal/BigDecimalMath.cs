using System.Numerics;
using AstroMultimedia.Core.Strings;

namespace AstroMultimedia.Numerics.Types;

public partial struct BigDecimal
{
    #region Arithmetic methods

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
            int sign = x.Significand < 0 ? -1 : 1;
            BigInteger absSig = BigInteger.Abs(x.Significand);
            string strAbsSig = absSig.ToString();
            BigInteger newAbsSig;
            string strRight;
            if (nDigitsToCut >= strAbsSig.Length)
            {
                newAbsSig = 0;
                sign = 1;
                strRight = "0".Repeat(nDigitsToCut - strAbsSig.Length) + strAbsSig;
            }
            else
            {
                string strLeft = strAbsSig[..^nDigitsToCut];
                strRight = strAbsSig[^nDigitsToCut..];
                newAbsSig = BigInteger.Parse(strLeft);
            }

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

    #endregion Arithmetic methods

    #region Arithmetic operators

    /// <inheritdoc />
    public static BigDecimal operator +(BigDecimal value) =>
        (BigDecimal)value.Clone();

    /// <inheritdoc />
    public static BigDecimal operator +(BigDecimal left, BigDecimal right)
    {
        (BigDecimal x, BigDecimal y) = Align(left, right);
        return new BigDecimal(x.Significand + y.Significand, x.Exponent);
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
        return new BigDecimal(x.Significand - y.Significand, x.Exponent);
    }

    /// <inheritdoc />
    public static BigDecimal operator --(BigDecimal value) =>
        value - One;

    /// <inheritdoc />
    public static BigDecimal operator *(BigDecimal left, BigDecimal right) =>
        new (left.Significand * right.Significand, left.Exponent + right.Exponent);

    /// <inheritdoc />
    /// <remarks>
    /// Computes division using the Goldschmidt algorithm.
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
}
