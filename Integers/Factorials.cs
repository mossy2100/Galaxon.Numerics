using System.Numerics;
using static AstroMultimedia.Numerics.Integers.Functions;

namespace AstroMultimedia.Numerics.Integers;

/// <summary>
/// Factorials, permutations, and combinations.
/// </summary>
public static class Factorials
{
    #region Factorial

    /// <summary>
    /// Factorial of n, n >= 0.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static BigInteger _Factorial(BigInteger n)
    {
        // Guard.
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");
        }

        return n <= 1 ? 1 : n * Factorial(n - 1);
    }

    /// <summary>
    /// Public memoized version of the Factorial method.
    /// </summary>
    public static readonly Func<BigInteger, BigInteger> Factorial =
        Memoize<BigInteger, BigInteger>(_Factorial);

    #endregion Factorial

    #region Combinations

    /// <summary>
    /// Find the number of ways r items can be selected from a set of n items.
    /// The order of the items is unimportant.
    /// i.e. If selecting 2 integers from a set {1, 2, 3, 4, 5}, the selection {1, 2} is counted as
    /// equal to the selection {2, 1}.
    /// </summary>
    /// <param name="n">Number of items to select from.</param>
    /// <param name="r">Number of items to select.</param>
    /// <returns></returns>
    public static BigInteger NumCombinations(long n, long r) =>
        Factorial(n) / (Factorial(r) * Factorial(n - r));

    /// <summary>
    /// Get all the different ways to select n items from a bag of items, ignoring order.
    /// </summary>
    public static List<List<T>> GetCombinations<T>(List<T> bag, int n)
    {
        List<List<T>> result = new ();

        // Combinations of 0 items is undefined.
        if (n == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n),
                "Number of items to select must be positive.");
        }

        // There is only one way of selecting n items from a bag with n items.
        if (n == bag.Count)
        {
            result.Add(bag.ToList());
            return result;
        }

        // There are zero ways of selecting n items from a bag with fewer than n items.
        if (n > bag.Count)
        {
            return result;
        }

        for (int i = 0; i < bag.Count; i++)
        {
            // If we only want 1 item, shortcut.
            if (n == 1)
            {
                List<T> newCombo = new () { bag[i] };
                result.Add(newCombo);
                continue;
            }

            // How many items left?
            int nRemainingItems = bag.Count - i;

            // If there are n items remaining, shortcut.
            if (nRemainingItems == n)
            {
                List<T> newCombo = bag.GetRange(i, n);
                result.Add(newCombo);
                continue;
            }

            // Get the bag with all items following the ith item.
            List<T> remainder = bag.GetRange(i + 1, nRemainingItems - 1);

            // Find all the ways of selecting n-1 items from the remainder.
            List<List<T>> remCombos = GetCombinations(remainder, n - 1);
            foreach (List<T> remCombo in remCombos)
            {
                List<T> newCombo = new () { bag[i] };
                newCombo.AddRange(remCombo);
                result.Add(newCombo);
            }
        }

        return result;
    }

    #endregion Combinations

    #region Permutations

    /// <summary>
    /// Find the number of ways r items can be selected from a set of n items.
    /// The order of the items is important.
    /// i.e. If selecting 2 integers from a set {1, 2, 3, 4, 5}, the selection {1, 2} is different
    /// from the selection {2, 1}.
    /// </summary>
    /// <param name="n">Number of items to select from.</param>
    /// <param name="r">Number of items to select.</param>
    /// <returns></returns>
    public static BigInteger NumPermutations(long n, long r) =>
        Factorial(n) / Factorial(n - r);

    /// <summary>
    /// Get all the different ways to select n items from a bag of items, considering order.
    /// </summary>
    public static List<List<T>> GetPermutations<T>(List<T> bag, int n)
    {
        List<List<T>> result = new ();

        // There are 0 ways of selecting n items from a bag with fewer than n items.
        if (n > bag.Count)
        {
            return result;
        }

        // There is only 1 way of selecting 0 items from a bag, irrespective of how many items are
        // in the bag.
        if (n == 0)
        {
            result.Add(new List<T>());
            return result;
        }

        // There are n ways of selecting 1 item from a bag with n items.
        if (n == 1)
        {
            result.AddRange(bag.Select(t => new List<T> { t }));
            return result;
        }

        // Get all the ways of selecting n items from the bag.
        for (int i = 0; i < bag.Count; i++)
        {
            // Select an item from the bag.
            T item = bag[i];

            // Get the remainder of the bag.
            List<T> remainder = bag.GetRange(0, i);
            List<T> remainder2 = bag.GetRange(i + 1, bag.Count - i - 1);
            remainder.AddRange(remainder2);

            // Get all permutations of the remainder.
            List<List<T>> remPerms = GetPermutations(remainder, n - 1);
            foreach (List<T> remPerm in remPerms)
            {
                List<T> perm = new () { item };
                perm.AddRange(remPerm);
                result.Add(perm);
            }
        }

        return result;
    }

    /// <summary>
    /// Get all permutations of the given string of characters.
    /// </summary>
    /// <param name="chars">String of characters.</param>
    /// <returns>All possible permutations.</returns>
    private static List<string> _CharPermutations(string chars)
    {
        switch (chars.Length)
        {
            case 0:
                return new List<string>();

            case 1:
                return new List<string> { chars };
        }

        HashSet<string> result = new ();
        for (int i = 0; i < chars.Length; i++)
        {
            char ch = chars[i];
            string rem = chars[..i] + chars[(i + 1)..];
            List<string> permutations = CharPermutations(rem);
            foreach (string perm in permutations)
            {
                result.Add($"{ch}{perm}");
            }
        }
        return result.ToList();
    }

    /// <summary>
    /// Public memoized version of method.
    /// </summary>
    public static readonly Func<string, List<string>> CharPermutations =
        Memoize<string, List<string>>(_CharPermutations);

    #endregion Permutations
}
