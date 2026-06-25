namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(I = 1, Base = "Ampere")]
[SiLinearUnits(UnitPrefixes.Nano, UnitPrefixes.Micro, UnitPrefixes.Milli, UnitPrefixes.Kilo)]
[MultiplyOperation<TimeQuantitySpec, ElectricChargeQuantitySpec>]
[DivideOperation<TimeQuantitySpec, ElectricChargeQuantitySpec>]
[MultiplyOperation<ElectricResistanceQuantitySpec, VoltageQuantitySpec>]
internal class ElectricCurrentQuantitySpec
{
}
