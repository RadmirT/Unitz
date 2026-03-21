using System.Numerics;
using Unitz.Core.UnitOperations.Binary;

namespace Unitz.Core;

/// <summary>
/// Represents a unit of measure for a physical quantity.
/// </summary>
/// <remarks>
/// The base class defines common unit properties: dimension, conversion to and from
/// the base unit, and equality logic.
/// </remarks>
/// <typeparam name="T">The numeric value type.</typeparam>
public abstract class Unit<T> : IEquatable<Unit<T>>, IUnit<T> where T : struct, INumber<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Unit{T}"/> class.
    /// </summary>
    /// <param name="dimension">The unit dimension.</param>
    /// <param name="family">The unit family.</param>
    internal Unit(Dimension dimension, UnitFamily family)
    {
        this.Dimension = dimension;
        this.Family = family;
    }

    /// <summary>
    /// Gets the unit dimension.
    /// </summary>
    public Dimension Dimension { get; }

    /// <summary>
    /// Gets the unit family.
    /// </summary>
    public UnitFamily Family { get; }

    /// <summary>
    /// Multiplies two units of measure and returns the result unit.
    /// </summary>
    /// <param name="left">The left unit.</param>
    /// <param name="right">The right unit.</param>
    /// <returns>The result unit of multiplication.</returns>
    public static Unit<T> operator *(Unit<T>? left, Unit<T>? right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return BinaryUnitOperations<T>.Execute(BinaryUnitOperationType.Multiply, left, right);
    }

    /// <summary>
    /// Divides one unit of measure by another and returns the result unit.
    /// </summary>
    /// <param name="left">The dividend unit.</param>
    /// <param name="right">The divisor unit.</param>
    /// <returns>The result unit of division.</returns>
    public static Unit<T> operator /(Unit<T>? left, Unit<T>? right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return BinaryUnitOperations<T>.Execute(BinaryUnitOperationType.Divide, left, right);
    }

    /// <summary>
    /// Determines whether two units of measure are equal.
    /// </summary>
    /// <param name="left">The left unit.</param>
    /// <param name="right">The right unit.</param>
    /// <returns><c>true</c> if the units are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Unit<T>? left, Unit<T>? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    /// <summary>
    /// Determines whether two units of measure are not equal.
    /// </summary>
    /// <param name="left">The left unit.</param>
    /// <param name="right">The right unit.</param>
    /// <returns><c>true</c> if the units are different; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Unit<T>? left, Unit<T>? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Determines whether two units of measure have the same dimension.
    /// </summary>
    /// <param name="unitA">The first unit to compare.</param>
    /// <param name="unitB">The second unit to compare.</param>
    /// <returns>
    /// <c>true</c> if both units have the same physical dimension; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsSameDimension(IUnit unitA, IUnit unitB)
    {
        ArgumentNullException.ThrowIfNull(unitA);
        ArgumentNullException.ThrowIfNull(unitB);

        return unitA.Dimension == unitB.Dimension;
    }

    /// <summary>
    /// Determines whether this unit is equivalent to another unit.
    /// </summary>
    /// <param name="other">The unit to compare with.</param>
    /// <returns><c>true</c> if the units are equivalent; otherwise, <c>false</c>.</returns>
    public bool Equals(Unit<T>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }
        
        return this.Dimension == other.Dimension
               && this.Family == other.Family
               && this.EqualsCore(other);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Unit<T> u && this.Equals(u);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Family, this.Dimension, this.GetHashCodeCore());
    }

    /// <summary>
    /// Converts a value from this unit to the base unit.
    /// </summary>
    /// <param name="value">The value in this unit.</param>
    /// <returns>The value in the base unit.</returns>
    public abstract T ToBaseValue(T value);

    /// <summary>
    /// Converts a value from this unit to the specified unit.
    /// </summary>
    /// <param name="value">The value in this unit.</param>
    /// <param name="targetUnit">The target unit.</param>
    /// <returns>The value in the target unit.</returns>
    public T ConvertValueTo(T value, IUnit<T> targetUnit)
    {
        ArgumentNullException.ThrowIfNull(targetUnit);

        if (!this.IsSameDimension(targetUnit))
        {
            throw new InvalidOperationException(
                $"Cannot convert a value from unit dimension {this.Dimension} to unit dimension {targetUnit.Dimension}.");
        }

        if (this.Equals(targetUnit))
        {
            return value;
        }

        return targetUnit.FromBaseValue(this.ToBaseValue(value));
    }

    /// <summary>
    /// Determines whether this unit has the same dimension as another unit.
    /// </summary>
    /// <param name="other">The unit to compare with.</param>
    /// <returns>
    /// <c>true</c> if both units have the same physical dimension; otherwise, <c>false</c>.
    /// </returns>
    public bool IsSameDimension(IUnit other) => IsSameDimension(this, other);


    /// <summary>
    /// Determines whether this unit has the specified dimension.
    /// </summary>
    /// <param name="dimension">The dimension to compare with.</param>
    /// <returns>
    /// <c>true</c> if the unit has the specified physical dimension; otherwise, <c>false</c>.
    /// </returns>
    public bool IsSameDimension(Dimension dimension) => this.Dimension == dimension;

    /// <summary>
    /// Determines whether this unit is equivalent to another unit.
    /// </summary>
    /// <param name="other">The unit to compare with.</param>
    /// <returns><c>true</c> if the units are equivalent; otherwise, <c>false</c>.</returns>
    public bool Equals(IUnit other)
    {
        return other is not null
               && this.Dimension == other.Dimension
               && this.Family == other.Family
               && this.EqualsCore(other);
    }

    /// <summary>
    /// Converts a value from the base unit to this unit.
    /// </summary>
    /// <param name="value">The value in the base unit.</param>
    /// <returns>The value in this unit.</returns>
    public abstract T FromBaseValue(T value);

    /// <summary>
    /// Compares fields specific to derived unit implementations.
    /// </summary>
    /// <param name="other">The unit to compare with.</param>
    /// <returns><c>true</c> if the fields are equal; otherwise, <c>false</c>.</returns>
    protected abstract bool EqualsCore(IUnit other);

    /// <summary>
    /// Gets the hash code for fields specific to derived unit implementations.
    /// </summary>
    /// <returns>The hash code.</returns>
    protected abstract int GetHashCodeCore();
}
