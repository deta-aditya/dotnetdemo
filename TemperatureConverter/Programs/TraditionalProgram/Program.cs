namespace TemperatureConverter.Programs.TraditionalProgram;

public class Program: IProgram
{
    public void Run()
    {
        Console.Write("Temperature value: ");
        var inputValue = Console.ReadLine();
        
        Console.Write("Convert from (C/F): ");
        var inputUnit = Console.ReadLine();
        
        Console.Write("Convert to (C/F): ");
        var outputUnit = Console.ReadLine();

        if (!double.TryParse(inputValue, out var inputDouble))
        {
            Console.WriteLine("Value should be real number!");
            return;
        }
        
        double result;
        if (inputUnit == "C" && outputUnit == "F")
        {
            result = inputDouble * 1.8 + 32.0;
        } else if (inputUnit == "F" && outputUnit == "C")
        {
            result = (inputDouble - 32.0) * 5.0 / 9.0;
        } else if (inputUnit == outputUnit)
        {
            result = inputDouble;
        }
        else
        {
            Console.WriteLine("Input/output unit should be either C or F");
            return;
        }
        
        Console.WriteLine("{0:F2} {1} = {2:F2} {3}", inputDouble, inputUnit, result, outputUnit);
    }
}