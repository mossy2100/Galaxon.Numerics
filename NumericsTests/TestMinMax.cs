using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestMinMax
{
    [TestMethod]
    public void TestMax()
    {
        // byte
        Assert.AreEqual(200, Functions.Max(128, 9, 200, 13, 1, 0));
        // int
        Assert.AreEqual(12345, Functions.Max(-100, -45, 1000, 4, 12345, 0, 9020));
        // double
        Assert.AreEqual(123.2, Functions.Max(45.1, 9.333, 123.2));
        // decimal
        Assert.AreEqual(98383m, Functions.Max(-88m, 133, -1231.008m, 98383m, 176.3m));
        // string
        Assert.AreEqual("cat", Functions.Max("apple", "cat", "ball"));
    }

    [TestMethod]
    public void TestMin()
    {
        // byte
        Assert.AreEqual(0, Functions.Min(128, 9, 200, 13, 1, 0));
        // int
        Assert.AreEqual(-100, Functions.Min(-100, -45, 1000, 4, 12345, 0, 9020));
        // double
        Assert.AreEqual(9.333, Functions.Min(45.1, 9.333, 123.2));
        // decimal
        Assert.AreEqual(-1231.008m, Functions.Min(-88m, 133, -1231.008m, 98383m, 176.3m));
        // string
        Assert.AreEqual("apple", Functions.Min("apple", "cat", "ball"));
    }
}
