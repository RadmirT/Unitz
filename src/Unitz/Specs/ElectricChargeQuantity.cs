namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(T = 1, I = 1, Base = "Coulomb")]
[SiLinearUnits(UnitPrefixes.Milli, UnitPrefixes.Micro)]
[LinearUnits("AmpereHour", Scale = 3600)]
[LinearUnits("MilliampereHour", Scale = 3.6)]
[DivideOperation<VoltageQuantitySpec, CapacitanceQuantitySpec>]
[DivideOperation<TimeQuantitySpec, ElectricCurrentQuantitySpec>]
internal class ElectricChargeQuantitySpec
{
}
