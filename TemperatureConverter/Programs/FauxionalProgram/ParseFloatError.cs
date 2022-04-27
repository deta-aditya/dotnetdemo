namespace TemperatureConverter.Programs.FauxionalProgram;

internal class ParseFloatError : IProgramError
{
    public string Message()
    {
        return "Value should be real number!";
    }
}