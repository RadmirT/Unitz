using System.Numerics;

namespace Unitz.Core;

/// <summary>
/// Represents a quantity with the specified unit type.
/// </summary>
/// <typeparam name="TValue">The numeric value type.</typeparam>
/// <typeparam name="TUnit">The unit type.</typeparam>
public class Quantity<TValue, TUnit> : IQuantity<TValue>
    where TValue : struct, INumber<TValue>
    where TUnit : Unit<TValue>, IUnit<TUnit, TValue>
{
    /// <summary>
    /// Initializes a new quantity instance.
    /// </summary>
    /// <param name="value">The numeric quantity value.</param>
    /// <param name="unit">The unit of measure.</param>
    public Quantity(TValue value, TUnit unit)
    {
        ArgumentNullException.ThrowIfNull(unit);

        this.Value = value;
        this.Unit = unit;
    }

    /// <summary>
    /// Gets the quantity value in its original unit.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// Gets the unit in which the quantity is expressed.
    /// </summary>
    public TUnit Unit { get; }

    Unit<TValue> IQuantity<TValue>.Unit => this.Unit;

    /// <summary>
    /// Gets the quantity value converted to the base unit.
    /// </summary>
    public TValue BaseValue => this.Unit.ToBaseValue(this.Value);

    /// <summary>
    /// Converts this quantity to another typed unit.
    /// </summary>
    /// <typeparam name="TOtherUnit">The target unit type.</typeparam>
    /// <param name="targetUnit">The target unit.</param>
    /// <returns>A new quantity expressed in the target unit.</returns>
    public Quantity<TValue, TOtherUnit> To<TOtherUnit>(TOtherUnit targetUnit)
        where TOtherUnit : Unit<TValue>, IUnit<TOtherUnit, TValue>
    {
        ArgumentNullException.ThrowIfNull(targetUnit);

        var convertedValue = this.Unit.ConvertValueTo(this.Value, targetUnit);
        return new Quantity<TValue, TOtherUnit>(convertedValue, targetUnit);
    }

    /// <summary>
    /// Divides one quantity by another and returns a new quantity.
    /// </summary>
    /// <param name="left">The dividend quantity.</param>
    /// <param name="right">The divisor quantity.</param>
    /// <returns>A new quantity with the result unit of division.</returns>
    public static Quantity<TValue> operator /(Quantity<TValue, TUnit> left, Quantity<TValue, TUnit> right)
    {
        return QuantityOperationCore<TValue>.Divide(left, right);
    }

    /// <summary>
    /// Multiplies two quantities and returns a new quantity.
    /// </summary>
    /// <param name="left">The first factor.</param>
    /// <param name="right">The second factor.</param>
    /// <returns>A new quantity with the result unit of multiplication.</returns>
    public static Quantity<TValue> operator *(Quantity<TValue, TUnit> left, Quantity<TValue, TUnit> right)
    {
        return QuantityOperationCore<TValue>.Multiply(left, right);
    }

    /// <summary>
    /// Adds two quantities and returns a new quantity.
    /// </summary>
    /// <param name="left">The first addend.</param>
    /// <param name="right">The second addend.</param>
    /// <returns>A new quantity with the result unit of addition.</returns>
    public static Quantity<TValue> operator +(Quantity<TValue, TUnit> left, Quantity<TValue, TUnit> right)
    {
        return QuantityOperationCore<TValue>.Add(left, right);
    }

    /// <summary>
    /// Subtracts one quantity from another and returns a new quantity.
    /// </summary>
    /// <param name="left">The minuend quantity.</param>
    /// <param name="right">The subtrahend quantity.</param>
    /// <returns>A new quantity with the result unit of subtraction.</returns>
    public static Quantity<TValue> operator -(Quantity<TValue, TUnit> left, Quantity<TValue, TUnit> right)
    {
        return QuantityOperationCore<TValue>.Subtract(left, right);
    }
}

/// <summary>
/// Represents a quantity with an arbitrary unit of measure.
/// </summary>
/// <typeparam name="T">The numeric value type.</typeparam>
public class Quantity<T> : IQuantity<T> where T : struct, INumber<T>
{
    /// <summary>
    /// Initializes a new quantity instance.
    /// </summary>
    /// <param name="value">The numeric quantity value.</param>
    /// <param name="unit">The unit of measure.</param>
    public Quantity(T value, Unit<T> unit)
    {
        ArgumentNullException.ThrowIfNull(unit);

        this.Value = value;
        this.Unit = unit;
    }

    /// <summary>
    /// Gets the quantity value in its original unit.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Gets the unit in which the quantity is expressed.
    /// </summary>
    public Unit<T> Unit { get; }

    /// <summary>
    /// Gets the quantity value converted to the base unit.
    /// </summary>
    public T BaseValue => this.Unit.ToBaseValue(this.Value);

    /// <summary>
    /// Divides one quantity by another and returns a new quantity
    /// with the corresponding result dimension.
    /// </summary>
    /// <param name="left">The dividend quantity.</param>
    /// <param name="right">The divisor quantity.</param>
    /// <returns>
    /// A new <see cref="Quantity{T}"/> instance representing the division result.
    /// </returns>
    public static Quantity<T> operator /(Quantity<T> left, Quantity<T> right)
    {
        return QuantityOperationCore<T>.Divide(left, right);
    }

    /// <summary>
    /// Multiplies two quantities and returns a new quantity
    /// with the corresponding result dimension.
    /// </summary>
    /// <param name="left">The first factor.</param>
    /// <param name="right">The second factor.</param>
    /// <returns>
    /// A new <see cref="Quantity{T}"/> instance representing the product.
    /// </returns>
    public static Quantity<T> operator *(Quantity<T> left, Quantity<T> right)
    {
        return QuantityOperationCore<T>.Multiply(left, right);
    }

    /// <summary>
    /// Adds two quantities and returns a new quantity
    /// with the corresponding result dimension.
    /// </summary>
    /// <param name="left">The first addend.</param>
    /// <param name="right">The second addend.</param>
    /// <returns>
    /// A new <see cref="Quantity{T}"/> instance representing the sum.
    /// </returns>
    public static Quantity<T> operator +(Quantity<T> left, Quantity<T> right)
    {
        return QuantityOperationCore<T>.Add(left, right);
    }
    
    /// <summary>
    /// Subtracts one quantity from another and returns a new quantity
    /// with the corresponding result dimension.
    /// </summary>
    /// <param name="left">The minuend quantity.</param>
    /// <param name="right">The subtrahend quantity.</param>
    /// <returns>
    /// A new <see cref="Quantity{T}"/> instance representing the difference.
    /// </returns>
    public static Quantity<T> operator -(Quantity<T> left, Quantity<T> right)
    {
        return QuantityOperationCore<T>.Subtract(left, right);
    }
}
