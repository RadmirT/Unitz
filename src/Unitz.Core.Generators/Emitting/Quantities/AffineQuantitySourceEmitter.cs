using Unitz.Core.Generators.Emitting.Quantities.Operations;
using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Quantities;

internal sealed class AffineQuantitySourceEmitter : QuantitySourceEmitterBase
{
    public override string GetTypeName(QuantityModel model) => model.QuantityName;

    protected override void GenerateSource(CodeWriter codeWriter, QuantityModel model)
    {
        codeWriter.WriteLine($"{model.Accessibility} partial class {model.QuantityName}{model.TypeParametersDeclaration} : global::Unitz.Core.AffineQuantity<{model.QuantityReference}, {model.DeltaQuantityReference}, {model.ValueTypeReference}, {model.UnitReference}, {model.DeltaUnitReference}>,");
        codeWriter.WriteLine($"global::System.Numerics.ISubtractionOperators<{model.QuantityReference}, {model.QuantityReference}, {model.DeltaQuantityReference}>,");
        codeWriter.WriteLine($"global::System.Numerics.IAdditionOperators<{model.QuantityReference}, {model.DeltaQuantityReference}, {model.QuantityReference}>,");
        codeWriter.WriteLine($"global::System.Numerics.ISubtractionOperators<{model.QuantityReference}, {model.DeltaQuantityReference}, {model.QuantityReference}>");

        QuantityOperatorGenerator.GenerateOperatorInterfaces(codeWriter, model);
        codeWriter.Write(model.TypeParameterConstraints);
        codeWriter.WriteLine();
        using (codeWriter.Block())
        {
            codeWriter.WriteQuantityConstructors(model.QuantityName, model.ValueTypeReference, model.UnitReference);
            codeWriter.WriteLine();
            WriteCreateOverride(codeWriter, model.QuantityReference, model.ValueTypeReference, model.UnitReference);
            codeWriter.WriteLine();
            codeWriter.WriteLine($"protected override {model.DeltaQuantityReference} CreateDelta({model.ValueTypeReference} value, {model.DeltaUnitReference} unit)");
            using (codeWriter.Block())
            {
                codeWriter.WriteLine($"return new {model.DeltaQuantityReference}(value, unit);");
            }

            GenerateAffineQuantityOperators(codeWriter, model);
            QuantityOperatorGenerator.GenerateOperators(codeWriter, model);
        }
    }

    private static void GenerateAffineQuantityOperators(CodeWriter codeWriter, QuantityModel model)
    {
        var baseType = $"global::Unitz.Core.AffineQuantity<{model.QuantityReference}, {model.DeltaQuantityReference}, {model.ValueTypeReference}, {model.UnitReference}, {model.DeltaUnitReference}>";

        codeWriter.WriteLine();
        codeWriter.WriteLine($"public static {model.DeltaQuantityReference} operator -({model.QuantityReference} left, {model.QuantityReference} right)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine($"return ({baseType})left - right;");
        }

        codeWriter.WriteLine();
        codeWriter.WriteLine($"public static {model.QuantityReference} operator +({model.QuantityReference} left, {model.DeltaQuantityReference} right)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine($"return ({baseType})left + right;");
        }

        codeWriter.WriteLine();
        codeWriter.WriteLine($"public static {model.QuantityReference} operator -({model.QuantityReference} left, {model.DeltaQuantityReference} right)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine($"return ({baseType})left - right;");
        }
    }
}
