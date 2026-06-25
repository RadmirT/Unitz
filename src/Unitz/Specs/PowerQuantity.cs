namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 2, M = 1, T = -3, Base = "Watt")]
[SiLinearUnits(UnitPrefixes.Milli, UnitPrefixes.Kilo, UnitPrefixes.Mega, UnitPrefixes.Giga)]
[LinearUnits("Horsepower", Scale = 745.6998715822702)]
[DivideOperation<ElectricCurrentQuantitySpec, VoltageQuantitySpec>]
[MultiplyOperation<TimeQuantitySpec, EnergyQuantitySpec>]
internal class PowerQuantitySpec
{
}
