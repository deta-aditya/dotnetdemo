namespace TemperatureConverter.Programs.FauxionalProgram;

public class Prompt
{
    public string? Ask(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }
}