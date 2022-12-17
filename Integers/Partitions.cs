using System.Numerics;

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

        // Get the signs and arguments so we can calculate the smaller partitions first.
        // This wiil avoid a stack overflow.
        // The results of P() are cached due to memoization of the method.
        List<(int sign, ushort arg)> terms = new ();
        int k = 1;
        int s = 1;
        while (true)
        {
            // Positive k term.
            int arg = n - k * (3 * k - 1) / 2;
            Console.WriteLine(arg);
            if (arg < 0)
            {
                break;
            }
            terms.Add((s, (ushort)arg));

            // Negative k term.
            arg = n - k * (3 * k + 1) / 2;
            Console.WriteLine(arg);
            if (arg < 0)
            {
                break;
            }
            terms.Add((s, (ushort)arg));

            // Go to next pair.
            k++;
            s = -s;
        }

        // Order the terms by argument so we calculate the smaller ones first.
        terms = terms.OrderBy(t => t.arg).ToList();

        // Sum the terms.
        BigInteger sum = 0;
        foreach ((int sign, ushort arg) in terms)
        {
            sum += sign * P(arg);
        }
        return sum;
    }

    /// <summary>
    /// Memoized version of P().
    /// </summary>
    public static readonly Func<ushort, BigInteger> P =
        Functions.Memoize<ushort, BigInteger>(_P);
}
