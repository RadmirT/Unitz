using System.Numerics;
using Unitz.Core.UnitOperations.Binary;

namespace Unitz.Core;

/// <summary>
/// Represents a quantity whose unit is converted to the base unit by a scale factor and an offset.
/// </summary>
/// <typeparam name="TSelf">The concrete quantity type.</typeparam>
/// <typeparam name="TDelta">The linear quantity type that represents a difference between two affine quantities.</typeparam>
/// <typeparam name="TValue">The numeric value type.</typeparam>
/// <typeparam name="TUnit">The affine unit type.</typeparam>
/// <typeparam name="TDeltaUnit">The linear unit type used by the difference quantity.</typeparam>
public abstract class AffineQuantity<TSelf, TDelta, TValue, TUnit, TDeltaUnit> : Quantity<TValue, TUnit>, IQuantity<TSelf, TValue, TUnit>
    where TUnit : AffineUnit<TValue>, IUnit<TUnit, TValue>
    where TDeltaUnit : LinearUnit<TValue>, IUnit<TDeltaUnit, TValue>
    where TValue : struct, INumber<TValue>
    where TSelf : IQuantity<TSelf, TValue, TUnit>
    where TDelta : IQuantity<TDelta, TValue, TDeltaUnit>
{
    /// <summary>
    /// Initializes a new affine quantity instance.
    /// </summary>
    /// <param name="value">The numeric quantity value.</param>
    /// <param name="unit">The affine unit of measure.</param>
    protected AffineQuantity(TValue value, TUnit unit)
        : base(value, unit)
    {
    }

    /// <summary>
    /// Subtracts one affine quantity from another and returns a linear difference.
    /// </summary>
    /// <param name="left">The minuend quantity.</param>
    /// <param name="right">The subtrahend quantity.</param>
    /// <returns>A linear quantity representing the difference between the two affine quantities.</returns>
    public static TDelta operator -(AffineQuantity<TSelf, TDelta, TValue, TUnit, TDeltaUnit> left, AffineQuantity<TSelf, TDelta, TValue, TUnit, TDeltaUnit> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        var resultUnit = TDeltaUnit.From(BinaryUnitOperations<TValue>.Execute(BinaryUnitOperationType.Subtract, left.Unit, right.Unit));
        var resultValue = resultUnit.FromBaseValue(left.BaseValue - right.BaseValue);

        return left.CreateDelta(resultValue, resultUnit);
    }

    /// <summary>
    /// Adds a linear difference to an affine quantity.
    /// </summary>
    /// <param name="left">The affine quantity.</param>
    /// <param name="right">The linear difference quantity.</param>
    /// <returns>A new affine quantity with the result unit of addition.</returns>
    public static TSelf operator +(AffineQuantity<TSelf, TDelta, TValue, TUnit, TDeltaUnit> left, TDelta right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        var resultUnit = TUnit.From(BinaryUnitOperations<TValue>.Execute(BinaryUnitOperationType.Add, left.Unit, right.Unit));
        var resultValue = resultUnit.FromBaseValue(left.BaseValue + right.BaseValue);

        return left.Create(resultValue, resultUnit);
    }

    /// <summary>
    /// Subtracts a linear difference from an affine quantity.
    /// </summary>
    /// <param name="left">The affine quantity.</param>
    /// <param name="right">The linear difference quantity.</param>
    /// <returns>A new affine quantity with the result unit of subtraction.</returns>
    public static TSelf operator -(AffineQuantity<TSelf, TDelta, TValue, TUnit, TDeltaUnit> left, TDelta right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        var resultUnit = TUnit.From(BinaryUnitOperations<TValue>.Execute(BinaryUnitOperationType.Subtract, left.Unit, right.Unit));
        var resultValue = resultUnit.FromBaseValue(left.BaseValue - right.BaseValue);

        return left.Create(resultValue, resultUnit);
    }

    /// <summary>
    /// Creates a new quantity with the specified value and unit.
    /// </summary>
    /// <param name="value">The quantity value.</param>
    /// <param name="unit">The unit of measure.</param>
    /// <returns>A new quantity instance.</returns>
    protected abstract TSelf Create(TValue value, TUnit unit);

    /// <summary>
    /// Creates a new linear quantity that represents a difference between affine quantities.
    /// </summary>
    /// <param name="value">The difference value.</param>
    /// <param name="unit">The linear unit of measure.</param>
    /// <returns>A new linear difference quantity instance.</returns>
    protected abstract TDelta CreateDelta(TValue value, TDeltaUnit unit);
}
