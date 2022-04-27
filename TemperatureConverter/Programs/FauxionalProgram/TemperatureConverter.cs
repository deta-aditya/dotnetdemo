namespace TemperatureConverter.Programs.FauxionalProgram;

public enum TemperatureUnit
{
    Fahrenheit,
    Celsius
}

public abstract record Temperature(double Value);

public sealed record Celsius(double Value) : Temperature(Value)
{
    public override string ToString()
    {
        return $"{Value} C";
    }
}

public sealed record Fahrenheit(double Value) : Temperature(Value)
{
    public override string ToString()
    {
        return $"{Value} F";
    }
}

public class TemperatureConverter
{
    public Result<IProgramError, TemperatureUnit> ToUnit(string? unitString) =>
        unitString switch
        {
            "C" => Result<IProgramError, TemperatureUnit>.Ok(TemperatureUnit.Celsius),
            "F" => Result<IProgramError, TemperatureUnit>.Ok(TemperatureUnit.Fahrenheit),
            _ => Result<IProgramError, TemperatureUnit>.Error(new ParseUnitError()),
        };

    public Result<IProgramError, Temperature> ToTemperature(string? unitString, double value)
    {
        return unitString switch
        {
            "C" => Result<IProgramError, Temperature>.Ok(new Celsius(value)),
            "F" => Result<IProgramError, Temperature>.Ok(new Fahrenheit(value)),
            _ => Result<IProgramError, Temperature>.Error(new ParseUnitError()),
        };
    }

    public Temperature Convert(TemperatureUnit to, Temperature from)
    {
        return (from, to) switch
        {
            (Celsius celsius, TemperatureUnit.Fahrenheit) => new Fahrenheit(celsius.Value * 1.8 + 32),
            (Fahrenheit fahrenheit, TemperatureUnit.Celsius) => new Celsius((fahrenheit.Value - 32) * 5 / 9),
            _ => from
        };
    }
}

public class ParseUnitError : IProgramError
{
    public string Message()
    {
        return "Input/output unit should be either C or F";
    }
}