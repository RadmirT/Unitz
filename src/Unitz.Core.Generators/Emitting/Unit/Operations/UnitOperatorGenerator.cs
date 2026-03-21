using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Unit.Operations;

internal static class UnitOperatorGenerator
{
    private static readonly Dictionary<OperationKind, IUnitOperatorSourceEmitter> Emitters = new()
    {
        { OperationKind.Divide, new UnitDivideOperatorSourceEmitter() },
        { OperationKind.Multiply, new UnitMultiplyOperatorSourceEmitter() },
    };

    public static void GenerateOperators(CodeWriter codeWriter, IEnumerable<OperationModel> operationModelsModel, string unitTypeReference, string valueTypeReference)
    {
        foreach (var operationModel in operationModelsModel)
        {
            Emitters[operationModel.Kind].GenerateOperator(codeWriter, operationModel, unitTypeReference, valueTypeReference);
        }
    }
    
    public static void GenerateOperatorInterfaces(CodeWriter codeWriter, IEnumerable<OperationModel> operationModelsModel, string unitTypeReference, string valueTypeReference)
    {
        foreach (var operationModel in operationModelsModel)
        {
            codeWriter.Write(", ");
            Emitters[operationModel.Kind].GenerateOperationInterfaces(codeWriter, operationModel, unitTypeReference, valueTypeReference);
        }
    }
}
