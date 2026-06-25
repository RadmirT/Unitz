namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(L = 2, Base = "SquareMeter")]
[SiLinearUnits(UnitPrefixes.Milli, UnitPrefixes.Centi, UnitPrefixes.Kilo)]
[LinearUnits("SquareInch", Scale = 0.00064516)]
[LinearUnits("SquareFoot", Scale = 0.09290304)]
[LinearUnits("SquareYard", Scale = 0.83612736)]
[LinearUnits("SquareMile", Scale = 2_589_988.110336)]
[LinearUnits("Acre", Scale = 4046.856422)]
[LinearUnits("Hectare", Scale = 10_000)]
internal class AreaQuantitySpec
{
}
