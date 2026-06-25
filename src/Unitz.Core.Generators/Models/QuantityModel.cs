namespace Unitz.Core.Generators.Models;

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

internal sealed class QuantityModel
{
    public QuantityModel(
        INamedTypeSymbol type,
        QuantityKind kind,
        string baseUnitName,
        DimensionModel dimension,
        string valueTypeReference,
        string quantityName,
        string unitName,
        string deltaQuantityName,
        string deltaUnitName,
        string @namespace,
        string accessibility,
        string quantityReference,
        string unitReference,
        string deltaQuantityReference,
        string deltaUnitReference,
        string typeParametersDeclaration = "",
        string typeParametersUsage = "",
        string typeParameterConstraints = "",
        IReadOnlyList<int>? siPrefixes = null,
        IReadOnlyList<LinearUnitModel>? linearUnits = null,
        IReadOnlyList<AffineUnitModel>? affineUnits = null,
        IReadOnlyList<OperationModel>? operations = null)
    {
        Type = type;
        Kind = kind;
        BaseUnitName = baseUnitName;
        Dimension = dimension;
        ValueTypeReference = valueTypeReference;
        QuantityName = quantityName;
        UnitName = unitName;
        DeltaQuantityName = deltaQuantityName;
        DeltaUnitName = deltaUnitName;
        Namespace = @namespace;
        Accessibility = accessibility;
        QuantityReference = quantityReference;
        UnitReference = unitReference;
        DeltaQuantityReference = deltaQuantityReference;
        DeltaUnitReference = deltaUnitReference;
        TypeParametersDeclaration = typeParametersDeclaration;
        TypeParametersUsage = typeParametersUsage;
        TypeParameterConstraints = typeParameterConstraints;
        SiPrefixes = siPrefixes ?? [];
        LinearUnits = linearUnits ?? [];
        AffineUnits = affineUnits ?? [];
        Operations = operations ?? [];
    }

    public INamedTypeSymbol Type { get; }

    public QuantityKind Kind { get; }

    public string QuantityName { get; }

    public string UnitName { get; }

    public string DeltaQuantityName { get; }

    public string DeltaUnitName { get; }

    public string BaseUnitName { get; }

    public DimensionModel Dimension { get; }

    public string Namespace { get; }

    public string Accessibility { get; }

    public string ValueTypeReference { get; }

    public string QuantityReference { get; }

    public string UnitReference { get; }

    public string DeltaQuantityReference { get; }

    public string DeltaUnitReference { get; }

    public string TypeParametersDeclaration { get; }

    public string TypeParametersUsage { get; }

    public string TypeParameterConstraints { get; }

    public bool IsGenericDefinition => !string.IsNullOrEmpty(TypeParametersUsage);

    public IReadOnlyList<int> SiPrefixes { get; }

    public IReadOnlyList<LinearUnitModel> LinearUnits { get; }

    public IReadOnlyList<AffineUnitModel> AffineUnits { get; }

    public IReadOnlyList<OperationModel> Operations { get; }

    public QuantityModel WithOperations(IReadOnlyList<OperationModel> operations)
    {
        return new QuantityModel(
            Type,
            Kind,
            BaseUnitName,
            Dimension,
            ValueTypeReference,
            QuantityName,
            UnitName,
            DeltaQuantityName,
            DeltaUnitName,
            Namespace,
            Accessibility,
            QuantityReference,
            UnitReference,
            DeltaQuantityReference,
            DeltaUnitReference,
            TypeParametersDeclaration,
            TypeParametersUsage,
            TypeParameterConstraints,
            SiPrefixes,
            LinearUnits,
            AffineUnits,
            operations);
    }
}
