namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(M = 1, Base = "Kilogram")]
[LinearUnits("Gram", Scale = 0.001)]
[LinearUnits("Milligram", Scale = 0.000001)]
[LinearUnits("Microgram", Scale = 0.000000001)]
[LinearUnits("Tonne", Scale = 1000)]
[LinearUnits("Pound", Scale = 0.45359237)]
[LinearUnits("Ounce", Scale = 0.028349523125)]
[LinearUnits("Stone", Scale = 6.35029318)]
[MultiplyOperation<AccelerationQuantitySpec, ForceQuantitySpec>]
[DivideOperation<VolumeQuantitySpec, DensityQuantitySpec>]
internal class MassQuantitySpec
{
}
