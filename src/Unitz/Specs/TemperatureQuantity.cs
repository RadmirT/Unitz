namespace Unitz;

using Unitz.Core;

[GenericAffineQuantity(Th = 1, Base = "Kelvin")]
[AffineUnits("Celsius", Scale = 1, Offset = 273.15)]
[AffineUnits("Fahrenheit", Scale = 0.5555555555555556, Offset = 255.3722222222222)]
[AffineUnits("Rankine", Scale = 0.5555555555555556, Offset = 0)]
internal class TemperatureQuantitySpec
{
}
