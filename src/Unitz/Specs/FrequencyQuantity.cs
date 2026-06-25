namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(T = -1, Base = "Hertz")]
[SiLinearUnits(UnitPrefixes.Milli, UnitPrefixes.Kilo, UnitPrefixes.Mega, UnitPrefixes.Giga)]
[MultiplyOperation<TimeQuantitySpec, DimensionlessQuantitySpec>]
internal class FrequencyQuantitySpec
{
}
