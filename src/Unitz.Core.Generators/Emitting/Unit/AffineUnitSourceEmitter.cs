using Unitz.Core.Generators.Emitting.Unit.Operations;
using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Unit;

internal sealed class AffineUnitSourceEmitter : UnitSourceEmitterBase
{
    public override string GetTypeName(QuantityModel model) => model.UnitName;

    protected override void GenerateSource(CodeWriter codeWriter, QuantityModel model)
    {
        codeWriter.WriteLine($"{model.Accessibility} partial class {model.UnitName}{model.TypeParametersDeclaration} : global::Unitz.Core.AffineUnit<{model.ValueTypeReference}>, global::Unitz.Core.IUnit<{model.UnitReference}, {model.ValueTypeReference}>");
        UnitOperatorGenerator.GenerateOperatorInterfaces(codeWriter, model.Operations, model.UnitReference, model.ValueTypeReference);
        codeWriter.Write(model.TypeParameterConstraints);
        using (codeWriter.Block())
        {
            WriteDimension(codeWriter, model.Dimension);

            codeWriter.WriteLine($"public {model.UnitName}(): base(Dimension, {NumberLiteralProvider.One(model.ValueTypeReference)})");
            codeWriter.WriteEmptyBlock();
            codeWriter.WriteLine();
            codeWriter.WriteLine($"public {model.UnitName}({model.ValueTypeReference} scale, {model.ValueTypeReference} offset): base(Dimension, scale, offset)");
            codeWriter.WriteEmptyBlock();
            codeWriter.WriteLine();
            codeWriter.WriteLine($"public static {model.UnitReference} Base => {model.BaseUnitName};");
            codeWriter.WriteLine();
            codeWriter.WriteLine($"public static {model.UnitReference} {model.BaseUnitName} => new();");
            codeWriter.WriteLine();

            WriteAffineUnitProperties(codeWriter, model);
            WriteAffineUnitFrom(codeWriter, model.UnitName, model.UnitReference, model.ValueTypeReference);
            UnitOperatorGenerator.GenerateOperators(codeWriter, model.Operations, model.UnitReference, model.ValueTypeReference);
            WritePow10(codeWriter, model.ValueTypeReference);
        }
    }

    private static void WriteAffineUnitProperties(CodeWriter codeWriter, QuantityModel model)
    {
        foreach (var unit in model.AffineUnits)
        {
            codeWriter.WriteLine();
            codeWriter.WriteLine($"public static {model.UnitReference} {unit.Name} {{ get; }} = new({NumberLiteralProvider.ToGenericNumber(unit.Scale, model.ValueTypeReference)}, {NumberLiteralProvider.ToGenericNumber(unit.Offset, model.ValueTypeReference)});");
        }
    }
}
