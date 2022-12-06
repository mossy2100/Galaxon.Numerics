using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestPentagonalNumbers
{
    [TestMethod]
    public void TestGet1()
    {
        Assert.AreEqual(1, Pentagonal.Get(1));
    }

    [TestMethod]
    public void TestGet5()
    {
        Assert.AreEqual(35, Pentagonal.Get(5));
    }

    [TestMethod]
    public void TestGetUpTo()
    {
        Dictionary<long, long> nums = Pentagonal.UpTo(60);
        Assert.AreEqual(1, nums[1]);
        Assert.AreEqual(5, nums[2]);
        Assert.AreEqual(12, nums[3]);
        Assert.AreEqual(22, nums[4]);
        Assert.AreEqual(35, nums[5]);
        Assert.AreEqual(51, nums[6]);
    }
}
