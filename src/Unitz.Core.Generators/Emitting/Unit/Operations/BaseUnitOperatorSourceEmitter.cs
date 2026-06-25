using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Unit.Operations;

internal abstract class BaseUnitOperatorSourceEmitter(OperationKind kind) : IUnitOperatorSourceEmitter
{
    public void GenerateOperator(CodeWriter codeWriter, OperationModel operationModel, string unitTypeReference,string valueTypeReference)
    {
        ValidateModel(operationModel);
        OnGenerateOperator(codeWriter, operationModel, unitTypeReference, valueTypeReference);  
        
    }

    public void GenerateOperationInterfaces(CodeWriter codeWriter, OperationModel operationModel,
        string unitTypeReference, string valueTypeReference)
    {
        ValidateModel(operationModel);
        OnGenerateOperationInterfaces(codeWriter, operationModel, unitTypeReference, valueTypeReference);  
    }
    
    private void ValidateModel(OperationModel operationModel)
    {
        if (operationModel.Kind != kind)
        {
            throw new InvalidOperationException();
        }
    }

    protected abstract void OnGenerateOperator(CodeWriter codeWriter, OperationModel operationModel, string unitTypeReference, string valueTypeReference);
    protected abstract void OnGenerateOperationInterfaces(CodeWriter codeWriter, OperationModel operationModel,string unitTypeReference, string valueTypeReference);
}
