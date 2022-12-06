namespace AstroMultimedia.Numerics.Quantities;

public static class Temperature
{
    /// <summary>
    /// Number of degrees difference between 0K and 0°C.
    /// </summary>
    public const double CELSIUS_KELVIN_DIFF = 273.15;

    /// <summary>
    /// Number of degrees difference between 0°F and 0°C.
    /// </summary>
    public const double CELSIUS_FAHRENHEIT_DIFF = 32;

    /// <summary>
    /// Number of degrees Celsius (or kelvins) per degree of Fahrenheit.
    /// </summary>
    public const double CELSIUS_PER_FAHRENHEIT = 5.0 / 9;

    /// <summary>
    /// Convert a temperature in Celsius to Kelvin.
    /// </summary>
    /// <param name="c">Temperature in Celsius.</param>
    /// <returns>Temperature in Kelvin.</returns>
    public static double CelsiusToKelvin(double c) =>
        c + CELSIUS_KELVIN_DIFF;

    public static double KelvinToCelsius(double k) =>
        k - CELSIUS_KELVIN_DIFF;

    public static double CelsiusToFahrenheit(double c) =>
        (c / CELSIUS_PER_FAHRENHEIT) + CELSIUS_FAHRENHEIT_DIFF;

    public static double FahrenheitToCelsius(double f) =>
        (f - CELSIUS_FAHRENHEIT_DIFF) * CELSIUS_PER_FAHRENHEIT;

    public static double FahrenheitToKelvin(double f) =>
        CelsiusToKelvin(FahrenheitToCelsius(f));

    public static double KelvinToFahrenheit(double k) =>
        CelsiusToFahrenheit(KelvinToCelsius(k));
}
