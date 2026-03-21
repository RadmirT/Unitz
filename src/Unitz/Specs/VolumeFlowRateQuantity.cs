namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 3, T = -1, Base = "CubicMeterPerSecond")]
[LinearUnits("LiterPerSecond", Scale = 0.001)]
[LinearUnits("LiterPerMinute", Scale = 0.000016666666666666667)]
[LinearUnits("CubicMeterPerHour", Scale = 0.0002777777777777778)]
[LinearUnits("GallonUsPerMinute", Scale = 0.0000630901964)]
[MultiplyOperation<TimeQuantitySpec, VolumeQuantitySpec>]
internal class VolumeFlowRateQuantitySpec
{
}
