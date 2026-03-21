using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Unit.Operations;

internal class UnitDivideOperatorSourceEmitter() : BaseUnitOperatorSourceEmitter(OperationKind.Divide)
{
  
    protected override void OnGenerateOperator(CodeWriter codeWriter, OperationModel operationModel, string unitTypeReference, string valueTypeReference)
    {
        var operandUnit = QuantityNameProvider.UnitReference(operationModel.Operand, valueTypeReference);
        var resultUnit = QuantityNameProvider.UnitReference(operationModel.Result, valueTypeReference);
     
        codeWriter.WriteLine();
        codeWriter.WriteLine($"public static {resultUnit} operator /({unitTypeReference} left, {operandUnit} right)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine("global::System.ArgumentNullException.ThrowIfNull(left);");
            codeWriter.WriteLine("global::System.ArgumentNullException.ThrowIfNull(right);");
            codeWriter.WriteLine($"return {resultUnit}.From((global::Unitz.Core.Unit<{valueTypeReference}>) left / right);");
        }
    }


    protected override void OnGenerateOperationInterfaces(CodeWriter codeWriter, OperationModel operationModel, string unitTypeReference, string valueTypeReference)
    {
        codeWriter.WriteLine($"global::System.Numerics.IDivisionOperators<{unitTypeReference}, {QuantityNameProvider.UnitReference(operationModel.Operand, valueTypeReference)}, {QuantityNameProvider.UnitReference(operationModel.Result, valueTypeReference)}>");
    }
}
