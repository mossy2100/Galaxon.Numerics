using Galaxon.Numerics.Integers;

namespace AstroMtimedia.Numerics.Tests;

[TestClass]
public class TestDivisors
{
    [TestMethod]
    public void TestGreatestCommonDivisor()
    {
        // Equal values. 0, 1, prime, composite.
        Assert.AreEqual(0, Divisors.GreatestCommonDivisor(0, 0));
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(1, 1));
        Assert.AreEqual(5, Divisors.GreatestCommonDivisor(5, 5));
        Assert.AreEqual(10, Divisors.GreatestCommonDivisor(10, 10));

        // 0 and 1.
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(1, 0));
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(0, 1));

        // 0 and prime.
        Assert.AreEqual(5, Divisors.GreatestCommonDivisor(5, 0));
        Assert.AreEqual(5, Divisors.GreatestCommonDivisor(0, 5));

        // 0 and composite.
        Assert.AreEqual(10, Divisors.GreatestCommonDivisor(10, 0));
        Assert.AreEqual(10, Divisors.GreatestCommonDivisor(0, 10));

        // 1 and prime.
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(1, 5));
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(5, 1));

        // 1 and composite.
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(1, 10));
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(10, 1));

        // Prime and prime.
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(3, 7));
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(7, 3));

        // Prime and composite with a common factor.
        Assert.AreEqual(5, Divisors.GreatestCommonDivisor(10, 5));
        Assert.AreEqual(5, Divisors.GreatestCommonDivisor(5, 10));

        // Prime and composite without a common factor.
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(6, 5));
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(5, 6));

        // Composite and composite with a prime common factor.
        Assert.AreEqual(2, Divisors.GreatestCommonDivisor(4, 6));
        Assert.AreEqual(2, Divisors.GreatestCommonDivisor(6, 4));

        // Composite and composite with a composite common factor.
        Assert.AreEqual(4, Divisors.GreatestCommonDivisor(4, 16));
        Assert.AreEqual(4, Divisors.GreatestCommonDivisor(16, 4));

        // Composite and composite without a common factor.
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(4, 9));
        Assert.AreEqual(1, Divisors.GreatestCommonDivisor(9, 4));
    }
}
