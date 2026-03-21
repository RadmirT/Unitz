namespace Unitz.Core.Generators.Analysis;

using System;
using Microsoft.CodeAnalysis;
using Unitz.Core.Generators.Emitting;
using Unitz.Core.Generators.Models;
using GeneratorOperationKind = Unitz.Core.Generators.Models.OperationKind;

internal static class QuantityModelBuilder
{
    public static IReadOnlyList<QuantityModel> Create(INamedTypeSymbol type)
    {
        if (!TryCreatePrimaryModel(type, out var model))
        {
            return [];
        }

        ApplyUnitAttributes(type, model);
        ApplyOperationAttributes(type, model);
        return CreateDerivedModels(model).ToArray();
    }

    private static bool TryCreatePrimaryModel(INamedTypeSymbol type, out MutableQuantityModel model)
    {
        foreach (var attribute in type.GetAttributes())
        {
            var fullName = attribute.AttributeClass?.ToDisplayString();
            if (!IsQuantityAttribute(fullName))
            {
                continue;
            }

            var kind = fullName == AttributeNames.GenericAffineQuantitySpecAttribute
                ? QuantityKind.Affine
                : QuantityKind.Linear;
            var baseUnitName = ResolveBaseUnitName(attribute);
            var quantityName = GetGeneratedQuantityName(type.Name);
            var unitName = QuantityNameProvider.GetUnitName(quantityName);
            var deltaQuantityName = QuantityNameProvider.GetDeltaQuantityName(quantityName);
            var deltaUnitName = QuantityNameProvider.GetDeltaUnitName(unitName);
            var @namespace = type.ContainingNamespace.IsGlobalNamespace ? string.Empty : type.ContainingNamespace.ToDisplayString();
            const string typeParametersDeclaration = "<TValue>";
            const string typeParametersUsage = "<TValue>";
            const string typeParameterConstraints = " where TValue : struct, global::System.Numerics.INumber<TValue>";

            model = new MutableQuantityModel(
                type,
                kind,
                baseUnitName,
                DimensionModel.From(attribute),
                "TValue",
                quantityName,
                unitName,
                deltaQuantityName,
                deltaUnitName,
                @namespace,
                "public",
                QuantityNameProvider.Reference(type, quantityName + typeParametersUsage),
                QuantityNameProvider.Reference(type, unitName + typeParametersUsage),
                QuantityNameProvider.Reference(type, deltaQuantityName + typeParametersUsage),
                QuantityNameProvider.Reference(type, deltaUnitName + typeParametersUsage),
                typeParametersDeclaration,
                typeParametersUsage,
                typeParameterConstraints);
            return true;
        }

        model = null!;
        return false;
    }

    private static IEnumerable<QuantityModel> CreateDerivedModels(MutableQuantityModel model)
    {
        yield return model.ToImmutable();
        if (model.Kind != QuantityKind.Affine)
        {
            yield break;
        }

        var deltaLinearUnits = model.AffineUnits.Select(static unit => new LinearUnitModel(unit.Name, unit.Scale)).ToArray();
        yield return new QuantityModel(
            model.Type,
            QuantityKind.Linear,
            model.BaseUnitName,
            model.Dimension,
            model.ValueTypeReference,
            model.DeltaQuantityName,
            model.DeltaUnitName,
            model.DeltaQuantityName,
            model.DeltaUnitName,
            model.Namespace,
            model.Accessibility,
            model.DeltaQuantityReference,
            model.DeltaUnitReference,
            model.DeltaQuantityReference,
            model.DeltaUnitReference,
            model.TypeParametersDeclaration,
            model.TypeParametersUsage,
            model.TypeParameterConstraints,
            linearUnits: deltaLinearUnits);
    }

    private static void ApplyUnitAttributes(INamedTypeSymbol type, MutableQuantityModel model)
    {
        foreach (var attribute in type.GetAttributes())
        {
            var fullName = attribute.AttributeClass?.ToDisplayString();
            switch (fullName)
            {
                case AttributeNames.SiLinearUnitsAttribute:
                    model.SiPrefixes.AddRange(AttributeReader.GetPrefixArguments(attribute));
                    break;
                case AttributeNames.LinearUnitsAttribute:
                    model.LinearUnits.Add(new LinearUnitModel(
                        AttributeReader.GetConstructorArgument(attribute, 0, "Unit"),
                        AttributeReader.GetNamedArgument(attribute, "Scale", 1d)));
                    break;
                case AttributeNames.AffineUnitsAttribute:
                    model.AffineUnits.Add(new AffineUnitModel(
                        AttributeReader.GetConstructorArgument(attribute, 0, "Unit"),
                        AttributeReader.GetNamedArgument(attribute, "Scale", 1d),
                        AttributeReader.GetNamedArgument(attribute, "Offset", 0d)));
                    break;
            }
        }
    }

    private static void ApplyOperationAttributes(INamedTypeSymbol type, MutableQuantityModel model)
    {
        foreach (var attribute in type.GetAttributes())
        {
            var originalName = attribute.AttributeClass?.OriginalDefinition.ToDisplayString();
            var kind = originalName switch
            {
                AttributeNames.MultiplyOperationAttribute + "<TOperand, TResult>" => GeneratorOperationKind.Multiply,
                AttributeNames.DivideOperationAttribute + "<TOperand, TResult>" => GeneratorOperationKind.Divide,
                _ => (GeneratorOperationKind?)null,
            };

            if (kind is null || attribute.AttributeClass is not INamedTypeSymbol attributeType || attributeType.TypeArguments.Length != 2)
            {
                continue;
            }

            if (attributeType.TypeArguments[0] is INamedTypeSymbol operand && attributeType.TypeArguments[1] is INamedTypeSymbol result)
            {
                model.Operations.Add(new OperationModel(kind.Value, operand, result));
            }
        }
    }

    private static bool IsQuantityAttribute(string? fullName)
    {
        return fullName == AttributeNames.GenericQuantitySpecAttribute ||
               fullName == AttributeNames.GenericLinearQuantitySpecAttribute ||
               fullName == AttributeNames.GenericAffineQuantitySpecAttribute;
    }

    private static string ResolveBaseUnitName(AttributeData attribute)
    {
        var baseUnitName = AttributeReader.GetNamedArgument(attribute, "Base", string.Empty);
        return string.IsNullOrWhiteSpace(baseUnitName) ? "Base" : baseUnitName;
    }

    private static string GetGeneratedQuantityName(string specTypeName)
    {
        return specTypeName.EndsWith("Spec", StringComparison.Ordinal)
            ? specTypeName.Substring(0, specTypeName.Length - 4)
            : specTypeName;
    }

    private sealed class MutableQuantityModel
    {
        public MutableQuantityModel(
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
            string typeParametersDeclaration,
            string typeParametersUsage,
            string typeParameterConstraints)
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
        public List<int> SiPrefixes { get; } = [];
        public List<LinearUnitModel> LinearUnits { get; } = [];
        public List<AffineUnitModel> AffineUnits { get; } = [];
        public List<OperationModel> Operations { get; } = [];

        public QuantityModel ToImmutable()
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
                SiPrefixes.ToArray(),
                LinearUnits.ToArray(),
                AffineUnits.ToArray(),
                Operations.ToArray());
        }
    }
}
