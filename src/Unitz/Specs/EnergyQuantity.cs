namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 2, M = 1, T = -2, Base = "Joule")]
[SiLinearUnits(UnitPrefixes.Milli, UnitPrefixes.Kilo, UnitPrefixes.Mega, UnitPrefixes.Giga)]
[LinearUnits("Calorie", Scale = 4.184)]
[LinearUnits("Kilocalorie", Scale = 4184)]
[LinearUnits("WattHour", Scale = 3600)]
[LinearUnits("KilowattHour", Scale = 3_600_000)]
[LinearUnits("Electronvolt", Scale = 1.602176634E-19)]
[LinearUnits("BritishThermalUnit", Scale = 1055.05585262)]
[DivideOperation<TimeQuantitySpec, PowerQuantitySpec>]
[DivideOperation<LengthQuantitySpec, ForceQuantitySpec>]
internal class EnergyQuantitySpec
{
}
