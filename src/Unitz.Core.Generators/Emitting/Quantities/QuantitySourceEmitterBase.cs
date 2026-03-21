using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Quantities;

internal abstract class QuantitySourceEmitterBase : BaseSourceEmitter
{
    protected static void WriteCreateOverride(CodeWriter codeWriter, string quantityReference, string valueTypeReference, string unitReference)
    {
        codeWriter.WriteLine($"protected override {quantityReference} Create({valueTypeReference} value, {unitReference} unit)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine($"return new {quantityReference}(value, unit);");
        }
    }

    protected static void WriteFromMethod(CodeWriter codeWriter, QuantityModel model, string quantityReference, string unitReference)
    {
        codeWriter.WriteLine($"public static {quantityReference} From(global::Unitz.Core.IQuantity<{model.ValueTypeReference}> quantity)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine("global::System.ArgumentNullException.ThrowIfNull(quantity);");
            codeWriter.WriteLine();
            codeWriter.WriteLine("if (!quantity.Unit.IsSameDimension(Dimension))");
            using (codeWriter.Block())
            {
                codeWriter.WriteLine("throw new global::System.InvalidOperationException(\"Operation is available only for quantities of the same dimension.\");");
            }

            codeWriter.WriteLine();
            codeWriter.WriteLine($"var targetUnit = {unitReference}.Base;");
            codeWriter.WriteLine($"return new {quantityReference}(targetUnit.FromBaseValue(quantity.BaseValue), targetUnit);");
        }
    }
}
