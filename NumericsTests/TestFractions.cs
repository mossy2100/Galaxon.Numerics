using System.Diagnostics;
using AstroMultimedia.Core.Numbers;
using AstroMultimedia.Numerics.Types;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestFractions
{
    [TestMethod]
    public void TestFindSingleDigit()
    {
        for (int n = 0; n < 10; n++)
        {
            for (int d = 1; d < 10; d++)
            {
                Fraction f = new(n, d);
                double x = (double)n / d;
                Fraction f2 = Fraction.Find(x);
                Trace.WriteLine($"Testing that {f} == {x}");
                Assert.AreEqual(f, f2);
            }
        }
    }

    [TestMethod]
    public void TestFindHalf()
    {
        double x = 0.5;
        Fraction f = Fraction.Find(x);
        Assert.AreEqual(1, f.Numerator);
        Assert.AreEqual(2, f.Denominator);
    }

    [TestMethod]
    public void TestFindThird()
    {
        double x = 0.333333333333333;
        Fraction f = Fraction.Find(x);
        Assert.AreEqual(1, f.Numerator);
        Assert.AreEqual(3, f.Denominator);
    }

    [TestMethod]
    public void TestFindRandom()
    {
        Random rnd = new();

        // Get a random numerator.
        int n = rnd.NextInt32();

        // Get a random denominator but not 0.
        int d = 0;
        while (d == 0)
        {
            d = rnd.NextInt32();
        }

        Fraction f = new(n, d);
        double x = (double)n / d;
        Fraction f2 = Fraction.Find(x);
        Trace.WriteLine($"f = {f}, x = {x}, f2 = {f2}");
        Assert.AreEqual(f, f2);
    }

    [TestMethod]
    public void TestFindPi()
    {
        double x = PI;
        Fraction f = Fraction.Find(x, 1e-9);
        double y = (double)f;
        Assert.AreEqual(x, y);
        Assert.AreEqual(245850922, f.Numerator);
        Assert.AreEqual(78256779, f.Denominator);
    }

    [TestMethod]
    public void TestFindPiLowerPrecision()
    {
        double x = PI;
        Fraction f = Fraction.Find(x, 1e-4);
        Assert.AreEqual(355, f.Numerator);
        Assert.AreEqual(113, f.Denominator);
    }

    [TestMethod]
    public void TestSimplifyRandom()
    {
        Random rnd = new();
        for (int i = 0; i < 10; i++)
        {
            while (true)
            {
                // Get a random numerator and denominator.
                int n = rnd.Next(1, 1000);
                int d = rnd.Next(1, 1000);

                Fraction f1 = new(n, d);

                if (f1.Numerator != n)
                {
                    // Fraction was simplified.
                    Trace.WriteLine($"{n}/{d} simplifies to {f1}");
                    break;
                }
            }
        }
    }

    [TestMethod]
    public void TestPowWithIntegerExponent()
    {
        Fraction f = new(2, 3);
        Fraction g = f ^ 2;
        Assert.AreEqual(4, g.Numerator);
        Assert.AreEqual(9, g.Denominator);
    }

    [TestMethod]
    public void TestPowWithNegativeOneExponent()
    {
        Fraction f = new(2, 3);
        Fraction g = f ^ -1;
        Assert.AreEqual(3, g.Numerator);
        Assert.AreEqual(2, g.Denominator);
    }

    [TestMethod]
    public void TestPowWithNegativeIntegerExponent()
    {
        Fraction f = new(2, 3);
        Fraction g = f ^ -2;
        Assert.AreEqual(9, g.Numerator);
        Assert.AreEqual(4, g.Denominator);
    }

    [TestMethod]
    public void TestPowWithFractionExponent()
    {
        Fraction f = new(4, 9);
        Fraction g = new(1, 2);
        Fraction h = f ^ g;
        Assert.AreEqual(2, h.Numerator);
        Assert.AreEqual(3, h.Denominator);
    }

    [TestMethod]
    public void TestPowWithDoubleExponent()
    {
        Fraction f = new(4, 9);
        double g = 0.5;
        Fraction h = f ^ g;
        Assert.AreEqual(2, h.Numerator);
        Assert.AreEqual(3, h.Denominator);
    }
}
