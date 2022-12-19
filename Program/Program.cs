// See https://aka.ms/new-console-template for more information

decimal[] values =
{
    -2.6m,
    -2.5m,
    -2.4m,
    -1.6m,
    -1.51m,
    -1.5m,
    -1.4m,
    -0.5m,
    0.5m,
    1.4m,
    1.5m,
    1.51m,
    1.6m,
    2.4m,
    2.5m,
    2.6m
};

foreach (decimal value in values)
{
    Console.WriteLine();
    Console.WriteLine($"Round({value}, ToEven) = {decimal.Round(value, MidpointRounding.ToEven)}");
    Console.WriteLine($"Round({value}, AwayFromZero) = {decimal.Round(value, MidpointRounding.AwayFromZero)}");
    Console.WriteLine($"Round({value}, ToZero) = {decimal.Round(value, MidpointRounding.ToZero)}");
    Console.WriteLine($"Round({value}, ToNegativeInfinity) = {decimal.Round(value, MidpointRounding.ToNegativeInfinity)}");
    Console.WriteLine($"Round({value}, ToPositiveInfinity) = {decimal.Round(value, MidpointRounding.ToPositiveInfinity)}");
}
