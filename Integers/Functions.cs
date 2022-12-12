using System.Numerics;
using System.Text;
using AstroMultimedia.Core.Exceptions;
using AstroMultimedia.Core.Numbers;

namespace AstroMultimedia.Numerics.Integers;

public static class Functions
{
    public static readonly Func<BigInteger, List<BigInteger>> GetProperDivisors =
        Memoize<BigInteger, List<BigInteger>>(_GetProperDivisors);

    public static readonly Func<long, List<long>> Collatz = Memoize<long, List<long>>(_Collatz);

    /// <summary>
    /// Get the maximum value from a series of values.
    /// Normally this method would be used with numbers, but it can be used for any IComparable.
    /// In C# 11 we might be able to use INumber, but then it couldn't be used with strings or other
    /// IComparable.
    /// </summary>
    /// <param name="values">A series of values.</param>
    /// <returns>The maximum value.</returns>
    /// <exception cref="ArgumentInvalidException">If no values were provided.</exception>
    public static T Max<T>(params T[] values) where T : IComparable
    {
        switch (values.Length)
        {
            case 0:
                throw new ArgumentInvalidException(nameof(values),
                    "At least one value must be provided.");

            case 1:
                return values[0];

            default:
                T max = values[0];
                for (int i = 1; i < values.Length; i++)
                {
                    if (values[i].CompareTo(max) == 1)
                    {
                        max = values[i];
                    }
                }
                return max;
        }
    }

    /// <summary>
    /// Get the minimum value from a series of values.
    /// Normally this method would be used with numbers, but it can be used for
    /// any IComparable.
    /// </summary>
    /// <param name="values">A series of values.</param>
    /// <returns>The minimum value.</returns>
    /// <exception cref="ArgumentInvalidException">If no values were provided.</exception>
    public static T Min<T>(params T[] values) where T : IComparable
    {
        switch (values.Length)
        {
            case 0:
                throw new ArgumentInvalidException(nameof(values),
                    "At least one value must be provided.");

            case 1:
                return values[0];

            default:
                T min = values[0];
                for (int i = 1; i < values.Length; i++)
                {
                    if (values[i].CompareTo(min) == -1)
                    {
                        min = values[i];
                    }
                }
                return min;
        }
    }

    /// <summary>
    /// Enables caching of the results of pure functions.
    /// </summary>
    /// <param name="f">The pure function.</param>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <returns>The memoized version of the pure function.</returns>
    public static Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> f) where T : notnull
    {
        Dictionary<T, TResult> cache = new ();
        return x =>
        {
            if (cache.TryGetValue(x, out TResult? result))
            {
                return result;
            }
            result = f(x);
            cache.Add(x, result);
            return result;
        };
    }

    /// <summary>
    /// Get the list of proper divisors of an integer.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static List<BigInteger> _GetProperDivisors(BigInteger n)
    {
        // Guard.
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");
        }

        List<BigInteger> divisors = new ();

        // Look for divisors up to the square root.
        for (BigInteger i = 1; i <= XBigInteger.Sqrt(n); i++)
        {
            if (n % i != 0)
            {
                continue;
            }

            divisors.Add(i);
            BigInteger j = n / i;
            if (j != i && j < n)
            {
                divisors.Add(j);
            }
        }

        divisors.Sort();
        return divisors;
    }

    public static List<BigInteger> GetDivisors(BigInteger n)
    {
        List<BigInteger> divisors = GetProperDivisors(n);
        divisors.Add(n);
        return divisors;
    }

    public static BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b)
    {
        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);

        // Optimizations.
        if (a == 0 && b == 0)
        {
            return 0;
        }

        if (a == 0)
        {
            return b;
        }

        if (b == 0 || a == b)
        {
            return a;
        }

        if (a == 1 || b == 1
            || (a <= ulong.MaxValue && Primes.IsPrime((ulong)a))
            || (b <= ulong.MaxValue && Primes.IsPrime((ulong)b)))
        {
            return 1;
        }

        // Make a < b.
        if (a > b)
        {
            (a, b) = (b, a);
        }

        // Optimisation. See if a (the smaller number) evenly divides b (the larger).
        if (a != 0 && b % a == 0)
        {
            return a;
        }

        // Check divisors of a up to the square root.
        BigInteger sqrt = (BigInteger)Floor(Sqrt((double)a));
        for (BigInteger d = 2; d <= sqrt; d++)
        {
            if (a % d != 0)
            {
                continue;
            }

            // d is the small divisor of a, get the matching larger one.
            BigInteger c = a / d;

            // See if it also divides b.
            if (b % c == 0)
            {
                // Got it.
                return c;
            }
        }

        // None found.
        return 1;
    }

    public static BigInteger LeastCommonMultiple(BigInteger a, BigInteger b)
    {
        // Special case.
        if (a == 0 || b == 0)
        {
            return 0;
        }

        return BigInteger.Abs(a) * (BigInteger.Abs(b) / GreatestCommonDivisor(a, b));
    }

    /// <summary>
    /// Returns series of numbers in a Collatz series, starting at n and ending in 1.
    /// <see href="https://en.wikipedia.org/wiki/Collatz_conjecture" />
    /// </summary>
    /// <param name="n">Starting number.</param>
    /// <returns>The series of numbers.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static List<long> _Collatz(long n)
    {
        // Guard.
        if (n < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be less than 1.");
        }

        List<long> result = new () { n };

        if (n > 1)
        {
            // Get the next number.
            long m = n % 2 == 0 ? n / 2 : 3 * n + 1;

            // Append additional items in the chain.
            result.AddRange(Collatz(m));
        }

        return result;
    }

    /// <summary>
    /// Convert a long value to words.
    /// </summary>
    public static string NumberToWords(long n)
    {
        long quotient, remainder;
        string quotientStr, remainderStr;

        switch (n)
        {
            // We can't get the negative of long.MinValue as a long (see case < 0 below), so let's
            // just hard code the result for that one.
            // long.MinValue == -9,223,372,036,854,775,808
            case long.MinValue:
                return "negative nine quintillion, "
                    + "two hundred and twenty-three quadrillion, "
                    + "three hundred and seventy-two trillion, "
                    + "thirty-six billion, "
                    + "eight hundred and fifty-four million, "
                    + "seven hundred and seventy-five thousand, "
                    + "eight hundred and eight";

            case < 0:
                return $"negative {NumberToWords(-n)}";

            case < 20:
                return n switch
                {
                    0 => "zero",
                    1 => "one",
                    2 => "two",
                    3 => "three",
                    4 => "four",
                    5 => "five",
                    6 => "six",
                    7 => "seven",
                    8 => "eight",
                    9 => "nine",
                    10 => "ten",
                    11 => "eleven",
                    12 => "twelve",
                    13 => "thirteen",
                    14 => "fourteen",
                    15 => "fifteen",
                    16 => "sixteen",
                    17 => "seventeen",
                    18 => "eighteen",
                    19 => "nineteen",
                    // ReSharper disable once UnreachableSwitchArmDueToIntegerAnalysis
                    _ => "?"
                };

            case < 100:
                quotient = n / 10;
                remainder = n % 10;
                quotientStr = quotient switch
                {
                    2 => "twenty",
                    3 => "thirty",
                    4 => "forty",
                    5 => "fifty",
                    6 => "sixty",
                    7 => "seventy",
                    8 => "eighty",
                    9 => "ninety",
                    // Silence the code analyser about the non-exhaustive switch.
                    _ => throw new Exception()
                };
                remainderStr = remainder > 0 ? $"-{NumberToWords(remainder)}" : "";
                return $"{quotientStr}{remainderStr}";

            case < 1000:
                quotient = n / 100;
                remainder = n % 100;
                quotientStr = $"{NumberToWords(quotient)} hundred";
                remainderStr = remainder > 0 ? $" and {NumberToWords(remainder)}" : "";
                return $"{quotientStr}{remainderStr}";

            default:
                Dictionary<long, string> multiplierWords = new ()
                {
                    { 1000, "thousand" },
                    { 1_000_000, "million" },
                    { 1_000_000_000, "billion" },
                    { 1_000_000_000_000, "trillion" },
                    { 1_000_000_000_000_000, "quadrillion" },
                    { 1_000_000_000_000_000_000, "quintillion" }
                };
                long multiplier = 1_000_000_000_000_000_000;
                while (n < multiplier)
                {
                    multiplier /= 1000;
                }
                quotient = n / multiplier;
                remainder = n % multiplier;
                quotientStr = $"{NumberToWords(quotient)} {multiplierWords[multiplier]}";
                remainderStr = remainder > 0 ? $", {NumberToWords(remainder)}" : "";
                return $"{quotientStr}{remainderStr}";
        }
    }

    /// <summary>
    /// Count all the letters in a string.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int LetterCount(string s) =>
        s.Count(char.IsLetter);

    public static int GetAlphabeticalValue(string name) =>
        name.Sum(c => c - (char.IsLower(c) ? 'a' : 'A') + 1);

    /// <summary>
    /// Check if a number is perfect, deficient, or abundant.
    /// <see href="https://en.wikipedia.org/wiki/Perfect_number" />
    /// </summary>
    public static sbyte PerfectNumber(long n)
    {
        BigInteger spd = GetProperDivisors(n).Sum();

        // Check if the number is perfect.
        if (spd == n)
        {
            return 0;
        }

        // Check if the number is deficient or abundant.
        return (sbyte)(spd < n ? -1 : 1);
    }

    public static List<ulong> GetRotations(long n)
    {
        List<ulong> result = new ();
        string nString = n.ToString();
        int nDigits = nString.Length;
        for (int i = 1; i < nDigits; i++)
        {
            string newString = nString[i..] + nString[..i];
            result.Add(ulong.Parse(newString));
        }
        return result;
    }

    public static bool IsPandigital(string digits)
    {
        // Sort the digits.
        char[] sortedDigits = digits.ToCharArray();
        Array.Sort(sortedDigits);
        string strSortedDigits = string.Join("", sortedDigits);

        // Check they look like 1, 2, 3...
        return !strSortedDigits.Where((t, i) => t != i + '1').Any();
    }

    public static bool IsPandigital(ulong digits) =>
        IsPandigital(digits.ToString());

    public static int NumDigits(BigInteger n)
    {
        // Avoid logarithm of 0.
        if (n == 0)
        {
            return 1;
        }

        // Avoid logarithm of a negative number.
        n = BigInteger.Abs(n);

        // Get the logarithm, which will be within 1 of the answer.
        double log = BigInteger.Log10(n);

        // Account for fuzziness in the double representation of the logarithm.
        double floor = Floor(log);
        double round = Round(log);
        double nDigits = floor + (round > floor && log.FuzzyEquals(round) ? 2 : 1);

        return (int)nDigits;
    }

    public static List<byte> GetDigits(long n) =>
        Abs(n).ToString().Select(c => (byte)(c - '0')).ToList();

    public static List<byte> GetDistinctDigits(long n) =>
        GetDigits(n).Distinct().ToList();

    public static uint?[,] ConstructSpiral(int size, bool clockwise, EDirection start)
    {
        // Make sure it's odd.
        if (size % 2 != 1)
        {
            throw new ArgumentInvalidException(nameof(size), "Must be odd.");
        }

        // Create the data structure.
        uint?[,] spiral = new uint?[size, size];

        bool IsVacant(int x, int y) =>
            x >= 0 && x < size && y >= 0 && y < size && spiral[x, y] == null;

        // Start in the centre.
        int x = size / 2;
        int y = x;
        EDirection direction =
            clockwise ? Direction.GoAntiClockwise(start) : Direction.GoClockwise(start);

        // Loop until the spiral is built.
        for (uint n = 1; n <= size * size; n++)
        {
            // Set the value of the current position.
            spiral[x, y] = n;

            // Try to turn.
            switch (direction)
            {
                case EDirection.Up:
                    if (clockwise && IsVacant(x + 1, y))
                    {
                        // Go right.
                        x++;
                        direction = EDirection.Right;
                    }
                    else if (!clockwise && IsVacant(x - 1, y))
                    {
                        // Go left.
                        x--;
                        direction = EDirection.Left;
                    }
                    else if (IsVacant(x, y - 1))
                    {
                        // Go up again.
                        y--;
                    }
                    break;

                case EDirection.Right:
                    if (clockwise && IsVacant(x, y + 1))
                    {
                        // Go down.
                        y++;
                        direction = EDirection.Down;
                    }
                    else if (!clockwise && IsVacant(x, y - 1))
                    {
                        // Go up.
                        y--;
                        direction = EDirection.Up;
                    }
                    else if (IsVacant(x + 1, y))
                    {
                        // Go right again.
                        x++;
                    }
                    break;

                case EDirection.Down:
                    if (clockwise && IsVacant(x - 1, y))
                    {
                        // Go left.
                        x--;
                        direction = EDirection.Left;
                    }
                    else if (!clockwise && IsVacant(x + 1, y))
                    {
                        // Go right.
                        x++;
                        direction = EDirection.Right;
                    }
                    else if (IsVacant(x, y + 1))
                    {
                        // Go down again.
                        y++;
                    }
                    break;

                case EDirection.Left:
                    if (clockwise && IsVacant(x, y - 1))
                    {
                        // Go up.
                        y--;
                        direction = EDirection.Up;
                    }
                    else if (!clockwise && IsVacant(x, y + 1))
                    {
                        // Go down.
                        y++;
                        direction = EDirection.Down;
                    }
                    else if (IsVacant(x - 1, y))
                    {
                        // Go left again.
                        x--;
                    }
                    break;
            } // switch
        } // for n

        return spiral;
    }

    public static void PrintGrid(uint?[,] grid)
    {
        int size = XInt32.Sqrt(grid.Length);
        for (int y = 0; y < size; y++)
        {
            Console.Write("[ ");
            for (int x = 0; x < size; x++)
            {
                Console.Write((grid[x, y]?.ToString() ?? "").PadLeft(8));
            }
            Console.WriteLine(" ]");
        }
    }

    /// <summary>
    /// Calculate the inverse of an integer, showing reptend in brackets.
    /// </summary>
    /// <param name="x">An integer greater than 1.</param>
    /// <param name="reptend">The reptend, if any.</param>
    /// <returns>The inverse as a string.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string Inverse(long x, out string? reptend)
    {
        if (x < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                "Method only supports values greater than 1.");
        }

        StringBuilder decimals = new ();
        long n = 1;
        long d = x;
        int current = 0;
        Dictionary<(long n, long d), int> seen = new ();
        reptend = null;

        while (true)
        {
            // Get the quotient.
            long q = n / d;

            // Check for a repeat of this quotient. If we've seen it before, the pattern of digits
            // will repeat, indicating a reptend.
            if (seen.ContainsKey((n, d)))
            {
                // Found a repeat.
                int reptendStart = seen[(n, d)];
                int reptendLen = current - reptendStart;
                string decimalString = decimals.ToString();
                reptend = decimalString[^reptendLen..];
                return $"0.{decimalString[1..reptendStart]}({reptend})";
            }

            // Add the digit and record the numerator and denominator that produced it, and where.
            decimals.Append(q);
            seen[(n, d)] = current;

            // Get the remainder. If it's 0, we're done.
            long r = n % d;
            if (r == 0)
            {
                return $"0.{decimals.ToString()[1..]}";
            }

            // The remainder is the new numerator.
            n = r * 10;

            // Go to the next digit.
            current++;
        }
    }
}
