namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 1, T = -1, Base = "MeterPerSecond")]
[LinearUnits("KilometerPerHour", Scale = 0.2777777777777778)]
[LinearUnits("MilePerHour", Scale = 0.44704)]
[LinearUnits("FootPerSecond", Scale = 0.3048)]
[LinearUnits("Knot", Scale = 0.5144444444444445)]
[MultiplyOperation<TimeQuantitySpec, LengthQuantitySpec>]
[DivideOperation<TimeQuantitySpec, AccelerationQuantitySpec>]
internal class SpeedQuantitySpec
{
}
