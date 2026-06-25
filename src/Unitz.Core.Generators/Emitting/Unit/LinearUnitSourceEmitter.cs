using Unitz.Core.Generators.Emitting.Unit.Operations;
using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Unit;

internal sealed class LinearUnitSourceEmitter : UnitSourceEmitterBase
{
    public override string GetTypeName(QuantityModel model) => model.UnitName;

    protected override void GenerateSource(CodeWriter codeWriter, QuantityModel model)
    {
        codeWriter.WriteLine($"{model.Accessibility} partial class {model.UnitName}{model.TypeParametersDeclaration} : global::Unitz.Core.LinearUnit<{model.ValueTypeReference}>, global::Unitz.Core.IUnit<{model.UnitReference}, {model.ValueTypeReference}>");
        UnitOperatorGenerator.GenerateOperatorInterfaces(codeWriter, model.Operations, model.UnitReference, model.ValueTypeReference);
        codeWriter.Write(model.TypeParameterConstraints);
        using (codeWriter.Block())
        {
            WriteDimension(codeWriter, model.Dimension);

            codeWriter.WriteLine($"public {model.UnitName}(): base(Dimension, {NumberLiteralProvider.One(model.ValueTypeReference)})");
            codeWriter.WriteEmptyBlock();
            codeWriter.WriteLine();
            codeWriter.WriteLine($"public {model.UnitName}({model.ValueTypeReference} scale): base(Dimension, scale)");
            codeWriter.WriteEmptyBlock();
            codeWriter.WriteLine();
            codeWriter.WriteLine($"public static {model.UnitReference} Base => {model.BaseUnitName};");
            codeWriter.WriteLine();
            codeWriter.WriteLine($"public static {model.UnitReference} {model.BaseUnitName} => new();");
            codeWriter.WriteLine();

            WriteLinearUnitProperties(codeWriter, model);
            WriteLinearUnitFrom(codeWriter, model.UnitName, model.UnitReference, model.ValueTypeReference);
            UnitOperatorGenerator.GenerateOperators(codeWriter, model.Operations, model.UnitReference, model.ValueTypeReference);
            WritePow10(codeWriter, model.ValueTypeReference);
        }
    }

    private static void WriteLinearUnitProperties(CodeWriter codeWriter, QuantityModel model)
    {
        foreach (var prefix in model.SiPrefixes)
        {
            var name = SiPrefixNameProvider.BuildSiUnitName(model.BaseUnitName, prefix);
            var exponent = prefix * SiPrefixNameProvider.GetSiDimensionPower(model.Dimension);
            codeWriter.WriteLine();
            codeWriter.WriteLine($"public static {model.UnitReference} {name} {{ get; }} = new(Pow10({exponent}));");
        }

        foreach (var unit in model.LinearUnits)
        {
            codeWriter.WriteLine();
            codeWriter.WriteLine($"public static {model.UnitReference} {unit.Name} {{ get; }} = new({NumberLiteralProvider.ToGenericNumber(unit.Scale, model.ValueTypeReference)});");
        }
    }
}
