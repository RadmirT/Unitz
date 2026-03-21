namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 2, M = 1, T = -2, I = -1, Base = "Weber")]
[SiLinearUnits(UnitPrefixes.Milli, UnitPrefixes.Micro)]
[DivideOperation<AreaQuantitySpec, MagneticFluxDensityQuantitySpec>]
internal class MagneticFluxQuantitySpec
{
}
