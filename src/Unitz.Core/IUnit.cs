using System.Numerics;

namespace Unitz.Core;

/// <summary>
/// Represents a unit of measure.
/// </summary>
public interface IUnit
{
    /// <summary>
    /// Gets the unit dimension.
    /// </summary>
    Dimension Dimension { get; }

    /// <summary>
    /// Gets the unit family.
    /// </summary>
    UnitFamily Family { get; }

    /// <summary>
    /// Determines whether this unit has the same dimension as another unit.
    /// </summary>
    /// <param name="other">The unit to compare with.</param>
    /// <returns>
    /// <c>true</c> if both units have the same physical dimension; otherwise, <c>false</c>.
    /// </returns>
    bool IsSameDimension(IUnit other);

    /// <summary>
    /// Determines whether this unit has the specified dimension.
    /// </summary>
    /// <param name="dimension">The dimension to compare with.</param>
    /// <returns>
    /// <c>true</c> if the unit has the specified physical dimension; otherwise, <c>false</c>.
    /// </returns>
    bool IsSameDimension(Dimension dimension);
    
    /// <summary>
    /// Determines whether this unit is equivalent to another unit.
    /// </summary>
    /// <param name="other">The unit to compare with.</param>
    /// <returns><c>true</c> if the units are equivalent; otherwise, <c>false</c>.</returns>
    bool Equals(IUnit other);

}

/// <summary>
/// Represents a unit of measure for the specified numeric value type.
/// </summary>
/// <typeparam name="T">The numeric value type.</typeparam>
public interface IUnit<T> : IUnit where T : INumber<T>
{
   
    /// <summary>
    /// Converts a value from this unit to the base unit.
    /// </summary>
    /// <param name="value">The value in this unit.</param>
    /// <returns>The value in the base unit.</returns>
    T ToBaseValue(T value);

    /// <summary>
    /// Converts a value from this unit to the specified unit.
    /// </summary>
    /// <param name="value">The value in this unit.</param>
    /// <param name="targetUnit">The target unit.</param>
    /// <returns>The value in the target unit.</returns>
    T ConvertValueTo(T value, IUnit<T> targetUnit);

    /// <summary>
    /// Converts a value from the base unit to this unit.
    /// </summary>
    /// <param name="value">The value in the base unit.</param>
    /// <returns>The value in this unit.</returns>
    T FromBaseValue(T value);
}

/// <summary>
/// Represents a typed unit that can be created from a general unit.
/// </summary>
/// <typeparam name="TSelf">The concrete unit type.</typeparam>
/// <typeparam name="T">The numeric value type.</typeparam>
public interface IUnit<TSelf, T> where TSelf : Unit<T>, IUnit<TSelf, T>
    where T : struct, INumber<T>
{
    /// <summary>
    /// Creates a typed unit from a general unit.
    /// </summary>
    /// <param name="unit">The unit of measure.</param>
    /// <returns>The typed unit of measure.</returns>
    static abstract TSelf From(Unit<T> unit);
}
