using System.Diagnostics;
using AstroMultimedia.Core.Time;
using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestPrimes
{
    [TestMethod]
    public void TestIsPrimeSlow()
    {
        long t1 = DateTime.Now.Ticks;

        // Test IsPrimeSlow() is correctly reporting whether or not a value is prime for all values
        // up to Primes.MaxValueChecked.
        for (uint i = 0; i <= Primes.MaxValueChecked; i++)
        {
            bool expected = Primes.Cache.Contains(i);
            bool actual = Primes.IsPrimeSlow(i);
            if (expected != actual)
            {
                Trace.WriteLine($"Error. i is prime? {expected} but the function says {actual}.");
            }
            Assert.AreEqual(expected, actual);
        }

        long t2 = DateTime.Now.Ticks;
        long t = t2 - t1;
        Trace.WriteLine($"Time taken: {t} ticks.");
    }

    [TestMethod]
    public void TestIsPrimeFast()
    {
        long t1 = DateTime.Now.Ticks;

        // Test IsPrimeFast() is correctly reporting whether or not a value is prime for all values
        // up to Primes.MaxValueChecked.
        for (uint i = 0; i <= Primes.MaxValueChecked; i++)
        {
            bool expected = Primes.Cache.Contains(i);
            bool actual = Primes.IsPrime(i);
            if (expected != actual)
            {
                Trace.WriteLine($"Error. i is prime? {expected} but the function says {actual}.");
            }
            Assert.AreEqual(expected, actual);
        }

        long t2 = DateTime.Now.Ticks;
        long t = t2 - t1;
        Trace.WriteLine($"Time taken: {t} ticks.");
    }

    public void CompareIsPrimeMethods(ulong n)
    {
        Trace.WriteLine("");

        Trace.WriteLine($"Testing IsPrimeSlow({n})...");
        Primes.ClearCache();
        long t3 = (long)(DateTime.Now.Ticks / Time.TICKS_PER_MICROSECOND);
        bool isPrimeSlow = Primes.IsPrimeSlow(n);
        long t4 = (long)(DateTime.Now.Ticks / Time.TICKS_PER_MICROSECOND);
        long tSlow = t4 - t3;
        Trace.WriteLine($"IsPrimeSlow(): {n} is " + (isPrimeSlow ? "" : "not ") + "prime.");
        Trace.WriteLine($"IsPrimeSlow() took {tSlow} µs.");

        Trace.WriteLine($"Testing IsPrime({n})...");
        Primes.ClearCache();
        long t1 = (long)(DateTime.Now.Ticks / Time.TICKS_PER_MICROSECOND);
        bool isPrimeFast = Primes.IsPrime(n);
        long t2 = (long)(DateTime.Now.Ticks / Time.TICKS_PER_MICROSECOND);
        long tFast = t2 - t1;
        Trace.WriteLine($"IsPrime(): {n} is " + (isPrimeFast ? "" : "not ") + "prime.");
        Trace.WriteLine($"IsPrime() took {tFast} µs.");

        Assert.AreEqual(isPrimeSlow, isPrimeFast);
    }

    [TestMethod]
    public void CompareIsPrimeMethods1MillionValues()
    {
        for (ulong i = 0; i < 1000000; i++)
        {
            CompareIsPrimeMethods(i);
        }
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxShort()
    {
        ulong n = (ulong)short.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest short prime is {n}");
        CompareIsPrimeMethods(n);
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxUShort()
    {
        ulong n = ushort.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest ushort prime is {n}");
        CompareIsPrimeMethods(n);
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxInt()
    {
        ulong n = int.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest int prime is {n}");
        CompareIsPrimeMethods(n);
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxUInt()
    {
        ulong n = uint.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest uint prime is {n}");
        CompareIsPrimeMethods(n);
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxLong()
    {
        ulong n = long.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest long prime is {n}");
        // IsPrimeSlow() takes too long.
        // CompareIsPrimeMethods(n);
    }

    [TestMethod]
    public void CompareIsPrimeMethodsMaxULong()
    {
        ulong n = ulong.MaxValue;
        if (!Primes.IsPrime(n))
        {
            n = Primes.GetPrevious(n);
        }
        Trace.WriteLine($"The largest ulong prime is {n}");
        // IsPrimeSlow() takes too long.
        // CompareIsPrimeMethods(n);
    }

    /// <summary>
    /// <see href="https://projecteuler.net/problem=10" />
    /// </summary>
    [TestMethod]
    public void PrimesSumTest()
    {
        List<ulong> primes = Primes.GetPrimesUpTo(2_000_000);
        ulong total = primes.Aggregate(0ul, (sum, item) => sum + item);
        Assert.AreEqual(142913828922ul, total);
    }
}
