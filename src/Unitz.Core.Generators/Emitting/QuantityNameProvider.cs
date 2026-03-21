namespace Unitz.Core.Generators.Emitting;

using System.Linq;
using Microsoft.CodeAnalysis;

internal static class QuantityNameProvider
{
    public static string QuantityReference(INamedTypeSymbol type, string? genericArgument)
    {
        var quantityName = NormalizeSpecTypeName(type.Name);
        return Reference(type, quantityName + BuildTypeArgumentList(type, genericArgument));
    }

    public static string QuantityReference(INamedTypeSymbol type)
    {
        return QuantityReference(type, null);
    }

    public static string UnitReference(INamedTypeSymbol quantityType, string? genericArgument)
    {
        var quantityName = NormalizeSpecTypeName(quantityType.Name);
        var unitName = GetUnitName(quantityName) + BuildTypeArgumentList(quantityType, genericArgument);
        return Reference(quantityType, unitName);
    }

    public static string UnitReference(INamedTypeSymbol quantityType)
    {
        return UnitReference(quantityType, null);
    }

    public static string Reference(INamedTypeSymbol type, string typeName)
    {
        var ns = type.ContainingNamespace.IsGlobalNamespace ? string.Empty : type.ContainingNamespace.ToDisplayString() + ".";
        return $"global::{ns}{typeName}";
    }

    public static string GetUnitName(string quantityName)
    {
        return quantityName.EndsWith("Quantity", System.StringComparison.Ordinal)
            ? quantityName.Substring(0, quantityName.Length - "Quantity".Length) + "Unit"
            : quantityName + "Unit";
    }

    public static string GetDeltaQuantityName(string quantityName)
    {
        return quantityName.EndsWith("Quantity", System.StringComparison.Ordinal)
            ? quantityName.Substring(0, quantityName.Length - "Quantity".Length) + "DeltaQuantity"
            : quantityName + "DeltaQuantity";
    }

    public static string GetDeltaUnitName(string unitName)
    {
        return unitName.EndsWith("Unit", System.StringComparison.Ordinal)
            ? unitName.Substring(0, unitName.Length - "Unit".Length) + "DeltaUnit"
            : unitName + "DeltaUnit";
    }

    private static string BuildTypeArgumentList(INamedTypeSymbol type, string? genericArgument)
    {
        if (type.Arity == 0)
        {
            return IsGeneratedGenericSpec(type) && !string.IsNullOrWhiteSpace(genericArgument)
                ? "<" + genericArgument + ">"
                : string.Empty;
        }

        if (!string.IsNullOrWhiteSpace(genericArgument))
        {
            return "<" + string.Join(", ", Enumerable.Repeat(genericArgument, type.Arity)) + ">";
        }

        if (type.TypeArguments.Length == type.Arity && type.TypeArguments.All(static a => a.TypeKind != TypeKind.Error))
        {
            return "<" + string.Join(", ", type.TypeArguments.Select(ToTypeReference)) + ">";
        }

        return "<" + string.Join(", ", Enumerable.Repeat("TValue", type.Arity)) + ">";
    }

    private static bool IsGeneratedGenericSpec(INamedTypeSymbol type)
    {
        foreach (var attribute in type.GetAttributes())
        {
            var attributeName = attribute.AttributeClass?.ToDisplayString();
            if (attributeName == "Unitz.Core.GenericQuantityAttribute" ||
                attributeName == "Unitz.Core.GenericLinearQuantityAttribute" ||
                attributeName == "Unitz.Core.GenericAffineQuantityAttribute")
            {
                return true;
            }
        }

        return false;
    }

    private static string ToTypeReference(ITypeSymbol typeSymbol)
    {
        if (typeSymbol is ITypeParameterSymbol parameter)
        {
            return parameter.Name;
        }

        return typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }

    private static string NormalizeSpecTypeName(string typeName)
    {
        return typeName.EndsWith("Spec", System.StringComparison.Ordinal)
            ? typeName.Substring(0, typeName.Length - 4)
            : typeName;
    }
}
