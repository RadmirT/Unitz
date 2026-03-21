namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 3, Base = "CubicMeter")]
[SiLinearUnits(UnitPrefixes.Milli, UnitPrefixes.Centi, UnitPrefixes.Deci)]
[LinearUnits("Liter", Scale = 0.001)]
[LinearUnits("Milliliter", Scale = 0.000001)]
[LinearUnits("CubicInch", Scale = 0.000016387064)]
[LinearUnits("CubicFoot", Scale = 0.028316846592)]
[LinearUnits("CubicYard", Scale = 0.764554857984)]
[LinearUnits("GallonUs", Scale = 0.003785411784)]
[LinearUnits("QuartUs", Scale = 0.000946352946)]
[LinearUnits("PintUs", Scale = 0.000473176473)]
[LinearUnits("CupUs", Scale = 0.0002365882365)]
[LinearUnits("FluidOunceUs", Scale = 0.0000295735295625)]
[DivideOperation<AreaQuantitySpec, LengthQuantitySpec>]
[DivideOperation<TimeQuantitySpec, VolumeFlowRateQuantitySpec>]
internal class VolumeQuantitySpec
{
}
