namespace Unitz.Core.Generators.Models;

using Microsoft.CodeAnalysis;

internal sealed class OperationModel
{
    public OperationModel(OperationKind kind, INamedTypeSymbol operand, INamedTypeSymbol result)
    {
        this.Kind = kind;
        this.Operand = operand;
        this.Result = result;
    }

    public OperationKind Kind { get; }

    public INamedTypeSymbol Operand { get; }

    public INamedTypeSymbol Result { get; }
}
