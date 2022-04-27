// For more information see https://aka.ms/fsharp-console-apps

open System

type Result<'l, 'r> =
    | Ok of 'r
    | Error of 'l
    
let bind f r =
    match r with
    | Ok ok -> f ok
    | Error err -> Error err 
    
let lift2 f =
    fun a b ->
        match a, b with
        | Ok a', Ok b' -> Ok (f a' b')
        | Error err, _ -> Error err
        | _, Error err -> Error err
    
type ProgramError =
    | ParseValueError
    | ParseUnitError

type TemperatureUnit =
    | CelsiusUnit
    | FahrenheitUnit
    
type Temperature =
    | Celsius of double
    | Fahrenheit of double
    
let createUnit unitStr =
    match unitStr with
    | "C" -> Ok CelsiusUnit
    | "F" -> Ok FahrenheitUnit
    | _ -> Error ParseUnitError

let createTemperature unitStr value =
    match unitStr with
    | "C" -> Celsius value |> Ok
    | "F" -> Fahrenheit value |> Ok
    | _ -> Error ParseUnitError

let convert fromTemp toUnit =
    match fromTemp, toUnit with
    | Celsius c, FahrenheitUnit -> Fahrenheit (c * 1.8 + 32.0)
    | Fahrenheit f, CelsiusUnit -> Celsius ((f - 32.0) * 5.0 / 9.0)
    | _ -> fromTemp
    
let temperatureToString temp =
    match temp with
    | Celsius c -> $"%.2f{c} C"
    | Fahrenheit f -> $"%.2f{f} F"
    
let toPrintable input output =
    $"%s{temperatureToString input} = %s{temperatureToString output}"

let prompt (message: string) =
    printf $"%s{message}"
    Console.ReadLine()
    
let parseDouble s =
    try
        Ok (double s)
    with _ ->
        Error ParseValueError
let value = prompt "Temperature value: "
let inputUnit = prompt "Convert from (C/F): "
let outputUnit = prompt "Convert to (C/F): "

let from =
    parseDouble value
    |> bind (createTemperature inputUnit)

let printableResult =
    createUnit outputUnit
    |> lift2 convert from
    |> lift2 toPrintable from

let printableStr =
    match printableResult with
    | Ok result -> result
    | Error ParseValueError -> "Value should be real number!"
    | Error ParseUnitError -> "Input/output unit should be either C or F"

printf $"%s{printableStr}"    
