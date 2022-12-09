using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestPolygonalNumbers
{
    [TestMethod]
    public void TestLargeNumber()
    {
        ulong n = 40755;
        Assert.IsTrue(PolygonalNumbers.IsTriangular(n));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(n));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(n));

        n = 1533776805;
        Assert.IsTrue(PolygonalNumbers.IsTriangular(n));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(n));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(n));
    }

    #region Triangular numbers

    [TestMethod]
    public void TestGetTriangular1() =>
        Assert.AreEqual(1u, PolygonalNumbers.GetTriangular(1));

    [TestMethod]
    public void TestGetTriangular5() =>
        Assert.AreEqual(15u, PolygonalNumbers.GetTriangular(5));

    [TestMethod]
    public void TestGetAllTriangularUpTo()
    {
        Dictionary<ulong, ulong> nums = PolygonalNumbers.GetAllTriangularUpTo(25);
        Assert.AreEqual(1u, nums[1]);
        Assert.AreEqual(3u, nums[2]);
        Assert.AreEqual(6u, nums[3]);
        Assert.AreEqual(10u, nums[4]);
        Assert.AreEqual(15u, nums[5]);
        Assert.AreEqual(21u, nums[6]);
    }

    [TestMethod]
    public void TestIsTriangular()
    {
        Assert.IsTrue(PolygonalNumbers.IsTriangular(1));
        Assert.IsTrue(PolygonalNumbers.IsTriangular(3));
        Assert.IsTrue(PolygonalNumbers.IsTriangular(6));
        Assert.IsTrue(PolygonalNumbers.IsTriangular(10));
        Assert.IsTrue(PolygonalNumbers.IsTriangular(15));
        Assert.IsTrue(PolygonalNumbers.IsTriangular(21));

        Assert.IsFalse(PolygonalNumbers.IsTriangular(2));
        Assert.IsFalse(PolygonalNumbers.IsTriangular(4));
        Assert.IsFalse(PolygonalNumbers.IsTriangular(9));
        Assert.IsFalse(PolygonalNumbers.IsTriangular(12));
        Assert.IsFalse(PolygonalNumbers.IsTriangular(20));
        Assert.IsFalse(PolygonalNumbers.IsTriangular(25));
    }

    #endregion Triangular numbers

    #region Pentagonal numbers

    [TestMethod]
    public void TestGetPentagonal1() =>
        Assert.AreEqual(1u, PolygonalNumbers.GetPentagonal(1));

    [TestMethod]
    public void TestGetPentagonal5() =>
        Assert.AreEqual(35u, PolygonalNumbers.GetPentagonal(5));

    [TestMethod]
    public void TestGetAllPentagonalUpTo()
    {
        Dictionary<ulong, ulong> nums = PolygonalNumbers.GetAllPentagonalUpTo(60);
        Assert.AreEqual(1u, nums[1]);
        Assert.AreEqual(5u, nums[2]);
        Assert.AreEqual(12u, nums[3]);
        Assert.AreEqual(22u, nums[4]);
        Assert.AreEqual(35u, nums[5]);
        Assert.AreEqual(51u, nums[6]);
    }

    [TestMethod]
    public void TestIsPentagonal()
    {
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(1));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(5));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(12));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(22));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(35));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(51));

        Assert.IsFalse(PolygonalNumbers.IsPentagonal(2));
        Assert.IsFalse(PolygonalNumbers.IsPentagonal(7));
        Assert.IsFalse(PolygonalNumbers.IsPentagonal(15));
        Assert.IsFalse(PolygonalNumbers.IsPentagonal(27));
        Assert.IsFalse(PolygonalNumbers.IsPentagonal(38));
        Assert.IsFalse(PolygonalNumbers.IsPentagonal(55));
    }

    #endregion Pentagonal numbers

    #region Hexagonal numbers

    [TestMethod]
    public void TestGetHexagonal1() =>
        Assert.AreEqual(1u, PolygonalNumbers.GetHexagonal(1));

    [TestMethod]
    public void TestGetHexagonal5() =>
        Assert.AreEqual(45u, PolygonalNumbers.GetHexagonal(5));

    [TestMethod]
    public void TestGetAllHexagonalUpTo()
    {
        Dictionary<ulong, ulong> nums = PolygonalNumbers.GetAllHexagonalUpTo(100);
        Assert.AreEqual(1u, nums[1]);
        Assert.AreEqual(6u, nums[2]);
        Assert.AreEqual(15u, nums[3]);
        Assert.AreEqual(28u, nums[4]);
        Assert.AreEqual(45u, nums[5]);
        Assert.AreEqual(66u, nums[6]);
        Assert.AreEqual(91u, nums[7]);
    }

    [TestMethod]
    public void TestIsHexagonal()
    {
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(1));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(6));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(15));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(28));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(45));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(66));

        Assert.IsFalse(PolygonalNumbers.IsHexagonal(2));
        Assert.IsFalse(PolygonalNumbers.IsHexagonal(7));
        Assert.IsFalse(PolygonalNumbers.IsHexagonal(18));
        Assert.IsFalse(PolygonalNumbers.IsHexagonal(33));
        Assert.IsFalse(PolygonalNumbers.IsHexagonal(56));
        Assert.IsFalse(PolygonalNumbers.IsHexagonal(71));
    }

    #endregion Hexagonal numbers
}
