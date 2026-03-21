namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 1, M = 1, T = -2, Base = "Newton")]
[SiLinearUnits(UnitPrefixes.Milli, UnitPrefixes.Kilo, UnitPrefixes.Mega)]
[LinearUnits("Dyne", Scale = 0.00001)]
[LinearUnits("PoundForce", Scale = 4.4482216152605)]
[DivideOperation<AreaQuantitySpec, PressureQuantitySpec>]
[MultiplyOperation<LengthQuantitySpec, EnergyQuantitySpec>]
internal class ForceQuantitySpec
{
}
