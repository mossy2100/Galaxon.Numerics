using AstroMultimedia.Core.Time;

namespace AstroMultimedia.Numerics.Quantities;

public static class Speed
{
    /// <summary>
    /// Convert a speed given in km/h to m/s.
    /// </summary>
    /// <param name="kmPerHour">Speed in km/h.</param>
    /// <returns>Speed in m/s.</returns>
    public static double KmPerHourToMetresPerSecond(double kmPerHour) =>
        kmPerHour * UnitPrefix.GetMultiplier("k") / Time.SECONDS_PER_HOUR;
}
