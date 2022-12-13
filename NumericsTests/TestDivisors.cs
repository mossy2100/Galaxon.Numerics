using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestDivisors
{
    [TestMethod]
    public void TestGreatestCommonDivisor()
    {
        Assert.AreEqual(0, Divisors.GreatestCommonDivisor(0, 0));
        Assert.AreEqual(5, Divisors.GreatestCommonDivisor(5, 5));
        Assert.AreEqual(10, Divisors.GreatestCommonDivisor(10, 10));

        Assert.AreEqual(5, Divisors.GreatestCommonDivisor(5, 0));
        Assert.AreEqual(5, Divisors.GreatestCommonDivisor(0, 5));

        Assert.AreEqual(10, Divisors.GreatestCommonDivisor(10, 0));
        Assert.AreEqual(10, Divisors.GreatestCommonDivisor(0, 10));

        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(1, 5));
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(5, 1));

        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(1, 10));
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(10, 1));

        Assert.AreEqual(5, Divisors.GreatestCommonDivisor(10, 5));
        Assert.AreEqual(5, Divisors.GreatestCommonDivisor(5, 10));

        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(3, 7));
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(7, 3));

        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(4, 9));
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(9, 4));

        Assert.AreEqual(4, Divisors.GreatestCommonDivisor(4, 16));
        Assert.AreEqual(4, Divisors.GreatestCommonDivisor(16, 4));
    }
}
