using System.Numerics;
using Unitz.Core.UnitOperations.Binary;

namespace Unitz.Core;

internal static class QuantityOperationCore<TValue> where TValue : struct, INumber<TValue>
{
    internal static Quantity<TValue> Divide(IQuantity<TValue> left, IQuantity<TValue> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        if (right.BaseValue == TValue.Zero)
        {
            throw new DivideByZeroException();
        }

        return ExecuteBinaryOperation(BinaryUnitOperationType.Divide, left, right, static (l, r) => l / r);
    }

    internal static Quantity<TValue> Multiply(IQuantity<TValue> left, IQuantity<TValue> right)
    {
        return ExecuteBinaryOperation(BinaryUnitOperationType.Multiply, left, right, static (l, r) => l * r);
    }

    internal static Quantity<TValue> Add(IQuantity<TValue> left, IQuantity<TValue> right)
    {
        return ExecuteBinaryOperation(BinaryUnitOperationType.Add, left, right, static (l, r) => l + r);
    }

    internal static Quantity<TValue> Subtract(IQuantity<TValue> left, IQuantity<TValue> right)
    {
        return ExecuteBinaryOperation(BinaryUnitOperationType.Subtract, left, right, static (l, r) => l - r);
    }

    private static Quantity<TValue> ExecuteBinaryOperation(
        BinaryUnitOperationType operation,
        IQuantity<TValue> left,
        IQuantity<TValue> right,
        Func<TValue, TValue, TValue> valueOperation)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(valueOperation);

        var resultUnit = BinaryUnitOperations<TValue>.Execute(operation, left.Unit, right.Unit);
        var resultValue = resultUnit.FromBaseValue(valueOperation(left.BaseValue, right.BaseValue));
        return new Quantity<TValue>(resultValue, resultUnit);
    }
}
