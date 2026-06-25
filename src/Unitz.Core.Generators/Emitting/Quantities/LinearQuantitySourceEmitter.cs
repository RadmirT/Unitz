using Unitz.Core.Generators.Emitting.Quantities.Operations;
using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Quantities;

internal sealed class LinearQuantitySourceEmitter : QuantitySourceEmitterBase
{
    public override string GetTypeName(QuantityModel model) => model.QuantityName;
    

    protected override void GenerateSource(CodeWriter codeWriter, QuantityModel model)
    {
        codeWriter.WriteLine($"{model.Accessibility} partial class {model.QuantityName}{model.TypeParametersDeclaration} : global::Unitz.Core.LinearQuantity<{model.QuantityReference}, {model.ValueTypeReference}, {model.UnitReference}>,");
        codeWriter.WriteLine($"global::System.Numerics.IAdditionOperators<{model.QuantityReference}, {model.QuantityReference}, {model.QuantityReference}>,");
        codeWriter.WriteLine($"global::System.Numerics.ISubtractionOperators<{model.QuantityReference}, {model.QuantityReference}, {model.QuantityReference}>,");
        codeWriter.WriteLine($"global::System.Numerics.IMultiplyOperators<{model.QuantityReference}, {model.ValueTypeReference}, {model.QuantityReference}>,");
        codeWriter.WriteLine($"global::System.Numerics.IDivisionOperators<{model.QuantityReference}, {model.ValueTypeReference}, {model.QuantityReference}>");
        QuantityOperatorGenerator.GenerateOperatorInterfaces(codeWriter, model);
        codeWriter.Write(model.TypeParameterConstraints);
        codeWriter.WriteLine();
        using (codeWriter.Block())
        {
            codeWriter.WriteQuantityConstructors(model.QuantityName, model.ValueTypeReference, model.UnitReference);
            codeWriter.WriteLine();
            WriteCreateOverride(codeWriter, model.QuantityReference, model.ValueTypeReference, model.UnitReference);
            codeWriter.WriteLine();
            codeWriter.WriteLine($"public static global::Unitz.Core.Dimension Dimension => {model.UnitReference}.Dimension;");
            codeWriter.WriteLine();
            WriteFromMethod(codeWriter, model, model.QuantityReference, model.UnitReference);
            codeWriter.GenerateLinearQuantityOperators(model.QuantityReference, model.ValueTypeReference, model.UnitReference);
            QuantityOperatorGenerator.GenerateOperators(codeWriter, model);
        }
    }
}
