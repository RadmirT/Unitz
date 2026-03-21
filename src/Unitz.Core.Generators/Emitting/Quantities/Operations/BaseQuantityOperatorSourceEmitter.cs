using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Quantities.Operations;

internal abstract class BaseQuantityOperatorSourceEmitter(OperationKind kind) : IQuantityOperatorSourceEmitter
{
    public void GenerateOperator(CodeWriter codeWriter, OperationModel operationModel, QuantityModel quantityModel)
    {
        ValidateModel(operationModel);
        OnGenerateOperator(codeWriter, operationModel, quantityModel);  
        
    }

    public void GenerateOperationInterfaces(CodeWriter codeWriter, OperationModel operationModel, QuantityModel quantityModel)
    {
        ValidateModel(operationModel);
        OnGenerateOperationInterfaces(codeWriter, operationModel, quantityModel);  
    }
    
    private void ValidateModel(OperationModel operationModel)
    {
        if (operationModel.Kind != kind)
        {
            throw new InvalidOperationException();
        }
    }

    protected abstract void OnGenerateOperator(CodeWriter codeWriter, OperationModel operationModel, QuantityModel quantityModel);
    protected abstract void OnGenerateOperationInterfaces(CodeWriter codeWriter, OperationModel operationModel, QuantityModel quantityModel);
}