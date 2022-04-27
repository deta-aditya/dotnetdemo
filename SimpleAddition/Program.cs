// See https://aka.ms/new-console-template for more information

Console.Write("First number: ");
var number1 = Console.ReadLine();
Console.Write("Second number: ");
var number2 = Console.ReadLine();

if (!(int.TryParse(number1, out var number1Int) && int.TryParse(number2, out var number2Int)))
{
    Console.WriteLine("Please input number!");
    return;
}

var result = number1Int + number2Int;

Console.WriteLine("{0} + {1} = {2}", number1Int, number2Int, result);
