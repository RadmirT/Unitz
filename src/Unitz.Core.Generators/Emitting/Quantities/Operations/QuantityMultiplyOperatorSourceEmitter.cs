using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Quantities.Operations;

internal sealed class QuantityMultiplyOperatorSourceEmitter() : BaseQuantityOperatorSourceEmitter(OperationKind.Multiply)
{
    protected override void OnGenerateOperator(CodeWriter codeWriter, OperationModel operationModel, QuantityModel quantityModel)
    {
        var operand = QuantityNameProvider.QuantityReference(operationModel.Operand, quantityModel.ValueTypeReference);
        var result = QuantityNameProvider.QuantityReference(operationModel.Result, quantityModel.ValueTypeReference);
        var resultUnit = QuantityNameProvider.UnitReference(operationModel.Result, quantityModel.ValueTypeReference);
        codeWriter.WriteLine($"public static {result} operator *({quantityModel.QuantityReference} left, {operand} right)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine("global::System.ArgumentNullException.ThrowIfNull(left);");
            codeWriter.WriteLine("global::System.ArgumentNullException.ThrowIfNull(right);");
            codeWriter.WriteLine($"var targetUnit = {resultUnit}.From(left.Unit * right.Unit);");
            codeWriter.WriteLine($"return new {result}(targetUnit.FromBaseValue(left.BaseValue * right.BaseValue), targetUnit);");
        }
    }

    protected override void OnGenerateOperationInterfaces(CodeWriter codeWriter, OperationModel operationModel, QuantityModel quantityModel)
    {
        codeWriter.WriteLine($"global::System.Numerics.IMultiplyOperators<{quantityModel.QuantityReference}, {QuantityNameProvider.QuantityReference(operationModel.Operand, quantityModel.ValueTypeReference)}, {QuantityNameProvider.QuantityReference(operationModel.Result, quantityModel.ValueTypeReference)}>");
    }
}
