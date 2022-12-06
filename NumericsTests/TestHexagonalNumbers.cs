using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestHexagonalNumbers
{
    [TestMethod]
    public void TestGet1()
    {
        Assert.AreEqual(1, Hexagonal.Get(1));
    }

    [TestMethod]
    public void TestGet5()
    {
        Assert.AreEqual(45, Hexagonal.Get(5));
    }

    [TestMethod]
    public void TestGetUpTo()
    {
        Dictionary<long, long> nums = Hexagonal.UpTo(100);
        Assert.AreEqual(1, nums[1]);
        Assert.AreEqual(6, nums[2]);
        Assert.AreEqual(15, nums[3]);
        Assert.AreEqual(28, nums[4]);
        Assert.AreEqual(45, nums[5]);
        Assert.AreEqual(66, nums[6]);
        Assert.AreEqual(91, nums[7]);
    }
}
