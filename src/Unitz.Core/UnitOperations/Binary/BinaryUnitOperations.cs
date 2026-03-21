using System.Numerics;

namespace Unitz.Core.UnitOperations.Binary;

internal static class BinaryUnitOperations<T> where T : struct, INumber<T>
{
    public static Unit<T> Execute(BinaryUnitOperationType operation, Unit<T> left, Unit<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return (operation, left.Family, right.Family) switch
        {
            (BinaryUnitOperationType.Subtract, UnitFamily.Affine, UnitFamily.Affine) => SubtractAffineAffine(left, right),
            (BinaryUnitOperationType.Add, UnitFamily.Affine, UnitFamily.Linear) => ReturnLeftForSameDimension(left, right),
            (BinaryUnitOperationType.Subtract, UnitFamily.Affine, UnitFamily.Linear) => ReturnLeftForSameDimension(left, right),
            (BinaryUnitOperationType.Add, UnitFamily.Linear, UnitFamily.Linear) => ReturnLeftForSameDimension(left, right),
            (BinaryUnitOperationType.Subtract, UnitFamily.Linear, UnitFamily.Linear) => ReturnLeftForSameDimension(left, right),
            (BinaryUnitOperationType.Multiply, UnitFamily.Linear, UnitFamily.Linear) => MultiplyLinearLinear(left, right),
            (BinaryUnitOperationType.Divide, UnitFamily.Linear, UnitFamily.Linear) => DivideLinearLinear(left, right),
            _ => throw new InvalidOperationException(
                $"Operation {operation} is not supported for unit families {left.Family}, {right.Family}.")
        };
    }

    private static Unit<T> ReturnLeftForSameDimension(Unit<T> left, Unit<T> right)
    {
        EnsureSameDimension(left, right);
        return left;
    }

    private static Unit<T> SubtractAffineAffine(Unit<T> left, Unit<T> right)
    {
        EnsureSameDimension(left, right);

        var affine = left.To<AffineUnit<T>, T>();
        return new LinearUnit<T>(left.Dimension, affine.Scale);
    }

    private static Unit<T> MultiplyLinearLinear(Unit<T> left, Unit<T> right)
    {
        var l = left.To<LinearUnit<T>, T>();
        var r = right.To<LinearUnit<T>, T>();
        return new LinearUnit<T>(l.Dimension + r.Dimension, l.Scale * r.Scale);
    }

    private static Unit<T> DivideLinearLinear(Unit<T> left, Unit<T> right)
    {
        var l = left.To<LinearUnit<T>, T>();
        var r = right.To<LinearUnit<T>, T>();
        return new LinearUnit<T>(l.Dimension - r.Dimension, l.Scale / r.Scale);
    }

    private static void EnsureSameDimension(Unit<T> left, Unit<T> right)
    {
        if (!left.IsSameDimension(right))
        {
            throw new InvalidOperationException(
                $"Operation is only valid for units with the same dimension. Left unit: {left.Dimension}, right unit: {right.Dimension}.");
        }
    }
}
