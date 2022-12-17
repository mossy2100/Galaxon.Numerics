using System.Numerics;
using AstroMultimedia.Numerics.Maths;

namespace AstroMultimedia.Numerics.Integers;

public static class Partitions
{
    /// <summary>
    /// P() function using a recurrence relation.
    /// The number of unique ways an integer can be partitioned into smaller integers.
    /// The argument is limited to ushort to avoid out-of-memory errors.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Partition_function_(number_theory)#Recurrence_relations" />
    /// <param name="n">The number to partition.</param>
    /// <returns>The number of partitions.</returns>
    public static BigInteger _P(ushort n)
    {
        // Terminating condition.
        if (n == 0)
        {
            return 1;
        }

        // Calculate the maximum value for k, our starting value.
        List<Complex> solutions = Equations.SolveQuadratic(3, -1, -2 * n);
        int maxK = (int)solutions.Max(cx => Abs(cx.Real));

        // Calculate the sum by adding terms from smallest to largest,
        // which means looping on k from the maximum down to 1.
        // We add terms from smallest to largest so we're computing terms containing the smaller
        // partitions first, which prevents stack overflow errors.
        BigInteger sum = 0;
        int sign = (int)Pow(-1, maxK + 1);
        for (int k = maxK; k > 0; k--)
        {
            // Add the term for positive k.
            int arg = n - k * (3 * k - 1) / 2;
            if (arg >= 0)
            {
                sum += sign * P((ushort)arg);
            }

            // Add the term for negative k.
            arg = n - k * (3 * k + 1) / 2;
            if (arg >= 0)
            {
                sum += sign * P((ushort)arg);
            }

            // Flip the sign.
            sign = -sign;
        }

        return sum;
    }

    /// <summary>
    /// Memoized version of P().
    /// </summary>
    public static readonly Func<ushort, BigInteger> P =
        Functions.Memoize<ushort, BigInteger>(_P);
}
