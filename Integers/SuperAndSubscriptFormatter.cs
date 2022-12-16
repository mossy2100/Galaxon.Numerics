using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using AstroMultimedia.Core.Exceptions;
using AstroMultimedia.Core.Strings;

namespace AstroMultimedia.Numerics.Integers;

/// <summary>
/// Enables formatting of a number or numeric string as its superscript or subscript form.
/// </summary>
public class SuperAndSubscriptFormatter : IFormatProvider, ICustomFormatter
{
    /// <summary>
    /// Superscript versions of characters.
    /// </summary>
    public static readonly Dictionary<char, char> SuperscriptChars = new ()
    {
        { '-', '⁻' },
        { '0', '⁰' },
        { '1', '¹' },
        { '2', '²' },
        { '3', '³' },
        { '4', '⁴' },
        { '5', '⁵' },
        { '6', '⁶' },
        { '7', '⁷' },
        { '8', '⁸' },
        { '9', '⁹' }
    };

    /// <summary>
    /// Subscript versions of characters.
    /// </summary>
    public static readonly Dictionary<char, char> SubscriptChars = new ()
    {
        { '-', '₋' },
        { '0', '₀' },
        { '1', '₁' },
        { '2', '₂' },
        { '3', '₃' },
        { '4', '₄' },
        { '5', '₅' },
        { '6', '₆' },
        { '7', '₇' },
        { '8', '₈' },
        { '9', '₈' }
    };

    public object? GetFormat(Type? formatType) =>
        formatType == typeof(ICustomFormatter) ? this : null;

    /// <summary>
    /// Format an object using superscripts or subscripts.
    /// </summary>
    /// <param name="format">
    /// The format specifier, comprising "sup" or "sub", optionally followed by a digit indicating
    /// the action to take on encountering an invalid character.
    ///   0 = Throw an exception.
    ///   1 = Skip it, excluding it from the output (default).
    ///   2 = Keep the original, untransformed character.
    /// For example: sup, sup0, sup1, sup2, sub, sub0, sub1, or sub2
    /// </param>
    /// <param name="arg">The number or other object to format.</param>
    /// <param name="formatProvider">Reference to this instance.</param>
    /// <returns>The number or other object formatted as a string.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentInvalidException"></exception>
    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        // Check for legitimate callback.
        if (!Equals(formatProvider))
        {
            throw new ArgumentInvalidException(nameof(formatProvider),
                "Should match the instance.");
        }

        // Format null as empty string.
        if (arg is null)
        {
            return "";
        }

        // Check for valid format string.
        if (format is null || !Regex.IsMatch(format, "^su[pb][0-2]?$", RegexOptions.IgnoreCase))
        {
            throw new ArgumentInvalidException(nameof(format),
                "Invalid format specifier. Must be \"S\" for superscript or \"s\" for subscript,"
                + " optionally followed by a single-digit action code (0-2). See documentation for"
                + " more details.");
        }

        // Convert argument to string.
        string? strArg = arg switch
        {
            sbyte n => n.ToString("d"),
            byte n => n.ToString("d"),
            short n => n.ToString("d"),
            ushort n => n.ToString("d"),
            int n => n.ToString("d"),
            uint n => n.ToString("d"),
            long n => n.ToString("d"),
            ulong n => n.ToString("d"),
            BigInteger n => n.ToString("d"),

            // Format float, double, and decimal values as integers via truncation, since decimal
            // points aren't available as superscript or subscript.
            float n => n.ToString("f0"),
            double n => n.ToString("f0"),
            decimal n => n.ToString("f0"),

            // Handle other types.
            string s => s,
            IFormattable s => s.ToString(format, formatProvider),
            _ => arg.ToString(),
        };

        // If ToString() returns a null return an empty string.
        if (strArg is null)
        {
            return "";
        }

        // Extract the format parameters.
        string strLowerCaseFormat = format[..3].ToLower(CultureInfo.InvariantCulture);
        int invalidCharActionCode = (format.Length == 4) ? (format[^1] - '0') : 1;

        // Transform the string.
        return strLowerCaseFormat switch
        {
            // Superscript.
            "sup" => strArg.Transform(SuperscriptChars, invalidCharActionCode),

            // Subscript.
            "sub" => strArg.Transform(SubscriptChars, invalidCharActionCode),

            _ => throw new ArgumentInvalidException(nameof(format),
                "Invalid format.")
        };
    }
}
