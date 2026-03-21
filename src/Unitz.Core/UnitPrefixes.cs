namespace Unitz.Core;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Contains decimal SI prefix exponents.
/// Values correspond to powers of ten (10^n).
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Prefix names are self-explanatory.")]
#pragma warning disable CS1591
public static class UnitPrefixes
{
    public const int Quecto = -30;
    public const int Ronto = -27;
    public const int Yocto = -24;
    public const int Zepto = -21;
    public const int Atto = -18;
    public const int Femto = -15;
    public const int Pico = -12;
    public const int Nano = -9;
    public const int Micro = -6;
    public const int Milli = -3;
    public const int Centi = -2;
    public const int Deci = -1;
    public const int Deca = 1;
    public const int Hecto = 2;
    public const int Kilo = 3;
    public const int Mega = 6;
    public const int Giga = 9;
    public const int Tera = 12;
    public const int Peta = 15;
    public const int Exa = 18;
    public const int Zetta = 21;
    public const int Yotta = 24;
    public const int Ronna = 27;
    public const int Quetta = 30;
}
#pragma warning restore CS1591
