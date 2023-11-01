using System.Diagnostics;
using Galaxon.BigNumbers;
using Galaxon.Numerics.Algebra;

namespace Galaxon.Numerics.Tests;

[TestClass]
public class AnalysisTests
{
    /// <summary>
    /// This method just spits out the first 21 Bernoulli numbers, which can be compared with the
    /// Wikipedia page for correctness.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Bernoulli_number"/>
    [TestMethod]
    public void TestBernoulli1()
    {
        for (var i = 0; i <= 22; i++)
        {
            Trace.WriteLine($"Rationals.Bernoulli({i}) = {Analysis.Bernoulli(i)}");
        }
    }

    /// <summary>Check the first few Bernoulli numbers.</summary>
    [TestMethod]
    public void TestBernoulli2()
    {
        int n;
        BigRational expected;

        n = 0;
        expected = 1;
        Assert.AreEqual(expected, Analysis.Bernoulli(n));

        n = 1;
        expected = new BigRational(1, 2);
        Assert.AreEqual(expected, Analysis.Bernoulli(n));

        n = 2;
        expected = new BigRational(1, 6);
        Assert.AreEqual(expected, Analysis.Bernoulli(n));

        n = 3;
        expected = 0;
        Assert.AreEqual(expected, Analysis.Bernoulli(n));

        n = 4;
        expected = new BigRational(-1, 30);
        Assert.AreEqual(expected, Analysis.Bernoulli(n));

        n = 5;
        expected = 0;
        Assert.AreEqual(expected, Analysis.Bernoulli(n));

        n = 6;
        expected = new BigRational(1, 42);
        Assert.AreEqual(expected, Analysis.Bernoulli(n));

        n = 7;
        expected = 0;
        Assert.AreEqual(expected, Analysis.Bernoulli(n));

        n = 8;
        expected = new BigRational(-1, 30);
        Assert.AreEqual(expected, Analysis.Bernoulli(n));

        n = 9;
        expected = 0;
        Assert.AreEqual(expected, Analysis.Bernoulli(n));

        n = 10;
        expected = new BigRational(5, 66);
        Assert.AreEqual(expected, Analysis.Bernoulli(n));
    }
}
