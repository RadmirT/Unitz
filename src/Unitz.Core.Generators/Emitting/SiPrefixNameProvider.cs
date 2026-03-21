namespace Unitz.Core.Generators.Emitting;

using System;
using System.Globalization;
using System.Linq;
using Unitz.Core.Generators.Models;

internal static class SiPrefixNameProvider
{
    public static string BuildSiUnitName(string baseUnitName, int prefix)
    {
        var prefixName = GetSiPrefixName(prefix);
        if (baseUnitName.StartsWith("Square", StringComparison.Ordinal))
        {
            return "Square" + prefixName + LowerFirst(baseUnitName.Substring("Square".Length));
        }

        if (baseUnitName.StartsWith("Cubic", StringComparison.Ordinal))
        {
            return "Cubic" + prefixName + LowerFirst(baseUnitName.Substring("Cubic".Length));
        }

        return prefixName + LowerFirst(baseUnitName);
    }

    public static int GetSiDimensionPower(DimensionModel dimension)
    {
        var exponents = new[] { dimension.L, dimension.M, dimension.T, dimension.I, dimension.Th, dimension.N, dimension.J }
            .Where(static value => value != 0)
            .ToArray();

        return exponents.Length == 1 ? Math.Abs(exponents[0]) : 1;
    }

    private static string LowerFirst(string value)
    {
        return string.IsNullOrEmpty(value) ? value : char.ToLowerInvariant(value[0]) + value.Substring(1);
    }

    private static string GetSiPrefixName(int prefix)
    {
        return prefix switch
        {
            -30 => "Quecto",
            -27 => "Ronto",
            -24 => "Yocto",
            -21 => "Zepto",
            -18 => "Atto",
            -15 => "Femto",
            -12 => "Pico",
            -9 => "Nano",
            -6 => "Micro",
            -3 => "Milli",
            -2 => "Centi",
            -1 => "Deci",
            1 => "Deca",
            2 => "Hecto",
            3 => "Kilo",
            6 => "Mega",
            9 => "Giga",
            12 => "Tera",
            15 => "Peta",
            18 => "Exa",
            21 => "Zetta",
            24 => "Yotta",
            27 => "Ronna",
            30 => "Quetta",
            _ => "TenPower" + prefix.ToString(CultureInfo.InvariantCulture).Replace("-", "Minus"),
        };
    }
}
