using AstroMultimedia.Core.Numbers;
using AstroMultimedia.Numerics.Types;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestBigDecimal
{
    // [TestMethod]
    // public void DigitsToSignificandTest()
    // {
    //     byte[] digits = { 1, 2, 3, 4, 5 };
    //     byte[] significand = BigDecimal.PackDigits(digits);
    //     Assert.AreEqual(18, significand[0]);
    //     Assert.AreEqual(52, significand[1]);
    //     Assert.AreEqual(80, significand[2]);
    // }

    // [TestMethod]
    // public void SignificandToDigitsTest()
    // {
    //     byte[] significand = { 18, 52, 80, 0 };
    //     byte[] digits = BigDecimal.UnpackDigits(significand);
    //     Assert.AreEqual(1, digits[0]);
    //     Assert.AreEqual(2, digits[1]);
    //     Assert.AreEqual(3, digits[2]);
    //     Assert.AreEqual(4, digits[3]);
    //     Assert.AreEqual(5, digits[4]);
    //     Assert.AreEqual(0, digits[5]);
    //     Assert.AreEqual(0, digits[6]);
    //     Assert.AreEqual(0, digits[7]);
    // }

    [TestMethod]
    public void FromStringPositiveValuesTest()
    {
        string s;
        BigDecimal bd;

        s = "0";
        bd = new BigDecimal(s);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(0, bd.Significand.Length);
        Assert.AreEqual(0, bd.Mantissa);

        s = "12345";
        bd = new BigDecimal(s);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(1, bd.Significand.Length);
        Assert.AreEqual(12345, bd.Significand[0]);
        Assert.AreEqual(0, bd.Mantissa);

        s = "1234567890";
        bd = new BigDecimal(s);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(1, bd.Significand[0]);
        Assert.AreEqual(234567890, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        s = "1.2345";
        bd = new BigDecimal(s);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(1, bd.Significand[0]);
        Assert.AreEqual(234500000, bd.Significand[1]);
        Assert.AreEqual(0, bd.Mantissa);

        s = "1.2345e10"; // 12_345_000_000
        bd = new BigDecimal(s);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(12, bd.Significand[0]);
        Assert.AreEqual(345000000, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        s = "123.45e10";
        bd = new BigDecimal(s);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(1234, bd.Significand[0]);
        Assert.AreEqual(500000000, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        s = "123.45e-10";
        bd = new BigDecimal(s);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(12, bd.Significand[0]);
        Assert.AreEqual(345000000, bd.Significand[1]);
        Assert.AreEqual(-1, bd.Mantissa);
    }

    [TestMethod]
    public void FromStringNegativeValuesTest()
    {
        string s;
        BigDecimal bd;

        s = "-12345";
        bd = new BigDecimal(s);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(1, bd.Significand.Length);
        Assert.AreEqual(-12345, bd.Significand[0]);
        Assert.AreEqual(0, bd.Mantissa);

        s = "-1234567890";
        bd = new BigDecimal(s);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(-1, bd.Significand[0]);
        Assert.AreEqual(-234567890, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        s = "-1.2345";
        bd = new BigDecimal(s);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(-1, bd.Significand[0]);
        Assert.AreEqual(-234500000, bd.Significand[1]);
        Assert.AreEqual(0, bd.Mantissa);

        s = "-1.2345e10";
        bd = new BigDecimal(s);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(-12, bd.Significand[0]);
        Assert.AreEqual(-345000000, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        s = "-123.45e10";
        bd = new BigDecimal(s);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(-1234, bd.Significand[0]);
        Assert.AreEqual(-500000000, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        s = "-123.45e-10";
        bd = new BigDecimal(s);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(-12, bd.Significand[0]);
        Assert.AreEqual(-345000000, bd.Significand[1]);
        Assert.AreEqual(-1, bd.Mantissa);
    }

    [TestMethod]
    public void ToStringTest()
    {
        BigDecimal bd;
        int[] digits = { 111111111, 222222222, 333333333, 444444444, 555555555 };

        bd = new BigDecimal();
        Assert.AreEqual("0", bd.ToString());

        bd = new BigDecimal(digits);
        Assert.AreEqual("1.11111111222222222333333333444444444555555555E+8", bd.ToString());

        bd = new BigDecimal(digits, 1);
        Assert.AreEqual("1.11111111222222222333333333444444444555555555E+17", bd.ToString());

        bd = new BigDecimal(digits, -1);
        Assert.AreEqual("1.11111111222222222333333333444444444555555555E-1", bd.ToString());
    }

    [TestMethod]
    public void FromUInt64Test()
    {
        ulong ul;
        BigDecimal bd;

        ul = 0;
        bd = BigDecimal.FromUInt64(ul);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(0, bd.Significand.Length);
        Assert.AreEqual(0, bd.Mantissa);

        ul = 12345;
        bd = BigDecimal.FromUInt64(ul);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(1, bd.Significand.Length);
        Assert.AreEqual(12345, bd.Significand[0]);
        Assert.AreEqual(0, bd.Mantissa);

        ul = 1234567890;
        bd = BigDecimal.FromUInt64(ul);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(1, bd.Significand[0]);
        Assert.AreEqual(234567890, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        // 18,446,744,073,709,551,615
        ul = ulong.MaxValue;
        bd = BigDecimal.FromUInt64(ul);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(3, bd.Significand.Length);
        Assert.AreEqual(18, bd.Significand[0]);
        Assert.AreEqual(446744073, bd.Significand[1]);
        Assert.AreEqual(709551615, bd.Significand[2]);
        Assert.AreEqual(2, bd.Mantissa);
    }

    [TestMethod]
    public void FromInt64Test()
    {
        long l;
        BigDecimal bd;

        l = 0;
        bd = BigDecimal.FromInt64(l);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(0, bd.Significand.Length);
        Assert.AreEqual(0, bd.Mantissa);

        l = 12345;
        bd = BigDecimal.FromInt64(l);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(1, bd.Significand.Length);
        Assert.AreEqual(12345, bd.Significand[0]);
        Assert.AreEqual(0, bd.Mantissa);

        l = -12345;
        bd = BigDecimal.FromInt64(l);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(1, bd.Significand.Length);
        Assert.AreEqual(-12345, bd.Significand[0]);
        Assert.AreEqual(0, bd.Mantissa);

        l = 1234567890;
        bd = BigDecimal.FromInt64(l);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(1, bd.Significand[0]);
        Assert.AreEqual(234567890, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        l = -1234567890;
        bd = BigDecimal.FromInt64(l);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(2, bd.Significand.Length);
        Assert.AreEqual(-1, bd.Significand[0]);
        Assert.AreEqual(-234567890, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        // 9,223,372,036,854,775,807
        l = long.MaxValue;
        bd = BigDecimal.FromInt64(l);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(3, bd.Significand.Length);
        Assert.AreEqual(9, bd.Significand[0]);
        Assert.AreEqual(223372036, bd.Significand[1]);
        Assert.AreEqual(854775807, bd.Significand[2]);
        Assert.AreEqual(2, bd.Mantissa);

        // -9,223,372,036,854,775,808
        l = long.MinValue;
        bd = BigDecimal.FromInt64(l);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(3, bd.Significand.Length);
        Assert.AreEqual(-9, bd.Significand[0]);
        Assert.AreEqual(-223372036, bd.Significand[1]);
        Assert.AreEqual(-854775808, bd.Significand[2]);
        Assert.AreEqual(2, bd.Mantissa);
    }

    [TestMethod]
    public void FromDecimalPositiveValuesTest()
    {
        decimal d;
        BigDecimal bd;

        d = 0;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(0, bd.Significand.Length);
        Assert.AreEqual(0, bd.Mantissa);

        d = 1.2345m;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(1, bd.Significand[0]);
        Assert.AreEqual(234500000, bd.Significand[1]);
        Assert.AreEqual(0, bd.Mantissa);

        d = 1.2345e10m;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(12, bd.Significand[0]);
        Assert.AreEqual(345000000, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        d = 123.45e10m;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(1234, bd.Significand[0]);
        Assert.AreEqual(500000000, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        d = 123.45e-10m;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(12, bd.Significand[0]);
        Assert.AreEqual(345000000, bd.Significand[1]);
        Assert.AreEqual(-1, bd.Mantissa);

        // 79,228,162,514,264,337,593,543,950,335
        d = decimal.MaxValue;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(4, bd.Significand.Length);
        Assert.AreEqual(79, bd.Significand[0]);
        Assert.AreEqual(228162514, bd.Significand[1]);
        Assert.AreEqual(264337593, bd.Significand[2]);
        Assert.AreEqual(543950335, bd.Significand[3]);
        Assert.AreEqual(3, bd.Mantissa);

        d = 1e-28m;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(1, bd.Significand.Length);
        Assert.AreEqual(100000000, bd.Significand[0]);
        Assert.AreEqual(-4, bd.Mantissa);
    }

    [TestMethod]
    public void FromDecimalNegativeValuesTest()
    {
        decimal d;
        BigDecimal bd;

        d = -1.2345m;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(-1, bd.Significand[0]);
        Assert.AreEqual(-234500000, bd.Significand[1]);
        Assert.AreEqual(0, bd.Mantissa);

        d = -1.2345e10m;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(-12, bd.Significand[0]);
        Assert.AreEqual(-345000000, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        d = -123.45e10m;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(-1234, bd.Significand[0]);
        Assert.AreEqual(-500000000, bd.Significand[1]);
        Assert.AreEqual(1, bd.Mantissa);

        d = -123.45e-10m;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(-12, bd.Significand[0]);
        Assert.AreEqual(-345000000, bd.Significand[1]);
        Assert.AreEqual(-1, bd.Mantissa);

        // -79,228,162,514,264,337,593,543,950,335
        d = decimal.MinValue;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(4, bd.Significand.Length);
        Assert.AreEqual(-79, bd.Significand[0]);
        Assert.AreEqual(-228162514, bd.Significand[1]);
        Assert.AreEqual(-264337593, bd.Significand[2]);
        Assert.AreEqual(-543950335, bd.Significand[3]);
        Assert.AreEqual(3, bd.Mantissa);

        d = -1e-28m;
        bd = BigDecimal.FromDecimal(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(1, bd.Significand.Length);
        Assert.AreEqual(-100000000, bd.Significand[0]);
        Assert.AreEqual(-4, bd.Mantissa);
    }

    [TestMethod]
    public void FromDoublePositiveValuesTest()
    {
        double d;
        BigDecimal bd;

        d = 0;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(0, bd.Significand.Length);
        Assert.AreEqual(0, bd.Mantissa);

        d = 1.2345;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(1, bd.Significand[0]);
        Assert.AreEqual(234500000, bd.Significand[1], 1);
        Assert.AreEqual(0, bd.Mantissa);

        d = 1.2345e10;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(12, bd.Significand[0]);
        Assert.AreEqual(345000000, bd.Significand[1], 1);
        Assert.AreEqual(1, bd.Mantissa);

        d = 123.45e10;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(1234, bd.Significand[0]);
        Assert.AreEqual(500000000, bd.Significand[1], 1);
        Assert.AreEqual(1, bd.Mantissa);

        d = 123.45e-10;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(12, bd.Significand[0]);
        Assert.AreEqual(345000000, bd.Significand[1], 1);
        Assert.AreEqual(-1, bd.Mantissa);

        // 1.7976931348623157E+308
        d = double.MaxValue;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(179, bd.Significand[0]);
        Assert.AreEqual(769313486, bd.Significand[1]);
        Assert.AreEqual(231570000, bd.Significand[2], 10000);
        Assert.AreEqual(34, bd.Mantissa);

        // 4.94065645841247E-324
        d = double.Epsilon;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(false, bd.IsNegative);
        Assert.AreEqual(4, bd.Significand[0]);
        Assert.AreEqual(940656458, bd.Significand[1]);
        Assert.AreEqual(412470000, bd.Significand[2], 10000);
        Assert.AreEqual(-36, bd.Mantissa);
    }

    [TestMethod]
    public void ToDoublePositiveValuesTest()
    {
        double d, d2;
        BigDecimal bd;
        const double percent = 1e-13;

        d = 0;
        bd = BigDecimal.FromDouble(d);
        d2 = (double)bd;
        Console.WriteLine($"d={d:G17}, bd={bd}, d2={d2:G17}");
        Assert.AreEqual(d, d2, percent);

        d = 1.2345;
        bd = BigDecimal.FromDouble(d);
        d2 = (double)bd;
        Console.WriteLine($"d={d:G17}, bd={bd}, d2={d2:G17}");
        XAssert.AreEqualPercent(d, d2, percent);

        d = 1.2345e10;
        bd = BigDecimal.FromDouble(d);
        d2 = (double)bd;
        Console.WriteLine($"d={d:G17}, bd={bd}, d2={d2:G17}");
        XAssert.AreEqualPercent(d, d2, percent);

        d = 123.45e10;
        bd = BigDecimal.FromDouble(d);
        d2 = (double)bd;
        Console.WriteLine($"d={d:G17}, bd={bd}, d2={d2:G17}");
        XAssert.AreEqualPercent(d, d2, percent);

        d = 123.45e-10;
        bd = BigDecimal.FromDouble(d);
        d2 = (double)bd;
        Console.WriteLine($"d={d:G17}, bd={bd}, d2={d2:G17}");
        XAssert.AreEqualPercent(d, d2, percent);

        // 1.7976931348623157E+308
        d = double.MaxValue;
        bd = BigDecimal.FromDouble(d);
        d2 = (double)bd;
        Console.WriteLine($"d={d:G17}, bd={bd}, d2={d2:G17}");
        XAssert.AreEqualPercent(d, d2, percent);

        // // 4.94065645841247E-324
        // d = double.Epsilon;
        // bd = BigDecimal.FromDouble(d);
        // d2 = (double)bd;
        // Console.WriteLine($"d={d:G17}, bd={bd}, d2={d2:G17}");
        // XAssert.AreEqualPercent(d, d2, percent);
    }

    [TestMethod]
    public void FromDoubleNegativeValuesTest()
    {
        double d;
        BigDecimal bd;

        d = -1.2345;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(-1, bd.Significand[0]);
        Assert.AreEqual(-234500000, bd.Significand[1], 1);
        Assert.AreEqual(0, bd.Mantissa);

        d = -1.2345e10;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(-12, bd.Significand[0]);
        Assert.AreEqual(-345000000, bd.Significand[1], 1);
        Assert.AreEqual(1, bd.Mantissa);

        d = -123.45e10;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(-1234, bd.Significand[0]);
        Assert.AreEqual(-500000000, bd.Significand[1], 1);
        Assert.AreEqual(1, bd.Mantissa);

        d = -123.45e-10;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(-12, bd.Significand[0]);
        Assert.AreEqual(-345000000, bd.Significand[1], 1);
        Assert.AreEqual(-1, bd.Mantissa);

        // -1.7976931348623157E+308
        d = double.MinValue;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(-179, bd.Significand[0]);
        Assert.AreEqual(-769313486, bd.Significand[1]);
        Assert.AreEqual(-231570000, bd.Significand[2], 10000);
        Assert.AreEqual(34, bd.Mantissa);

        // -4.94065645841247E-324
        d = -double.Epsilon;
        bd = BigDecimal.FromDouble(d);
        Assert.AreEqual(true, bd.IsNegative);
        Assert.AreEqual(-4, bd.Significand[0]);
        Assert.AreEqual(-940656458, bd.Significand[1]);
        Assert.AreEqual(-412470000, bd.Significand[2], 10000);
        Assert.AreEqual(-36, bd.Mantissa);
    }

    [TestMethod]
    public void AddZeroToNumberReturnsNumber()
    {
        // Arrange.
        BigDecimal bd1 = new ("5.4321");
        BigDecimal bd2 = new ("0");

        // Act.
        BigDecimal bd3 = bd1 + bd2;

        // Assert.
        Assert.AreEqual("5.4321", bd3.ToString());
    }

    [TestMethod]
    public void AddNumberToZeroReturnsNumber()
    {
        // Arrange.
        BigDecimal bd1 = new ("0");
        BigDecimal bd2 = new ("5.4321");

        // Act.
        BigDecimal bd3 = bd1 + bd2;

        // Assert.
        Assert.AreEqual("5.4321", bd3.ToString());
    }

    [TestMethod]
    public void AddTest1()
    {
        // Arrange.
        BigDecimal bd1 = new ("1");
        BigDecimal bd2 = new ("1");

        // Act.
        BigDecimal bd3 = bd1 + bd2;

        // Assert.
        Assert.AreEqual("2", bd3.ToString());
    }

    [TestMethod]
    public void AddTest2()
    {
        // Arrange.
        BigDecimal bd1 = new ("5.4321");
        BigDecimal bd2 = new ("6.7894");

        // Act.
        BigDecimal bd3 = bd1 + bd2;

        // Assert.
        Assert.AreEqual("1.22215E+1", bd3.ToString());
    }

    [TestMethod]
    public void AddTest3()
    {
        // Arrange.
        BigDecimal bd1 = new ("12345");
        BigDecimal bd2 = new ("0.67890");

        // Act.
        BigDecimal bd3 = bd1 + bd2;

        // Assert.
        Assert.AreEqual("1.23456789E+4", bd3.ToString());
    }

    [TestMethod]
    public void AddTest4()
    {
        // Arrange.
        BigDecimal bd1 = new ("1e108");
        BigDecimal bd2 = new ("1");

        // Act.
        BigDecimal bd3 = bd1 + bd2;

        // Assert.
        string s = bd3.ToString();
        Assert.AreEqual("1E+108", s);
    }

    [TestMethod]
    public void AddTest5()
    {
        // Arrange.
        BigDecimal bd1 = new ("1e107");
        BigDecimal bd2 = new ("1");
        string expected = "1.0000000000000000000000000000000000000000000000000"
            + "0000000000000000000000000000000000000000000000000"
            + "000000001E+107";

        // Act.
        BigDecimal bd3 = bd1 + bd2;

        // Assert.
        string s = bd3.ToString();
        Assert.AreEqual(expected, s);
    }

    [TestMethod]
    public void ConvertToAndFromDecimalTest()
    {
        decimal m;
        BigDecimal bd;

        m = 0;
        bd = m;
        Assert.AreEqual(m, (decimal)bd);

        m = 1234567890;
        bd = m;
        Assert.AreEqual(m, (decimal)bd);

        m = 1.23456789e15m;
        bd = m;
        Assert.AreEqual(m, (decimal)bd);

        m = -9.87654321m;
        bd = m;
        Assert.AreEqual(m, (decimal)bd);

        m = -9.8765432e-20m;
        bd = m;
        Assert.AreEqual(m, (decimal)bd);

        m = decimal.MaxValue;
        bd = m;
        Assert.AreEqual(m, (decimal)bd);

        m = decimal.MinValue;
        bd = m;
        Assert.AreEqual(m, (decimal)bd);
    }

    [TestMethod]
    public void RandomAddTest()
    {
        decimal m1, m2, mSum;
        BigDecimal bd1, bd2, bdSum;
        Random rng = new ();

        // Create 1000 pairs of random decimals and add them.
        for (int i = 0; i < 1000; i++)
        {
            m1 = rng.NextDecimal();
            m2 = rng.NextDecimal();
            try
            {
                mSum = m1 + m2;
                int scale = (int)Floor(Log10((double)Abs(mSum)));
                // Console.WriteLine($"{m1} + {m2} = {mSum} (scale = {scale})");
                bd1 = m1;
                bd2 = m2;
                bdSum = bd1 + bd2;
                // decimal diff = Abs(mSum - (decimal)bdSum);
                // Check if they agree up to the second-least significant digit.
                Assert.AreEqual(mSum, (decimal)bdSum, XDecimal.Pow10(scale - 26));
                // Console.WriteLine($"Difference = {diff}");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow, continue.");
            }
        }
    }

    [TestMethod]
    public void RoundTest1()
    {
        // Arrange.
        BigDecimal bd1 = new ("1e107");
        BigDecimal bd2 = new ("0.5");

        // Act.
        BigDecimal bd3 = bd1 + bd2;

        string expected = "1E+107";
        // Assert.
        string s = bd3.ToString();
        Assert.AreEqual(expected, s);
    }

    [TestMethod]
    public void RoundTest2()
    {
        // Arrange.
        BigDecimal bd1 = new ("1E+107");
        BigDecimal bd2 = new ("0.6");

        // Act.
        BigDecimal bd3 = bd1 + bd2;

        string expected = "1.0000000000000000000000000000000000000000000000000"
            + "0000000000000000000000000000000000000000000000000000000001E+107";
        // Assert.
        string s = bd3.ToString();
        Assert.AreEqual(expected, s);
    }

    [TestMethod]
    public void RoundTest3()
    {
        // Arrange.
        BigDecimal bd1 = new ("1E+107");
        BigDecimal bd2 = new ("1.5");

        // Act.
        BigDecimal bd3 = bd1 + bd2;

        string expected = "1.0000000000000000000000000000000000000000000000000"
            + "0000000000000000000000000000000000000000000000000000000002E+107";
        // Assert.
        string s = bd3.ToString();
        Assert.AreEqual(expected, s);
    }

    [TestMethod]
    public void SubtractTest1()
    {
        // Arrange.
        BigDecimal bd1 = new ("1");
        BigDecimal bd2 = new ("1");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("0", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest2()
    {
        // Arrange.
        BigDecimal bd1 = new ("1");
        BigDecimal bd2 = new ("0");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("1", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest3()
    {
        // Arrange.
        BigDecimal bd1 = new ("0");
        BigDecimal bd2 = new ("1");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("-1", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest4()
    {
        // Arrange.
        BigDecimal bd1 = new ("8");
        BigDecimal bd2 = new ("5");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("3", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest5()
    {
        // Arrange.
        BigDecimal bd1 = new ("5");
        BigDecimal bd2 = new ("8");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("-3", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest6()
    {
        // Arrange.
        BigDecimal bd1 = new ("-8");
        BigDecimal bd2 = new ("5");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("-1.3E+1", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest7()
    {
        // Arrange.
        BigDecimal bd1 = new ("5");
        BigDecimal bd2 = new ("-8");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("1.3E+1", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest8()
    {
        // Arrange.
        BigDecimal bd1 = new ("-5");
        BigDecimal bd2 = new ("-8");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("3", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest9()
    {
        // Arrange.
        BigDecimal bd1 = new ("-8");
        BigDecimal bd2 = new ("-5");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("-3", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest10()
    {
        // Arrange.
        BigDecimal bd1 = new ("1");
        BigDecimal bd2 = new ("1");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("0", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest11()
    {
        // Arrange.
        BigDecimal bd1 = new ("67890");
        BigDecimal bd2 = new ("12345");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("5.5545E+4", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest12()
    {
        // Arrange.
        BigDecimal bd1 = new ("1234567890");
        BigDecimal bd2 = new ("9.87654321");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("1.23456788012345679E+9", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest13()
    {
        // Arrange.
        BigDecimal bd1 = new ("1E10");
        BigDecimal bd2 = new ("1");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("9.999999999E+9", bd3.ToString());
    }

    [TestMethod]
    public void SubtractTest14()
    {
        // Arrange.
        BigDecimal bd1 = new ("1");
        BigDecimal bd2 = new ("1E+10");

        // Act.
        BigDecimal bd3 = bd1 - bd2;

        // Assert.
        Assert.AreEqual("-9.999999999E+9", bd3.ToString());
    }

    [TestMethod]
    public void MultiplyTest1()
    {
        // Arrange.
        BigDecimal bd1 = 0;
        BigDecimal bd2 = 1.234m;

        // Act.
        BigDecimal bd3 = bd1 * bd2;

        // Assert.
        Assert.AreEqual(0, bd3);
    }

    [TestMethod]
    public void MultiplyTest2()
    {
        // Arrange.
        BigDecimal bd1 = 6.789m;
        BigDecimal bd2 = 0;

        // Act.
        BigDecimal bd3 = bd1 * bd2;

        // Assert.
        Assert.AreEqual(0, bd3);
    }

    [TestMethod]
    public void MultiplyTest4()
    {
        // Arrange.
        BigDecimal bd1 = 6.789m;
        BigDecimal bd2 = 1;

        // Act.
        BigDecimal bd3 = bd1 * bd2;

        // Assert.
        Assert.AreEqual(6.789m, bd3);
    }

    [TestMethod]
    public void MultiplyTest5()
    {
        // Arrange.
        BigDecimal bd1 = 2350m;
        BigDecimal bd2 = 15.67m;

        // Act.
        BigDecimal bd3 = bd1 * bd2;

        // Assert.
        Assert.AreEqual(36824.5m, bd3);
    }

    [TestMethod]
    public void MultiplyTest6()
    {
        // Arrange.
        BigDecimal bd1 = 1.23e20m;
        BigDecimal bd2 = 6.78e-12m;

        // Act.
        BigDecimal bd3 = bd1 * bd2;

        // Assert.
        Assert.AreEqual(833940000m, bd3);
    }

    [TestMethod]
    public void MultiplyTest7()
    {
        // Arrange.
        BigDecimal bd1 = -10000m;
        BigDecimal bd2 = 0.5m;

        // Act.
        BigDecimal bd3 = bd1 * bd2;

        // Assert.
        Assert.AreEqual(-5000m, bd3);
    }

    [TestMethod]
    public void MultiplyTest8()
    {
        // Arrange.
        BigDecimal bd1 = -9.8712m;
        BigDecimal bd2 = 481267111m;

        // Act.
        BigDecimal bd3 = bd1 * bd2;

        // Assert.
        Assert.AreEqual(-4750683906.1032m, bd3);
    }

    [TestMethod]
    public void MultiplyTest9()
    {
        // Arrange.
        BigDecimal bd1 = -0.0001234m;
        BigDecimal bd2 = -6789.543m;

        // Act.
        BigDecimal bd3 = bd1 * bd2;

        // Assert.
        Assert.AreEqual(0.8378296062m, bd3);
    }

    [TestMethod]
    public void MultiplyTest10()
    {
        // Arrange.
        BigDecimal bd1 = 9.9999m;
        BigDecimal bd2 = 99999.9m;

        // Act.
        BigDecimal bd3 = bd1 * bd2;

        // Assert.
        Assert.AreEqual(999989.00001m, bd3);
    }

    [TestMethod]
    public void RandomMultiplyTest()
    {
        double m1, m2, mProd;
        BigDecimal bd1, bd2, bdProd;
        Random rng = new ();
        double percent = 0.000001;

        // Create 1000 pairs of random decimals and multiply them.
        for (int i = 0; i < 1000; i++)
        {
            m1 = rng.NextDoubleFullRange();
            m2 = rng.NextDoubleFullRange();
            try
            {
                Console.WriteLine($"{m1} * {m2}");

                mProd = m1 * m2;

                if (double.IsInfinity(mProd))
                {
                    throw new OverflowException("Overflow, continue.");
                }

                bd1 = m1;
                bd2 = m2;
                bdProd = bd1 * bd2;

                Console.WriteLine($"Expected = {mProd:G17}");
                Console.WriteLine($"Actual = {bdProd}");
                double actualAsDouble = (double)bdProd;
                Console.WriteLine($"Actual as double = {actualAsDouble:G17}");

                // Check if they agree up to a reasonable number of digits.
                XAssert.AreEqualPercent(mProd, actualAsDouble, percent);
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow, continue.");
            }
            Console.WriteLine();
        }
    }

    [TestMethod]
    public void ReciprocalTest1()
    {
        BigDecimal x, y;

        x = 2;
        y = BigDecimal.Reciprocal(x);
        Assert.AreEqual(1, y.Length);
        Assert.AreEqual(500000000, y.Significand[0]);
        Assert.AreEqual(-1, y.Mantissa);
        Console.WriteLine(y);

        x = 3;
        y = BigDecimal.Reciprocal(x);
        Assert.AreEqual(12, y.Length);
        Assert.AreEqual(333333333, y.Significand[0]);
        Assert.AreEqual(333333333, y.Significand[1]);
        Assert.AreEqual(333333333, y.Significand[2]);
        Assert.AreEqual(333333333, y.Significand[3]);
        Assert.AreEqual(333333333, y.Significand[4]);
        Assert.AreEqual(333333333, y.Significand[5]);
        Assert.AreEqual(333333333, y.Significand[6]);
        Assert.AreEqual(333333333, y.Significand[7]);
        Assert.AreEqual(333333333, y.Significand[8]);
        Assert.AreEqual(333333333, y.Significand[9]);
        Assert.AreEqual(333333333, y.Significand[10]);
        Assert.AreEqual(333333333, y.Significand[11]);
        Assert.AreEqual(-1, y.Mantissa);
        Console.WriteLine(y);

        x = 4;
        y = BigDecimal.Reciprocal(x);
        Assert.AreEqual(1, y.Length);
        Assert.AreEqual(250000000, y.Significand[0]);
        Assert.AreEqual(-1, y.Mantissa);
        Console.WriteLine(y);

        x = 5;
        y = BigDecimal.Reciprocal(x);
        Assert.AreEqual(1, y.Length);
        Assert.AreEqual(200000000, y.Significand[0]);
        Assert.AreEqual(-1, y.Mantissa);
        Console.WriteLine(y);

        x = 6;
        y = BigDecimal.Reciprocal(x);
        Assert.AreEqual(12, y.Length);
        Assert.AreEqual(166666666, y.Significand[0]);
        Assert.AreEqual(666666666, y.Significand[1]);
        Assert.AreEqual(666666666, y.Significand[2]);
        Assert.AreEqual(666666666, y.Significand[3]);
        Assert.AreEqual(666666666, y.Significand[4]);
        Assert.AreEqual(666666666, y.Significand[5]);
        Assert.AreEqual(666666666, y.Significand[6]);
        Assert.AreEqual(666666666, y.Significand[7]);
        Assert.AreEqual(666666666, y.Significand[8]);
        Assert.AreEqual(666666666, y.Significand[9]);
        Assert.AreEqual(666666666, y.Significand[10]);
        Assert.AreEqual(666666667, y.Significand[11]);
        Assert.AreEqual(-1, y.Mantissa);
        Console.WriteLine(y);

        x = 7;
        y = BigDecimal.Reciprocal(x);
        Assert.AreEqual(12, y.Length);
        Assert.AreEqual(142857142, y.Significand[0]);
        Assert.AreEqual(857142857, y.Significand[1]);
        Assert.AreEqual(142857142, y.Significand[2]);
        Assert.AreEqual(857142857, y.Significand[3]);
        Assert.AreEqual(142857142, y.Significand[4]);
        Assert.AreEqual(857142857, y.Significand[5]);
        Assert.AreEqual(142857142, y.Significand[6]);
        Assert.AreEqual(857142857, y.Significand[7]);
        Assert.AreEqual(142857142, y.Significand[8]);
        Assert.AreEqual(857142857, y.Significand[9]);
        Assert.AreEqual(142857142, y.Significand[10]);
        Assert.AreEqual(857142857, y.Significand[11]);
        Assert.AreEqual(-1, y.Mantissa);
        Console.WriteLine(y);

        x = 8;
        y = BigDecimal.Reciprocal(x);
        Assert.AreEqual(1, y.Length);
        Assert.AreEqual(125000000, y.Significand[0]);
        Assert.AreEqual(-1, y.Mantissa);
        Console.WriteLine(y);

        x = 9;
        y = BigDecimal.Reciprocal(x);
        Assert.AreEqual(12, y.Length);
        Assert.AreEqual(111111111, y.Significand[0]);
        Assert.AreEqual(111111111, y.Significand[1]);
        Assert.AreEqual(111111111, y.Significand[2]);
        Assert.AreEqual(111111111, y.Significand[3]);
        Assert.AreEqual(111111111, y.Significand[4]);
        Assert.AreEqual(111111111, y.Significand[5]);
        Assert.AreEqual(111111111, y.Significand[6]);
        Assert.AreEqual(111111111, y.Significand[7]);
        Assert.AreEqual(111111111, y.Significand[8]);
        Assert.AreEqual(111111111, y.Significand[9]);
        Assert.AreEqual(111111111, y.Significand[10]);
        Assert.AreEqual(111111111, y.Significand[11]);
        Assert.AreEqual(-1, y.Mantissa);
        Console.WriteLine(y);
    }

    [TestMethod]
    public void RandomDivideTest()
    {
        double m1, m2, m3;
        BigDecimal bd1, bd2, bd3;
        Random rng = new ();
        double percent = 0.000001;
        int n = 100;

        // Create pairs of random decimals and multiply them.
        for (int i = 0; i < n; i++)
        {
            m1 = rng.NextDoubleFullRange();
            m2 = rng.NextDoubleFullRange();
            try
            {
                Console.WriteLine($"{m1} / {m2}");

                if (m2 == 0)
                {
                    throw new OverflowException("Division by zero, continue.");
                }

                m3 = m1 / m2;

                if (double.IsInfinity(m3))
                {
                    throw new OverflowException("Overflow, continue.");
                }

                bd1 = m1;
                bd2 = m2;
                bd3 = BigDecimal.Divide(bd1, bd2);

                Console.WriteLine($"Expected = {m3:G17}");
                Console.WriteLine($"Actual = {bd3}");
                double actualAsDouble = (double)bd3;
                Console.WriteLine($"Actual as double = {actualAsDouble:G17}");

                // Check if they agree up to a reasonable number of digits.
                XAssert.AreEqualPercent(m3, actualAsDouble, percent);
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow, continue.");
            }
            Console.WriteLine();
        }
    }

    [TestMethod]
    public void RandomReciprocalTest()
    {
        double m1, m2;
        BigDecimal bd1, bd2;
        Random rng = new ();
        // double percent = 0.001;
        int n = 100;

        // Create pairs of random decimals and multiply them.
        for (int i = 0; i < n; i++)
        {
            m1 = rng.NextDoubleFullRange();
            try
            {
                Console.WriteLine($"1 / {m1}");

                if (m1 == 0)
                {
                    throw new OverflowException("Division by zero, continue.");
                }

                m2 = 1 / m1;

                if (double.IsInfinity(m2))
                {
                    throw new OverflowException("Overflow, continue.");
                }

                bd1 = m1;
                bd2 = BigDecimal.Reciprocal(bd1);

                Console.WriteLine($"Expected = {m2:G17}");
                Console.WriteLine($"Actual = {bd2}");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow, continue.");
            }
            Console.WriteLine();
        }
    }

    [TestMethod]
    public void DivideGoldschmidtTest1()
    {
        BigDecimal bd1, bd2, bdResult;
        for (int j = 1; j <= 10; j++)
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"{i} / {j} = ");
                bd1 = i;
                bd2 = j;
                bdResult = BigDecimal.Divide(bd1, bd2);
                Console.WriteLine(bdResult);
                Console.WriteLine();
            }
        }
    }
}
