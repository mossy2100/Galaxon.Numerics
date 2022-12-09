using AstroMultimedia.Numerics.Integers;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestFunctions
{
    [TestMethod]
    public void TestNumDigits1()
    {
        Assert.AreEqual(1, Functions.NumDigits(0));
        Assert.AreEqual(1, Functions.NumDigits(1));
        Assert.AreEqual(1, Functions.NumDigits(2));
        Assert.AreEqual(1, Functions.NumDigits(9));
        Assert.AreEqual(2, Functions.NumDigits(10));
        Assert.AreEqual(2, Functions.NumDigits(11));
        Assert.AreEqual(2, Functions.NumDigits(99));
        Assert.AreEqual(3, Functions.NumDigits(100));
    }

    [TestMethod]
    public void TestNumbersToWords()
    {
        Assert.AreEqual("zero", Functions.NumberToWords(0));
        Assert.AreEqual("one", Functions.NumberToWords(1));
        Assert.AreEqual("negative one", Functions.NumberToWords(-1));
        Assert.AreEqual("one hundred and twenty-three", Functions.NumberToWords(123));
        Assert.AreEqual("four thousand, three hundred and twenty-one",
            Functions.NumberToWords(4321));
        Assert.AreEqual("six million", Functions.NumberToWords(6_000_000));
    }

    [TestMethod]
    public void TestNumbersToWordsMin() =>
        // -9,223,372,036,854,775,808
        Assert.AreEqual("negative nine quintillion, "
            + "two hundred and twenty-three quadrillion, "
            + "three hundred and seventy-two trillion, "
            + "thirty-six billion, "
            + "eight hundred and fifty-four million, "
            + "seven hundred and seventy-five thousand, "
            + "eight hundred and eight", Functions.NumberToWords(long.MinValue));

    [TestMethod]
    public void TestNumbersToWordsMax() =>
        // 9,223,372,036,854,775,807
        Assert.AreEqual("nine quintillion, "
            + "two hundred and twenty-three quadrillion, "
            + "three hundred and seventy-two trillion, "
            + "thirty-six billion, "
            + "eight hundred and fifty-four million, "
            + "seven hundred and seventy-five thousand, "
            + "eight hundred and seven", Functions.NumberToWords(long.MaxValue));

    [TestMethod]
    public void TestNumDigits()
    {
        Assert.AreEqual(1, Functions.NumDigits(0));
        Assert.AreEqual(1, Functions.NumDigits(1));
        Assert.AreEqual(1, Functions.NumDigits(-1));
        Assert.AreEqual(1, Functions.NumDigits(9));
        Assert.AreEqual(1, Functions.NumDigits(-9));
        Assert.AreEqual(2, Functions.NumDigits(10));
        Assert.AreEqual(2, Functions.NumDigits(99));
        Assert.AreEqual(3, Functions.NumDigits(100));
        Assert.AreEqual(3, Functions.NumDigits(101));
        Assert.AreEqual(10, Functions.NumDigits(1000000000));
    }
}
