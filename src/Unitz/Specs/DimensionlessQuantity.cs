namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(Base = "One")]
[LinearUnits("Percent", Scale = 0.01)]
[LinearUnits("Permille", Scale = 0.001)]
[LinearUnits("PartsPerMillion", Scale = 0.000001)]
[LinearUnits("PartsPerBillion", Scale = 0.000000001)]
internal class DimensionlessQuantitySpec
{
}
