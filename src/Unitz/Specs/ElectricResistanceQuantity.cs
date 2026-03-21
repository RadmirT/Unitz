namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 2, M = 1, T = -3, I = -2, Base = "Ohm")]
[SiLinearUnits(UnitPrefixes.Milli, UnitPrefixes.Kilo, UnitPrefixes.Mega)]
[MultiplyOperation<ElectricCurrentQuantitySpec, VoltageQuantitySpec>]
internal class ElectricResistanceQuantitySpec
{
}
