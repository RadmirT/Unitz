namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = -2, M = -1, T = 3, I = 2, Base = "Siemens")]
[SiLinearUnits(UnitPrefixes.Milli, UnitPrefixes.Micro, UnitPrefixes.Kilo)]
[MultiplyOperation<VoltageQuantitySpec, ElectricCurrentQuantitySpec>]
[DivideOperation<ElectricCurrentQuantitySpec, VoltageQuantitySpec>]
internal class ElectricConductanceQuantitySpec
{
}
