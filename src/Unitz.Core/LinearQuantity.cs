using System.Numerics;
using Unitz.Core.UnitOperations.Binary;
using Unitz.Core.UnitOperations.Scalar;

namespace Unitz.Core;

/// <summary>
/// Represents a quantity whose unit is converted to the base unit by a linear scale factor.
/// </summary>
/// <typeparam name="TSelf">The concrete quantity type.</typeparam>
/// <typeparam name="TValue">The numeric value type.</typeparam>
/// <typeparam name="TUnit">The linear unit type.</typeparam>
public abstract class LinearQuantity<TSelf, TValue, TUnit> : Quantity<TValue, TUnit>, IQuantity<TSelf, TValue, TUnit>
    where TUnit : LinearUnit<TValue>, IUnit<TUnit, TValue>
    where TValue : struct, INumber<TValue>
    where TSelf : IQuantity<TSelf, TValue, TUnit>
{
    /// <summary>
    /// Initializes a new linear quantity instance.
    /// </summary>
    /// <param name="value">The numeric quantity value.</param>
    /// <param name="unit">The linear unit of measure.</param>
    protected LinearQuantity(TValue value, TUnit unit)
        : base(value, unit)
    {
    }
    
    /// <summary>
    /// Multiplies a quantity by a scalar.
    /// </summary>
    /// <param name="quantity">The source quantity.</param>
    /// <param name="factor">The multiplication factor.</param>
    /// <returns>A new quantity with the same unit.</returns>
    public static TSelf operator *(LinearQuantity<TSelf, TValue, TUnit> quantity, TValue factor)
    {
        ArgumentNullException.ThrowIfNull(quantity);

        var resultUnit = ScalarUnitOperations<TValue>.Execute(ScalarUnitOperationType.MultiplyByScalar, quantity.Unit);
        return quantity.Create(quantity.Value * factor, TUnit.From(resultUnit));
    }

    /// <summary>
    /// Multiplies a quantity by a scalar.
    /// </summary>
    /// <param name="quantity">The source quantity.</param>
    /// <param name="factor">The multiplication factor.</param>
    /// <returns>A new quantity with the same unit.</returns>
    public static TSelf operator *(TValue factor, LinearQuantity<TSelf, TValue, TUnit> quantity)
    {
        return quantity * factor;
    }

    /// <summary>
    /// Divides a quantity by a scalar.
    /// </summary>
    /// <param name="quantity">The source quantity.</param>
    /// <param name="divisor">The division factor.</param>
    /// <returns>A new quantity with the same unit.</returns>
    public static TSelf operator /(LinearQuantity<TSelf, TValue, TUnit> quantity, TValue divisor)
    {
        ArgumentNullException.ThrowIfNull(quantity);

        if (divisor == TValue.Zero)
        {
            throw new DivideByZeroException();
        }

        var resultUnit = ScalarUnitOperations<TValue>.Execute(ScalarUnitOperationType.DivideByScalar, quantity.Unit);
        return quantity.Create(quantity.Value / divisor, TUnit.From(resultUnit));
    }

    /// <summary>
    /// Subtracts one linear quantity from another.
    /// </summary>
    /// <param name="left">The minuend quantity.</param>
    /// <param name="right">The subtrahend quantity.</param>
    /// <returns>A new quantity with the result unit of subtraction.</returns>
    public static TSelf operator -(LinearQuantity<TSelf, TValue, TUnit> left, LinearQuantity<TSelf, TValue, TUnit> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        var resultUnit = TUnit.From(BinaryUnitOperations<TValue>.Execute(BinaryUnitOperationType.Subtract, left.Unit, right.Unit));
        var resultValue = resultUnit.FromBaseValue(left.BaseValue - right.BaseValue);

        return left.Create(resultValue, resultUnit);
    }

    /// <summary>
    /// Adds two linear quantities.
    /// </summary>
    /// <param name="left">The first addend.</param>
    /// <param name="right">The second addend.</param>
    /// <returns>A new quantity with the result unit of addition.</returns>
    public static TSelf operator +(LinearQuantity<TSelf, TValue, TUnit> left, LinearQuantity<TSelf, TValue, TUnit> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        var resultUnit = TUnit.From(BinaryUnitOperations<TValue>.Execute(BinaryUnitOperationType.Add, left.Unit, right.Unit));
        var resultValue = resultUnit.FromBaseValue(left.BaseValue + right.BaseValue);

        return left.Create(resultValue, resultUnit);
    }

    
    /// <summary>
    /// Creates a new quantity with the specified value and unit.
    /// </summary>
    /// <param name="value">The quantity value.</param>
    /// <param name="unit">The unit of measure.</param>
    /// <returns>A new quantity instance.</returns>
    protected abstract TSelf Create(TValue value, TUnit unit);
}
