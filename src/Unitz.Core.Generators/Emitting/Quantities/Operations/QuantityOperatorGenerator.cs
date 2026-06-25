using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Quantities.Operations;

internal static class QuantityOperatorGenerator
{
    private static readonly Dictionary<OperationKind, IQuantityOperatorSourceEmitter> Emitters = new()
    {
        { OperationKind.Divide, new QuantityDivideOperatorSourceEmitter() },
        { OperationKind.Multiply, new QuantityMultiplyOperatorSourceEmitter() },
    };

    public static void GenerateOperators(CodeWriter codeWriter, QuantityModel quantityModel)
    {
        foreach (var operationModel in quantityModel.Operations)
        {
            Emitters[operationModel.Kind].GenerateOperator(codeWriter, operationModel, quantityModel);
        }
    }
    
    public static void GenerateOperatorInterfaces(CodeWriter codeWriter, QuantityModel quantityModel)
    {
        foreach (var operationModel in quantityModel.Operations)
        {
            codeWriter.Write(", ");
            Emitters[operationModel.Kind].GenerateOperationInterfaces(codeWriter, operationModel, quantityModel);
        }
    }
}