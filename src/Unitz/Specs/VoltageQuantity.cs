namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 2, M = 1, T = -3, I = -1, Base = "Volt")]
[SiLinearUnits(UnitPrefixes.Milli, UnitPrefixes.Kilo, UnitPrefixes.Mega)]
[DivideOperation<ElectricCurrentQuantitySpec, ElectricResistanceQuantitySpec>]
[DivideOperation<ElectricResistanceQuantitySpec, ElectricCurrentQuantitySpec>]
[MultiplyOperation<ElectricCurrentQuantitySpec, ElectricResistanceQuantitySpec>]
[MultiplyOperation<TimeQuantitySpec, MagneticFluxQuantitySpec>]
[DivideOperation<PowerQuantitySpec, ElectricCurrentQuantitySpec>]
internal class VoltageQuantitySpec
{
}
