namespace TemperatureConverter.Programs.FauxionalProgram;

class ParseFloatError : IProgramError
{
    public string Message()
    {
        return "Value should be real number!";
    }
}