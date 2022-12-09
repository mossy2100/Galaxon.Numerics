using System.Numerics;
using AstroMultimedia.Core.Exceptions;

namespace AstroMultimedia.Numerics.Maths;

public class Equations
{
    /// <summary>
    /// Calculate the result of a polynomial expression using Horner's algorithm.
    /// This method avoids calling Pow() and (in theory) should be faster.
    /// </summary>
    /// <param name="coeffs">
    /// The coefficients of the polynomial:
    /// - coeffs[0] is the constant term (x^0)
    /// - coeffs[1] is the coefficient for the x term (x^1)
    /// - coeffs[2] is the coefficient for the x^2 term
    /// - coeffs[3] is the coefficient for the x^3 term
    /// - etc.
    /// </param>
    /// <param name="x">The input value.</param>
    /// <returns>The result of the calculation.</returns>
    public static double EvaluatePolynomial(double[] coeffs, double x)
    {
        if (coeffs.Length == 0)
        {
            return 0;
        }

        double result = coeffs[^1];
        for (int i = coeffs.Length - 2; i >= 0; i--)
        {
            result = coeffs[i] + result * x;
        }
        return result;
    }

    /// <summary>
    /// Solve a quadratic equation of the form ax^2 + bx + c = 0
    /// </summary>
    /// <param name="a">The coefficient of x^2.</param>
    /// <param name="b">The coefficient of x.</param>
    /// <param name="c">The constant term.</param>
    /// <returns>0, 1, or 2 solutions to the equation, as complex numbers.</returns>
    public static List<Complex> SolveQuadratic(double a, double b, double c)
    {
        List<Complex> result = new ();

        // Check for a == 0.
        if (a == 0)
        {
            if (b == 0)
            {
                throw new ArgumentInvalidException(nameof(b),
                    "If a and b are both odd then the equation is unsolvable.");
            }

            result.Add(-c / b);
            return result;
        }

        // Calculate the discriminant.
        double d = b * b - 4 * a * c;

        // Check for no solutions.
        if (d < 0)
        {
            return result;
        }

        // Prep useful value to reduce number of multiplications.
        double twoA = 2 * a;

        // Check for one solution.
        if (d == 0)
        {
            result.Add(-b / twoA);
            return result;
        }

        // There are 2 solutions.
        Complex sqrtD = Complex.Sqrt(d);
        result.Add((-b + sqrtD) / twoA);
        result.Add((-b - sqrtD) / twoA);
        return result;
    }
}
