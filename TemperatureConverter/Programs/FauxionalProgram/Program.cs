namespace TemperatureConverter.Programs.FauxionalProgram;

public class Program : IProgram
{
    private readonly Prompt _prompt;
    private readonly TemperatureConverter _temperatureConverter;

    public Program(Prompt prompt, TemperatureConverter temperatureConverter)
    {
        _prompt = prompt;
        _temperatureConverter = temperatureConverter;
    }

    public void Run()
    {
        var inputValue = _prompt.Ask("Temperature value: ");
        var inputUnitString = _prompt.Ask("Convert from (C/F): ");
        var outputUnitString = _prompt.Ask("Convert to (C/F): ");
        
        var inputFloat = TryParseFloat(inputValue);
        var from = inputFloat.Bind(f => _temperatureConverter.ToTemperature(inputUnitString, f));
        var to = _temperatureConverter.ToUnit(outputUnitString);

        var convert = Result<IProgramError, Temperature>
            .Lift2<TemperatureUnit, Temperature>(_temperatureConverter.Convert);
        var result = convert(to, from);

        var toPrintableResult = Result<IProgramError, string>
            .Lift2<Temperature, Temperature>(ToPrintableResult);
        var printableResult = toPrintableResult(from, result);

        printableResult
            .MapError(error => error.Message())
            .Fork(Console.WriteLine, Console.WriteLine);
    }

    private static string ToPrintableResult(Temperature input, Temperature output)
    {
        return $"{input} = {output}";
    }
    
    private static Result<IProgramError, float> TryParseFloat(string? value)
    {
        return float.TryParse(value, out var floatResult) 
            ? Result<IProgramError, float>.Ok(floatResult) 
            : Result<IProgramError, float>.Error(new ParseFloatError());
    }
}