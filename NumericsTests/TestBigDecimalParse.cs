using System.Numerics;
using AstroMultimedia.Core.Exceptions;
using AstroMultimedia.Numerics.Types;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestBigDecimalParse
{
    [TestMethod]
    public void TestParse0()
    {
        BigDecimal bd = BigDecimal.Parse("0");
        Assert.AreEqual(0, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParse1()
    {
        BigDecimal bd = BigDecimal.Parse("1");
        Assert.AreEqual(1, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParse2()
    {
        BigDecimal bd = BigDecimal.Parse("2");
        Assert.AreEqual(2, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParseMinus1()
    {
        BigDecimal bd = BigDecimal.Parse("-1");
        Assert.AreEqual(-1, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParsePlus1()
    {
        BigDecimal bd = BigDecimal.Parse("+1");
        Assert.AreEqual(1, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParse10()
    {
        BigDecimal bd = BigDecimal.Parse("10");
        Assert.AreEqual(1, bd.Significand);
        Assert.AreEqual(1, bd.Exponent);
    }

    [TestMethod]
    public void TestParseMinus200()
    {
        BigDecimal bd = BigDecimal.Parse("-200");
        Assert.AreEqual(-2, bd.Significand);
        Assert.AreEqual(2, bd.Exponent);
    }

    [TestMethod]
    public void TestParsePositiveFloat()
    {
        BigDecimal bd = BigDecimal.Parse("3.14");
        Assert.AreEqual(314, bd.Significand);
        Assert.AreEqual(-2, bd.Exponent);
    }

    [TestMethod]
    public void TestParseNegativeFloat()
    {
        BigDecimal bd = BigDecimal.Parse("-6.28");
        Assert.AreEqual(-628, bd.Significand);
        Assert.AreEqual(-2, bd.Exponent);
    }

    [TestMethod]
    public void TestParsePi()
    {
        BigDecimal bd = BigDecimal.Pi;
        BigInteger expectedSignificand = BigInteger.Parse("3"
            + "14159265358979323846264338327950288419716939937510"
            + "58209749445923078164062862089986280348253421170679");
        Assert.AreEqual(expectedSignificand, bd.Significand);
        Assert.AreEqual(-100, bd.Exponent);
    }

    [TestMethod]
    public void TestParseFloatWithPositiveExponent()
    {
        // Avagadro's number.
        BigDecimal bd = BigDecimal.Parse("6.0221408e+23");
        Assert.AreEqual(60221408, bd.Significand);
        Assert.AreEqual(16, bd.Exponent);
    }

    [TestMethod]
    public void TestParseFloatWithPositiveExponentNoE()
    {
        // Astronomical unit in meters.
        BigDecimal bd = BigDecimal.Parse("1.496e11");
        Assert.AreEqual(1496, bd.Significand);
        Assert.AreEqual(8, bd.Exponent);
    }

    [TestMethod]
    public void TestParseFloatWithNegativeExponent()
    {
        // Charge on an electron.
        BigDecimal bd = BigDecimal.Parse("1.60217663e-19");
        Assert.AreEqual(160217663, bd.Significand);
        Assert.AreEqual(-27, bd.Exponent);
    }

    [TestMethod]
    public void TestParseNumberWithCommasForThousandsSeparators()
    {
        // Astronomical unit in meters.
        BigDecimal bd = BigDecimal.Parse("149,597,870,700");
        Assert.AreEqual(1495978707, bd.Significand);
        Assert.AreEqual(2, bd.Exponent);
    }

    [TestMethod]
    public void TestParseNumberWithSpacesForThousandsSeparators()
    {
        // Astronomical unit in meters.
        BigDecimal bd = BigDecimal.Parse("149 597 870 700");
        Assert.AreEqual(1495978707, bd.Significand);
        Assert.AreEqual(2, bd.Exponent);
    }

    [TestMethod]
    public void TestParseNumberWithUnderscoresForThousandsSeparators()
    {
        // Astronomical unit in meters.
        BigDecimal bd = BigDecimal.Parse("149_597_870_700");
        Assert.AreEqual(1495978707, bd.Significand);
        Assert.AreEqual(2, bd.Exponent);
    }

    [TestMethod]
    public void TestParseZeroFraction()
    {
        BigDecimal bd = BigDecimal.Parse("427.0000");
        Assert.AreEqual(427, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParseZeroFraction2()
    {
        BigDecimal bd = BigDecimal.Parse("42.700");
        Assert.AreEqual(427, bd.Significand);
        Assert.AreEqual(-1, bd.Exponent);
    }

    [TestMethod]
    public void TestParseZeroInteger()
    {
        BigDecimal bd = BigDecimal.Parse("0.7");
        Assert.AreEqual(7, bd.Significand);
        Assert.AreEqual(-1, bd.Exponent);
    }

    [TestMethod]
    public void TestParseZeroInteger2()
    {
        BigDecimal bd = BigDecimal.Parse("0000.735");
        Assert.AreEqual(735, bd.Significand);
        Assert.AreEqual(-3, bd.Exponent);
    }

    [TestMethod]
    public void TestParseZeroExponent()
    {
        BigDecimal bd = BigDecimal.Parse("3.1416e0");
        Assert.AreEqual(31416, bd.Significand);
        Assert.AreEqual(-4, bd.Exponent);
    }

    [TestMethod]
    public void TestParseInvalidFormat1()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse(""));
    }

    [TestMethod]
    public void TestParseInvalidFormat2()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("e15"));
    }

    [TestMethod]
    public void TestParseInvalidFormat3()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("1.445345.8"));
    }

    [TestMethod]
    public void TestParseMissingIntegerDigits()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse(".1414"));
    }

    [TestMethod]
    public void TestParseMissingFractionDigits()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("612."));
    }

    [TestMethod]
    public void TestParseMissingExponentDigits()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("6.02e"));
    }

    [TestMethod]
    public void TestParseWord()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("cat"));
    }

    [TestMethod]
    public void TestParseQuantity()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("3891.6 km"));
    }
}
