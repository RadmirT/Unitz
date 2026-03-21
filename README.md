# Unitz

Unitz is a .NET library for working with physical quantities and units of measure.
It provides dimension-aware values, unit conversion, arithmetic operations, and generated strongly typed quantity APIs.

The runtime projects target .NET 7 and use generic math (`INumber<T>`) for numeric value support.

## Features

- Dimension-aware units based on SI base dimensions.
- Linear units such as meter, kilogram, second, joule, watt, volt, byte, and many others.
- Affine units such as Celsius and Fahrenheit, including temperature delta handling.
- Unit conversion through base values.
- Runtime quantity arithmetic for arbitrary dimensions.
- Strongly typed generated quantities such as `LengthQuantity<TValue>`, `AreaQuantity<TValue>`, `SpeedQuantity<TValue>`, and `TemperatureQuantity<TValue>`.
- Generated typed operators for valid derived operations, for example length by length to area or length divided by time to speed.
- Generic math operator interfaces for use in generic algorithms.
- Source/spec-based generation model for adding new quantities and units.

## Projects

| Project | Purpose |
| --- | --- |
| `Unitz.Core` | Core abstractions for dimensions, units, quantities, conversions, and arithmetic. |
| `Unitz.Units` | Predefined generated quantities and common units. |
| `Unitz.Core.Generators` | Roslyn-based generator infrastructure. |
| `Unitz.Tools.CodeGen` | Code generation tool used by the repository. |
| `Unitz.Core.UnitTests` | xUnit test suite for core behavior and generated units. |
| `Unitz.Examples` | Runnable examples for common conversions and typed quantity operations. |

## Requirements

- .NET SDK 7.0 or newer.

## Getting Started

Clone the repository and restore/build the solution:

```powershell
dotnet restore Unitz.slnx
dotnet build Unitz.slnx
```

Run the tests:

```powershell
dotnet test Unitz.slnx
```

## Usage

```csharp
using Unitz.Units;

var distance = new LengthQuantity<double>(1, LengthUnit<double>.Kilometer);
var extra = new LengthQuantity<double>(500, LengthUnit<double>.Meter);

var total = distance + extra;

Console.WriteLine(total.Value);     // 1.5
Console.WriteLine(total.BaseValue); // 1500
```

Convert between units:

```csharp
using Unitz.Units;

var length = new LengthQuantity<double>(1000, LengthUnit<double>.Millimeter);
var meters = length.To(LengthUnit<double>.Meter);

Console.WriteLine(meters.Value); // 1
```

Use typed derived operations:

```csharp
using Unitz.Units;

var width = new LengthQuantity<double>(2, LengthUnit<double>.Meter);
var height = new LengthQuantity<double>(3, LengthUnit<double>.Meter);

AreaQuantity<double> area = width * height;

Console.WriteLine(area.Value); // 6
Console.WriteLine(area.Unit == AreaUnit<double>.SquareMeter); // True
```

Work with affine quantities:

```csharp
using Unitz.Units;

var current = new TemperatureQuantity<double>(20, TemperatureUnit<double>.Celsius);
var previous = new TemperatureQuantity<double>(10, TemperatureUnit<double>.Celsius);

TemperatureDeltaQuantity<double> delta = current - previous;
var shifted = current + new TemperatureDeltaQuantity<double>(18, TemperatureDeltaUnit<double>.Fahrenheit);

Console.WriteLine(delta.Value);   // 10
Console.WriteLine(shifted.Value); // 30
```

## Defining Quantities

Quantities are described as spec classes with attributes. Generated code creates generic quantity and unit types, predefined units, conversions, and typed operators.

Example:

```csharp
using Unitz.Core;

[GenericLinearQuantity(L = 1, Base = "Meter")]
[SiLinearUnits(UnitPrefixes.Nano, UnitPrefixes.Micro, UnitPrefixes.Milli, UnitPrefixes.Centi, UnitPrefixes.Deci, UnitPrefixes.Kilo)]
[LinearUnits("Inch", Scale = 0.0254)]
[LinearUnits("Foot", Scale = 0.3048)]
[LinearUnits("Yard", Scale = 0.9144)]
[LinearUnits("Mile", Scale = 1609.344)]
[MultiplyOperation<LengthQuantitySpec, AreaQuantitySpec>]
[DivideOperation<TimeQuantitySpec, SpeedQuantitySpec>]
internal class LengthQuantitySpec
{
}
```

For affine quantities use `GenericAffineQuantity`.

## Code Generation

The repository supports two generation flows.

1. Checked-in generated sources (`Unitz.Units/Generated`) via tool:

```powershell
dotnet run --project Unitz.Tools.CodeGen/Unitz.Tools.CodeGen.csproj -- Unitz.Units
```

2. Compile-time Roslyn generation directly during build:

```powershell
dotnet build Unitz.Units/Unitz.Units.csproj -p:UseRoslynSourceGeneration=true
```

When `UseRoslynSourceGeneration=true`, generated files are emitted to:

- `Unitz.Units/bin/<Configuration>/<TargetFramework>/Generated`

and checked-in files under `Unitz.Units/Generated` are excluded from compilation to avoid duplicate types.

Base dimension parameters:

| Parameter | Dimension |
| --- | --- |
| `L` | Length |
| `M` | Mass |
| `T` | Time |
| `I` | Electric current |
| `Th` | Thermodynamic temperature |
| `N` | Amount of substance |
| `J` | Luminous intensity |

## Repository Status

This repository is under active development. The public API, generated code shape, and package metadata may change before a stable release.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
