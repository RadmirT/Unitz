namespace Unitz;

using Unitz.Core;

[GenericLinearQuantity(Base = "Byte")]
[LinearUnits("Bit", Scale = 0.125)]
[LinearUnits("Kilobyte", Scale = 1000)]
[LinearUnits("Megabyte", Scale = 1_000_000)]
[LinearUnits("Gigabyte", Scale = 1_000_000_000)]
[LinearUnits("Terabyte", Scale = 1_000_000_000_000)]
[LinearUnits("Kibibyte", Scale = 1024)]
[LinearUnits("Mebibyte", Scale = 1_048_576)]
[LinearUnits("Gibibyte", Scale = 1_073_741_824)]
[LinearUnits("Tebibyte", Scale = 1_099_511_627_776)]
internal class InformationQuantitySpec
{
}
