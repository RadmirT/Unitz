using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Unit.Operations;

internal interface IUnitOperatorSourceEmitter
{
    void GenerateOperator(CodeWriter codeWriter, OperationModel operationModel, string unitTypeReference, string valueTypeReference);
    void GenerateOperationInterfaces(CodeWriter codeWriter, OperationModel operationModel, string unitTypeReference, string valueTypeReference);
}
