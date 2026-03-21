namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = -3, M = 1, Base = "KilogramPerCubicMeter")]
[LinearUnits("GramPerCubicCentimeter", Scale = 1000)]
[LinearUnits("GramPerMilliliter", Scale = 1000)]
[LinearUnits("PoundPerCubicFoot", Scale = 16.01846337396014)]
[MultiplyOperation<VolumeQuantitySpec, MassQuantitySpec>]
internal class DensityQuantitySpec
{
}
