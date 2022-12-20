using System.Diagnostics;
using AstroMultimedia.Numerics.Types;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestBigDecimalRound
{
    /// <summary>
    /// Check BigDecimal.Round() matches decimal.Round() for all rounding methods.
    /// </summary>
    [TestMethod]
    public void TestRoundingMethods()
    {
        decimal[] values =
        {
            -2.6m, -2.51m, -2.5m, -2.49m, -2.4m, -1.6m, -1.51m, -1.5m, -1.49m, -1.4m,
            1.4m, 1.49m, 1.5m, 1.51m, 1.6m, 2.4m, 2.49m, 2.5m, 2.51m, 2.6m
        };
        foreach (decimal value in values)
        {
            foreach (MidpointRounding method in Enum.GetValues<MidpointRounding>())
            {
                BigDecimal expected = decimal.Round(value, method);
                BigDecimal actual = BigDecimal.Round(value, 0, method);
                Assert.AreEqual(expected, actual);
            }
        }
    }

    [TestMethod]
    public void TestRoundPi()
    {
        BigDecimal pi = BigDecimal.Round(BigDecimal.Pi, 4);
        Assert.AreEqual(3.1416, pi);
    }
}
