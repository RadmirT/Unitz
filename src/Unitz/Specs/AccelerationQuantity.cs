namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 1, T = -2, Base = "MeterPerSecondSquared")]
[LinearUnits("StandardGravity", Scale = 9.80665)]
[MultiplyOperation<TimeQuantitySpec, SpeedQuantitySpec>]
internal class AccelerationQuantitySpec
{
}
