using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestPolygonalNumbers
{
    #region Triangular numbers

    [TestMethod]
    public void TestGetTriangular1()
    {
        Assert.AreEqual(1u, PolygonalNumbers.GetTriangular(1));
    }

    [TestMethod]
    public void TestGetTriangular5()
    {
        Assert.AreEqual(15u, PolygonalNumbers.GetTriangular(5));
    }

    [TestMethod]
    public void TestGetAllTriangularUpTo()
    {
        Dictionary<uint, uint> nums = PolygonalNumbers.GetAllTriangularUpTo(25);
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
        uint? n;
        Assert.IsTrue(PolygonalNumbers.IsTriangular(1, out n));
        Assert.IsTrue(PolygonalNumbers.IsTriangular(3, out n));
        Assert.IsTrue(PolygonalNumbers.IsTriangular(6, out n));
        Assert.IsTrue(PolygonalNumbers.IsTriangular(10, out n));
        Assert.IsTrue(PolygonalNumbers.IsTriangular(15, out n));
        Assert.IsTrue(PolygonalNumbers.IsTriangular(21, out n));

        Assert.IsFalse(PolygonalNumbers.IsTriangular(2, out n));
        Assert.IsFalse(PolygonalNumbers.IsTriangular(4, out n));
        Assert.IsFalse(PolygonalNumbers.IsTriangular(9, out n));
        Assert.IsFalse(PolygonalNumbers.IsTriangular(12, out n));
        Assert.IsFalse(PolygonalNumbers.IsTriangular(20, out n));
        Assert.IsFalse(PolygonalNumbers.IsTriangular(25, out n));
    }

    #endregion Triangular numbers

    #region Pentagonal numbers

    [TestMethod]
    public void TestGetPentagonal1()
    {
        Assert.AreEqual(1u, PolygonalNumbers.GetPentagonal(1));
    }

    [TestMethod]
    public void TestGetPentagonal5()
    {
        Assert.AreEqual(35u, PolygonalNumbers.GetPentagonal(5));
    }

    [TestMethod]
    public void TestGetAllPentagonalUpTo()
    {
        Dictionary<uint, uint> nums = PolygonalNumbers.GetAllPentagonalUpTo(60);
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
        uint? n;
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(1, out n));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(5, out n));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(12, out n));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(22, out n));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(35, out n));
        Assert.IsTrue(PolygonalNumbers.IsPentagonal(51, out n));

        Assert.IsFalse(PolygonalNumbers.IsPentagonal(2, out n));
        Assert.IsFalse(PolygonalNumbers.IsPentagonal(7, out n));
        Assert.IsFalse(PolygonalNumbers.IsPentagonal(15, out n));
        Assert.IsFalse(PolygonalNumbers.IsPentagonal(27, out n));
        Assert.IsFalse(PolygonalNumbers.IsPentagonal(38, out n));
        Assert.IsFalse(PolygonalNumbers.IsPentagonal(55, out n));
    }

    #endregion Pentagonal numbers

    #region Hexagonal numbers

    [TestMethod]
    public void TestGetHexagonal1()
    {
        Assert.AreEqual(1u, PolygonalNumbers.GetHexagonal(1));
    }

    [TestMethod]
    public void TestGetHexagonal5()
    {
        Assert.AreEqual(45u, PolygonalNumbers.GetHexagonal(5));
    }

    [TestMethod]
    public void TestGetAllHexagonalUpTo()
    {
        Dictionary<uint, uint> nums = PolygonalNumbers.GetAllHexagonalUpTo(100);
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
        uint? n;
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(1, out n));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(6, out n));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(15, out n));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(28, out n));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(45, out n));
        Assert.IsTrue(PolygonalNumbers.IsHexagonal(66, out n));

        Assert.IsFalse(PolygonalNumbers.IsHexagonal(2, out n));
        Assert.IsFalse(PolygonalNumbers.IsHexagonal(7, out n));
        Assert.IsFalse(PolygonalNumbers.IsHexagonal(18, out n));
        Assert.IsFalse(PolygonalNumbers.IsHexagonal(33, out n));
        Assert.IsFalse(PolygonalNumbers.IsHexagonal(56, out n));
        Assert.IsFalse(PolygonalNumbers.IsHexagonal(71, out n));
    }

    #endregion Hexagonal numbers
}
