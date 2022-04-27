// See https://aka.ms/new-console-template for more information

using TemperatureConverter.Programs;
using TemperatureConverter.Programs.FauxionalProgram;
using FauxionalProgram = TemperatureConverter.Programs.FauxionalProgram.Program;
using TraditionalProgram = TemperatureConverter.Programs.TraditionalProgram.Program;

// var temperature = new TemperatureConverter.Programs.FauxionalProgram.TemperatureConverter();
// var prompt = new Prompt();
//
// IProgram program = new FauxionalProgram(prompt, temperature);

IProgram program = new TraditionalProgram();
program.Run();
