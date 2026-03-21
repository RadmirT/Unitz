namespace Unitz.Core.Generators.Analysis;

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Unitz.Core.Generators.Models;

internal static class QuantityOperationResolver
{
    public static IReadOnlyList<QuantityModel> AddReverseMultiplyOperations(IReadOnlyList<QuantityModel> models)
    {
        var operationsByType = new Dictionary<INamedTypeSymbol, List<OperationModel>>(SymbolEqualityComparer.Default);
        foreach (var model in models)
        {
            operationsByType[model.Type] = model.Operations.ToList();
        }

        foreach (var model in models)
        {
            foreach (var operation in model.Operations)
            {
                if (operation.Kind != Models.OperationKind.Multiply ||
                    SymbolEqualityComparer.Default.Equals(operation.Operand, model.Type) ||
                    model.IsGenericDefinition)
                {
                    continue;
                }

                var operandOperations = GetOperations(operation.Operand, operationsByType);
                if (HasOperation(operandOperations, Models.OperationKind.Multiply, model.Type, operation.Result))
                {
                    continue;
                }

                operandOperations.Add(new OperationModel(Models.OperationKind.Multiply, model.Type, operation.Result));
            }
        }

        var resolved = new List<QuantityModel>(models.Count);
        foreach (var model in models)
        {
            resolved.Add(model.WithOperations(operationsByType[model.Type].ToArray()));
        }

        return resolved;
    }

    private static List<OperationModel> GetOperations(
        INamedTypeSymbol type,
        IDictionary<INamedTypeSymbol, List<OperationModel>> operationsByType)
    {
        if (operationsByType.TryGetValue(type, out var operations))
        {
            return operations;
        }

        operations = [];
        operationsByType[type] = operations;
        return operations;
    }

    private static bool HasOperation(
        IEnumerable<OperationModel> operations,
        Models.OperationKind kind,
        INamedTypeSymbol operand,
        INamedTypeSymbol result)
    {
        foreach (var operation in operations)
        {
            if (operation.Kind == kind &&
                SymbolEqualityComparer.Default.Equals(operation.Operand, operand) &&
                SymbolEqualityComparer.Default.Equals(operation.Result, result))
            {
                return true;
            }
        }

        return false;
    }
}
