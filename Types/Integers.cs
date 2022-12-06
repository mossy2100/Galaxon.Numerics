namespace AstroMultimedia.Numerics.Types;

public static class Integers
{
    /// <summary>
    /// Return the absolute value of a sbyte as a byte.
    /// It has to be a byte because Abs(sbyte.MinValue) can't be expressed as a sbyte.
    /// </summary>
    /// <param name="i">A sbyte value.</param>
    /// <returns>The absolute value.</returns>
    public static byte Abs(sbyte i) =>
        i switch
        {
            sbyte.MinValue => sbyte.MaxValue + 1,
            >= 0 => (byte)i,
            _ => (byte)-i
        };

    /// <summary>
    /// Return the absolute value of a short as a ushort.
    /// It has to be a ushort because Abs(short.MinValue) can't be expressed as a short.
    /// </summary>
    /// <param name="i">A short value.</param>
    /// <returns>The absolute value.</returns>
    public static ushort Abs(short i) =>
        i switch
        {
            short.MinValue => short.MaxValue + 1,
            >= 0 => (ushort)i,
            _ => (ushort)-i
        };

    /// <summary>
    /// Return the absolute value of an int as a uint.
    /// It has to be a uint because Abs(int.MinValue) can't be expressed as an int.
    /// </summary>
    /// <param name="l">A int value.</param>
    /// <returns>The absolute value.</returns>
    public static uint Abs(int l) =>
        l switch
        {
            int.MinValue => int.MaxValue + 1u,
            >= 0 => (uint)l,
            _ => (uint)-l
        };

    /// <summary>
    /// Return the absolute value of a long as a ulong.
    /// It has to be a ulong because Abs(long.MinValue) can't be expressed as a long.
    /// </summary>
    /// <param name="l">A long value.</param>
    /// <returns>The absolute value.</returns>
    public static ulong Abs(long l) =>
        l switch
        {
            long.MinValue => long.MaxValue + 1ul,
            >= 0 => (ulong)l,
            _ => (ulong)-l
        };
}
