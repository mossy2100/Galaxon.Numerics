using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestTriangleNumbers
{
    [TestMethod]
    public void TestGet1()
    {
        Assert.AreEqual(1, Triangular.Get(1));
    }

    [TestMethod]
    public void TestGet5()
    {
        Assert.AreEqual(15, Triangular.Get(5));
    }

    [TestMethod]
    public void TestGetUpTo()
    {
        Dictionary<long, long> nums = Triangular.UpTo(25);
        Assert.AreEqual(1, nums[1]);
        Assert.AreEqual(3, nums[2]);
        Assert.AreEqual(6, nums[3]);
        Assert.AreEqual(10, nums[4]);
        Assert.AreEqual(15, nums[5]);
        Assert.AreEqual(21, nums[6]);
    }

    [TestMethod]
    public void TestIs()
    {
        Assert.IsTrue(Triangular.IsTriangular(1));
        Assert.IsFalse(Triangular.IsTriangular(2));
        Assert.IsTrue(Triangular.IsTriangular(3));
        Assert.IsFalse(Triangular.IsTriangular(4));
        Assert.IsTrue(Triangular.IsTriangular(6));
        Assert.IsFalse(Triangular.IsTriangular(9));
        Assert.IsTrue(Triangular.IsTriangular(10));
        Assert.IsFalse(Triangular.IsTriangular(12));
        Assert.IsTrue(Triangular.IsTriangular(15));
        Assert.IsFalse(Triangular.IsTriangular(20));
    }
}
