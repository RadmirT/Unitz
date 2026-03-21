using System.Numerics;

namespace Unitz.Core.UnitOperations.Scalar;

internal static class ScalarUnitOperations<T> where T : struct, INumber<T>
{
    public static Unit<T> Execute(ScalarUnitOperationType operation, Unit<T> unit)
    {
        ArgumentNullException.ThrowIfNull(unit);

        return (operation, unit.Family) switch
        {
            (ScalarUnitOperationType.MultiplyByScalar, UnitFamily.Linear) => unit,
            (ScalarUnitOperationType.DivideByScalar, UnitFamily.Linear) => unit,
            (ScalarUnitOperationType.ScalarDivideByUnit, UnitFamily.Linear) => DivideScalarByLinear(unit),
            (ScalarUnitOperationType.MultiplyByScalar, UnitFamily.Affine) =>
                throw new InvalidOperationException("Affine units do not support multiplication by a scalar."),
            (ScalarUnitOperationType.DivideByScalar, UnitFamily.Affine) =>
                throw new InvalidOperationException("Affine units do not support division by a scalar."),
            _ => throw new InvalidOperationException(
                $"Operation {operation} is not supported for unit family {unit.Family}.")
        };
    }

    private static Unit<T> DivideScalarByLinear(Unit<T> unit)
    {
        var linear = unit.To<LinearUnit<T>, T>();
        return new LinearUnit<T>(
            Dimension.Dimensionless - linear.Dimension,
            T.One / linear.Scale);
    }
}
