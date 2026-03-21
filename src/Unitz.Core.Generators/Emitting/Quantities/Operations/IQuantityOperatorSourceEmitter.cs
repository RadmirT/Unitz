using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Quantities.Operations;

internal interface IQuantityOperatorSourceEmitter
{
    void GenerateOperator(CodeWriter codeWriter, OperationModel operationModel, QuantityModel  quantityModel);
    void GenerateOperationInterfaces(CodeWriter codeWriter, OperationModel operationModel, QuantityModel  quantityModel);
}