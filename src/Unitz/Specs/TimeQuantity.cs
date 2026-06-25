namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(T = 1, Base = "Second")]
[SiLinearUnits(UnitPrefixes.Nano, UnitPrefixes.Micro, UnitPrefixes.Milli)]
[LinearUnits("Minute", Scale = 60)]
[LinearUnits("Hour", Scale = 3600)]
[LinearUnits("Day", Scale = 86400)]
[LinearUnits("Week", Scale = 604800)]
internal class TimeQuantitySpec
{
}
