namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 1, Base = "Meter")]
[SiLinearUnits(UnitPrefixes.Nano, UnitPrefixes.Micro, UnitPrefixes.Milli, UnitPrefixes.Centi, UnitPrefixes.Deci, UnitPrefixes.Kilo)]
[LinearUnits("Inch", Scale = 0.0254)]
[LinearUnits("Foot", Scale = 0.3048)]
[LinearUnits("Yard", Scale = 0.9144)]
[LinearUnits("Mile", Scale = 1609.344)]
[LinearUnits("NauticalMile", Scale = 1852)]
[MultiplyOperation<LengthQuantitySpec, AreaQuantitySpec>]
[DivideOperation<TimeQuantitySpec, SpeedQuantitySpec>]
internal class LengthQuantitySpec
{
}
