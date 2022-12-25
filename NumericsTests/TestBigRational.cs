using System.Diagnostics;
using Galaxon.Core.Numbers;
using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class TestBigRational
{
    [TestMethod]
    public void TestImplicitCastFromInt()
    {
        int a = 5;
        BigRational f = a;
        Assert.AreEqual(5, f.Numerator);
        Assert.AreEqual(1, f.Denominator);
    }

    [TestMethod]
    public void TestFindSingleDigit()
    {
        for (int n = 0; n < 10; n++)
        {
            for (int d = 1; d < 10; d++)
            {
                BigRational f = new (n, d);
                double x = (double)n / d;
                BigRational f2 = BigRational.Find(x);
                Trace.WriteLine($"Testing that {f} == {x}");
                Assert.AreEqual(f, f2);
            }
        }
    }

    [TestMethod]
    public void TestFindHalf()
    {
        double x = 0.5;
        BigRational f = BigRational.Find(x);
        Assert.AreEqual(1, f.Numerator);
        Assert.AreEqual(2, f.Denominator);
    }

    [TestMethod]
    public void TestFindThird()
    {
        double x = 0.333333333333333;
        BigRational f = BigRational.Find(x);
        Assert.AreEqual(1, f.Numerator);
        Assert.AreEqual(3, f.Denominator);
    }

    [TestMethod]
    public void TestFindRandom()
    {
        Random rnd = new ();

        // Get a random numerator.
        int n = rnd.NextInt32();

        // Get a random denominator but not 0.
        int d = 0;
        while (d == 0)
        {
            d = rnd.NextInt32();
        }

        BigRational f = new (n, d);
        double x = (double)n / d;
        BigRational f2 = BigRational.Find(x);
        Trace.WriteLine($"f = {f}, x = {x}, f2 = {f2}");
        Assert.AreEqual(f, f2);
    }

    [TestMethod]
    public void TestFindPi()
    {
        double x = PI;
        BigRational f = BigRational.Find(x);
        double y = (double)f;
        Assert.AreEqual(x, y);
        Assert.AreEqual(245850922, f.Numerator);
        Assert.AreEqual(78256779, f.Denominator);
    }

    [TestMethod]
    public void TestFindPiLowerPrecision()
    {
        double x = PI;
        BigRational f = BigRational.Find(x, 1e-4);
        Assert.AreEqual(355, f.Numerator);
        Assert.AreEqual(113, f.Denominator);
    }

    [TestMethod]
    public void TestPowWithIntegerExponent()
    {
        BigRational f = new (2, 3);
        BigRational g = f ^ 2;
        Assert.AreEqual(4, g.Numerator);
        Assert.AreEqual(9, g.Denominator);
    }

    [TestMethod]
    public void TestPowWithNegativeOneExponent()
    {
        BigRational f = new (2, 3);
        BigRational g = f ^ -1;
        Assert.AreEqual(3, g.Numerator);
        Assert.AreEqual(2, g.Denominator);
    }

    [TestMethod]
    public void TestPowWithNegativeIntegerExponent()
    {
        BigRational f = new (2, 3);
        BigRational g = f ^ -2;
        Assert.AreEqual(9, g.Numerator);
        Assert.AreEqual(4, g.Denominator);
    }

    [TestMethod]
    public void TestPowWithBigRationalExponent()
    {
        BigRational f = new (4, 9);
        BigRational g = new (1, 2);
        BigRational h = f ^ g;
        Assert.AreEqual(2, h.Numerator);
        Assert.AreEqual(3, h.Denominator);
    }

    [TestMethod]
    public void TestPowWithDoubleExponent()
    {
        BigRational f = new (4, 9);
        double g = 0.5;
        BigRational h = f ^ g;
        Assert.AreEqual(2, h.Numerator);
        Assert.AreEqual(3, h.Denominator);
    }

    [TestMethod]
    public void TestToString()
    {
        BigRational f = new (3, 4);
        Assert.AreEqual("3/4", f.ToString("A"));
        Assert.AreEqual("³/₄", f.ToString("U"));
        Assert.AreEqual("³/₄", f.ToString("M"));

        f += 2;
        Assert.AreEqual("11/4", f.ToString("A"));
        Assert.AreEqual("¹¹/₄", f.ToString("U"));
        Assert.AreEqual("2³/₄", f.ToString("M"));
    }
}
