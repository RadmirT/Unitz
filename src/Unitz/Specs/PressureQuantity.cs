namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = -1, M = 1, T = -2, Base = "Pascal")]
[SiLinearUnits(UnitPrefixes.Kilo, UnitPrefixes.Mega)]
[LinearUnits("Bar", Scale = 100000)]
[LinearUnits("Atmosphere", Scale = 101325)]
[LinearUnits("Torr", Scale = 133.32236842105263)]
[LinearUnits("PoundPerSquareInch", Scale = 6894.757293168)]
[MultiplyOperation<AreaQuantitySpec, ForceQuantitySpec>]
internal class PressureQuantitySpec
{
}
