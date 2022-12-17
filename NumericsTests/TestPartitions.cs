using System.Numerics;
using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestPartitions
{
    [TestMethod]
    public void TestPDigits()
    {
        Assert.AreEqual(1, Partitions.P(0));
        Assert.AreEqual(1, Partitions.P(1));
        Assert.AreEqual(2, Partitions.P(2));
        Assert.AreEqual(3, Partitions.P(3));
        Assert.AreEqual(5, Partitions.P(4));
        Assert.AreEqual(7, Partitions.P(5));
        Assert.AreEqual(11, Partitions.P(6));
        Assert.AreEqual(15, Partitions.P(7));
        Assert.AreEqual(22, Partitions.P(8));
        Assert.AreEqual(30, Partitions.P(9));
    }

    [TestMethod]
    public void TestPTens()
    {
        Assert.AreEqual(42, Partitions.P(10));
        Assert.AreEqual(627, Partitions.P(20));
        Assert.AreEqual(5604, Partitions.P(30));
        Assert.AreEqual(37338, Partitions.P(40));
        Assert.AreEqual(204226, Partitions.P(50));
        Assert.AreEqual(966467, Partitions.P(60));
        Assert.AreEqual(4087968, Partitions.P(70));
        Assert.AreEqual(15796476, Partitions.P(80));
        Assert.AreEqual(56634173, Partitions.P(90));
        Assert.AreEqual(190569292, Partitions.P(100));
    }

    [TestMethod]
    public void TestPMax()
    {
        Assert.AreEqual(BigInteger.Parse("3358820431621906842444652576019869561880154987282714785689959242153335047317141722712232881134123626397327882626402334821236305901626530090286934670462497754264110840157018028678882085565859616856815577941255115159444661974214367098779658278272116033402923541053858868675003866332"),
            Partitions.P(ushort.MaxValue));
    }
}
