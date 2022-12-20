using System.Diagnostics;
using AstroMultimedia.Core.Strings;
using AstroMultimedia.Numerics.Types;

namespace AstroMultimedia.Numerics.Tests;

[TestClass]
public class TestBigDecimalToString
{
    [TestMethod]
    public void TestToStringI()
    {
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("I");
        Assert.AreEqual("12345", str);

        bd = 12345e67;
        str = bd.ToString("I");
        Assert.AreEqual("12345E+67", str);

        bd = 12345e-67;
        str = bd.ToString("I");
        Assert.AreEqual("12345E-67", str);

        bd = -12345e67;
        str = bd.ToString("I");
        Assert.AreEqual("-12345E+67", str);

        bd = -12345e-67;
        str = bd.ToString("I");
        Assert.AreEqual("-12345E-67", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("I");
        Assert.AreEqual("31415926535897932384626433832795028841971693993751058209749445923078164" +
            "062862089986280348253421170679E-100", str);
    }

    [TestMethod]
    public void TestToStringIU()
    {
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("IU");
        Assert.AreEqual("12345", str);

        bd = 12345e67;
        str = bd.ToString("IU");
        Assert.AreEqual("12345×10⁶⁷", str);

        bd = 12345e-67;
        str = bd.ToString("IU");
        Assert.AreEqual("12345×10⁻⁶⁷", str);

        bd = -12345e67;
        str = bd.ToString("IU");
        Assert.AreEqual("-12345×10⁶⁷", str);

        bd = -12345e-67;
        str = bd.ToString("IU");
        Assert.AreEqual("-12345×10⁻⁶⁷", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("IU");
        Assert.AreEqual("31415926535897932384626433832795028841971693993751058209749445923078164"
            + "062862089986280348253421170679×10⁻¹⁰⁰", str);
    }

    [TestMethod]
    public void TestToStringE()
    {
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("E");
        Assert.AreEqual("1.2345E+4", str);

        bd = 12345e67;
        str = bd.ToString("E");
        Assert.AreEqual("1.2345E+71", str);

        bd = 12345e-67;
        str = bd.ToString("E");
        Assert.AreEqual("1.2345E-63", str);

        bd = -12345e67;
        str = bd.ToString("E");
        Assert.AreEqual("-1.2345E+71", str);

        bd = -12345e-67;
        str = bd.ToString("E");
        Assert.AreEqual("-1.2345E-63", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("E");
        Assert.AreEqual("3.1415926535897932384626433832795028841971693993751058209749445923078164"
            + "062862089986280348253421170679E+0", str);
    }

    [TestMethod]
    public void TestToStringE3()
    {
        BigDecimal bd;
        string str;

        bd = 1.2345;
        str = bd.ToString("E3");
        Assert.AreEqual("1.234E+0", str);

        bd = 12345;
        str = bd.ToString("E3");
        Assert.AreEqual("1.234E+4", str);

        bd = 12345e67;
        str = bd.ToString("E3");
        Assert.AreEqual("1.234E+71", str);

        bd = 12345e-67;
        str = bd.ToString("E3");
        Assert.AreEqual("1.234E-63", str);

        bd = -12345e67;
        str = bd.ToString("E3");
        Assert.AreEqual("-1.234E+71", str);

        bd = -12345e-67;
        str = bd.ToString("E3");
        Assert.AreEqual("-1.234E-63", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("E3");
        Assert.AreEqual("3.142E+0", str);
    }

    [TestMethod]
    public void TestToStringEU()
    {
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("EU");
        Assert.AreEqual("1.2345×10" + "4".ToSuperscript(), str);

        bd = 12345e67;
        str = bd.ToString("EU");
        Assert.AreEqual("1.2345×10" + "71".ToSuperscript(), str);

        bd = 12345e-67;
        str = bd.ToString("EU");
        Assert.AreEqual("1.2345×10" + "-63".ToSuperscript(), str);

        bd = -12345e67;
        str = bd.ToString("EU");
        Assert.AreEqual("-1.2345×10" + "71".ToSuperscript(), str);

        bd = -12345e-67;
        str = bd.ToString("EU");
        Assert.AreEqual("-1.2345×10" + "-63".ToSuperscript(), str);

        bd = BigDecimal.Pi;
        str = bd.ToString("EU");
        Assert.AreEqual("3.1415926535897932384626433832795028841971693993751058209749445923078164"
            + "062862089986280348253421170679×10" + "0".ToSuperscript(), str);
    }

    [TestMethod]
    public void TestToStringE3U()
    {
        BigDecimal bd;
        string str;

        bd = 1.2345;
        str = bd.ToString("E3U");
        Assert.AreEqual("1.234×10" + "0".ToSuperscript(), str);

        bd = 12345;
        str = bd.ToString("E3U");
        Assert.AreEqual("1.234×10" + "4".ToSuperscript(), str);

        bd = 12345e67;
        str = bd.ToString("E3U");
        Assert.AreEqual("1.234×10" + "71".ToSuperscript(), str);

        bd = 12345e-67;
        str = bd.ToString("E3U");
        Assert.AreEqual("1.234×10" + "-63".ToSuperscript(), str);

        bd = -12345e67;
        str = bd.ToString("E3U");
        Assert.AreEqual("-1.234×10" + "71".ToSuperscript(), str);

        bd = -12345e-67;
        str = bd.ToString("E3U");
        Assert.AreEqual("-1.234×10" + "-63".ToSuperscript(), str);

        bd = BigDecimal.Pi;
        str = bd.ToString("E3U");
        Assert.AreEqual("3.142×10" + "0".ToSuperscript(), str);
    }

    [TestMethod]
    public void TestGetDigitStrings()
    {
        BigDecimal bd;
        string before, after;

        bd = 12345;
        (before, after) = bd.GetDigitStrings();
        Assert.AreEqual("12345", before);
        Assert.AreEqual("", after);

        bd = 12345e2;
        (before, after) = bd.GetDigitStrings();
        Assert.AreEqual("1234500", before);
        Assert.AreEqual("", after);

        bd = 12345e-5;
        (before, after) = bd.GetDigitStrings();
        Assert.AreEqual("0", before);
        Assert.AreEqual("12345", after);

        bd = 12345e-7;
        (before, after) = bd.GetDigitStrings();
        Assert.AreEqual("0", before);
        Assert.AreEqual("0012345", after);

        bd = 12345e-2;
        (before, after) = bd.GetDigitStrings();
        Assert.AreEqual("123", before);
        Assert.AreEqual("45", after);
    }

    [TestMethod]
    public void TestToStringF()
    {
        BigDecimal bd;
        string str;

        bd = 1.2345;
        str = bd.ToString("F");
        Assert.AreEqual("1.2345", str);

        bd = 12345;
        str = bd.ToString("F");
        Assert.AreEqual("12345", str);

        bd = 12345e2;
        str = bd.ToString("F");
        Assert.AreEqual("1234500", str);

        bd = 1.2345e7;
        str = bd.ToString("F");
        Assert.AreEqual("12345000", str);

        bd = 1.2345e-2;
        str = bd.ToString("F");
        Assert.AreEqual("0.012345", str);

        bd = 12345e-6;
        str = bd.ToString("F");
        Assert.AreEqual("0.012345", str);

        bd = -12345e2;
        str = bd.ToString("F");
        Assert.AreEqual("-1234500", str);

        bd = -12345e-6;
        str = bd.ToString("F");
        Assert.AreEqual("-0.012345", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("F");
        Assert.AreEqual("3."
            + "14159265358979323846264338327950288419716939937510"
            + "58209749445923078164062862089986280348253421170679", str);
    }

    [TestMethod]
    public void TestToStringF3()
    {
        BigDecimal bd;
        string str;

        bd = 1.2345;
        str = bd.ToString("F3");
        Assert.AreEqual("1.234", str);

        bd = 123.45;
        str = bd.ToString("F3");
        Assert.AreEqual("123.450", str);

        bd = 12345;
        str = bd.ToString("F3");
        Assert.AreEqual("12345.000", str);

        bd = 12345e2;
        str = bd.ToString("F3");
        Assert.AreEqual("1234500.000", str);

        bd = 1.2345e7;
        str = bd.ToString("F3");
        Assert.AreEqual("12345000.000", str);

        bd = 1.2345e-2;
        str = bd.ToString("F3");
        Assert.AreEqual("0.012", str);

        bd = 12345e-6;
        str = bd.ToString("F3");
        Assert.AreEqual("0.012", str);

        bd = -12345e2;
        str = bd.ToString("F3");
        Assert.AreEqual("-1234500.000", str);

        bd = -12345e-6;
        str = bd.ToString("F3");
        Assert.AreEqual("-0.012", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("F3");
        Assert.AreEqual("3.142", str);
    }

    [TestMethod]
    public void TestToStringF0()
    {
        BigDecimal bd;
        string str;

        bd = 1.2345;
        str = bd.ToString("F0");
        Assert.AreEqual("1", str);

        bd = 12345.5;
        str = bd.ToString("F0");
        Assert.AreEqual("12346", str);

        bd = 123.45;
        str = bd.ToString("F0");
        Assert.AreEqual("123", str);

        bd = 12345;
        str = bd.ToString("F0");
        Assert.AreEqual("12345", str);

        bd = 12345e2;
        str = bd.ToString("F0");
        Assert.AreEqual("1234500", str);

        bd = 1.2345e7;
        str = bd.ToString("F0");
        Assert.AreEqual("12345000", str);

        bd = 1.2345e-2;
        str = bd.ToString("F0");
        Assert.AreEqual("0", str);

        bd = 12345e-6;
        str = bd.ToString("F0");
        Assert.AreEqual("0", str);

        bd = -12345e2;
        str = bd.ToString("F0");
        Assert.AreEqual("-1234500", str);

        bd = -12345e-6;
        str = bd.ToString("F0");
        Assert.AreEqual("0", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("F0");
        Assert.AreEqual("3", str);

        bd = BigDecimal.E;
        str = bd.ToString("F0");
        Assert.AreEqual("3", str);
    }

    [TestMethod]
    public void TestToStringG()
    {
        BigDecimal bd;
        string strE, strF, strG;

        bd = 1.2345;
        strF = bd.ToString("F");
        strG = bd.ToString("G");
        Assert.AreEqual(strF, strG);

        bd = 12345;
        strF = bd.ToString("F");
        strG = bd.ToString("G");
        Assert.AreEqual(strF, strG);

        bd = 12345e2;
        strF = bd.ToString("F");
        strG = bd.ToString("G");
        Assert.AreEqual(strF, strG);

        bd = 1.2345e7;
        strF = bd.ToString("F");
        strG = bd.ToString("G");
        Assert.AreEqual(strF, strG);

        bd = 1.2345e-2;
        strF = bd.ToString("F");
        strG = bd.ToString("G");
        Assert.AreEqual(strF, strG);

        bd = 12345e-6;
        strF = bd.ToString("F");
        strG = bd.ToString("G");
        Assert.AreEqual(strF, strG);

        bd = -12345e2;
        strF = bd.ToString("F");
        strG = bd.ToString("G");
        Assert.AreEqual(strF, strG);

        bd = -12345e-6;
        strF = bd.ToString("F");
        strG = bd.ToString("G");
        Assert.AreEqual(strF, strG);

        bd = BigDecimal.Pi;
        strF = bd.ToString("F");
        strG = bd.ToString("G");
        Assert.AreEqual(strF, strG);

        bd = 12345e67;
        strE = bd.ToString("E");
        strG = bd.ToString("G");
        Assert.AreEqual(strE, strG);

        bd = 12345e-67;
        strE = bd.ToString("E");
        strG = bd.ToString("G");
        Assert.AreEqual(strE, strG);

        bd = -12345e67;
        strE = bd.ToString("E");
        strG = bd.ToString("G");
        Assert.AreEqual(strE, strG);

        bd = -12345e-67;
        strE = bd.ToString("E");
        strG = bd.ToString("G");
        Assert.AreEqual(strE, strG);
    }

    [TestMethod]
    public void TestToStringG3()
    {
        BigDecimal bd;
        string strE, strF, strG;

        bd = 1.2345;
        strF = bd.ToString("F3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strF, strG);

        bd = 12345;
        strF = bd.ToString("E3");
        strF = bd.ToString("F3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strF, strG);

        bd = 12345e2;
        strF = bd.ToString("F3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strF, strG);

        bd = 1.2345e7;
        strF = bd.ToString("F3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strF, strG);

        bd = 1.2345e-2;
        strF = bd.ToString("F3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strF, strG);

        bd = 12345e-6;
        strF = bd.ToString("F3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strF, strG);

        bd = -12345e2;
        strF = bd.ToString("F3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strF, strG);

        bd = -12345e-6;
        strF = bd.ToString("F3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strF, strG);

        bd = BigDecimal.Pi;
        strF = bd.ToString("F3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strF, strG);

        bd = 12345e67;
        strE = bd.ToString("E3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strE, strG);

        bd = 12345e-67;
        strE = bd.ToString("E3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strE, strG);

        bd = -12345e67;
        strE = bd.ToString("E3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strE, strG);

        bd = -12345e-67;
        strE = bd.ToString("E3");
        strG = bd.ToString("G3");
        Assert.AreEqual(strE, strG);
    }

    [TestMethod]
    public void TestToStringGU()
    {
        BigDecimal bd;
        string strE, strF, strG;

        bd = 1.2345;
        strF = bd.ToString("F");
        strG = bd.ToString("GU");
        Assert.AreEqual(strF, strG);

        bd = 12345;
        strF = bd.ToString("F");
        strG = bd.ToString("GU");
        Assert.AreEqual(strF, strG);

        bd = 12345e2;
        strF = bd.ToString("F");
        strG = bd.ToString("GU");
        Assert.AreEqual(strF, strG);

        bd = 1.2345e7;
        strF = bd.ToString("F");
        strG = bd.ToString("GU");
        Assert.AreEqual(strF, strG);

        bd = 1.2345e-2;
        strF = bd.ToString("F");
        strG = bd.ToString("GU");
        Assert.AreEqual(strF, strG);

        bd = 12345e-6;
        strF = bd.ToString("F");
        strG = bd.ToString("GU");
        Assert.AreEqual(strF, strG);

        bd = -12345e2;
        strF = bd.ToString("F");
        strG = bd.ToString("GU");
        Assert.AreEqual(strF, strG);

        bd = -12345e-6;
        strF = bd.ToString("F");
        strG = bd.ToString("GU");
        Assert.AreEqual(strF, strG);

        bd = 12345e67;
        strE = bd.ToString("EU");
        strG = bd.ToString("GU");
        Assert.AreEqual(strE, strG);

        bd = 12345e-67;
        strE = bd.ToString("EU");
        strG = bd.ToString("GU");
        Assert.AreEqual(strE, strG);

        bd = -12345e67;
        strE = bd.ToString("EU");
        strG = bd.ToString("GU");
        Assert.AreEqual(strE, strG);

        bd = -12345e-67;
        strE = bd.ToString("EU");
        strG = bd.ToString("GU");
        Assert.AreEqual(strE, strG);

        bd = BigDecimal.Pi;
        strF = bd.ToString("F");
        strG = bd.ToString("GU");
        Assert.AreEqual(strF, strG);
    }
}
