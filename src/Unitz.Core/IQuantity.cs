using System.Numerics;

namespace Unitz.Core;

/// <summary>
/// Represents a quantity with an arbitrary unit of measure.
/// </summary>
/// <typeparam name="T">The numeric value type.</typeparam>
public interface IQuantity<T>
where T: struct, INumber<T>
{
    /// <summary>
    /// Gets the quantity value in its original unit.
    /// </summary>
    T Value { get; }

    /// <summary>
    /// Gets the unit in which the quantity is expressed.
    /// </summary>
    Unit<T> Unit { get; }
    
    /// <summary>
    /// Gets the value expressed in base units.
    /// </summary>
    T BaseValue => this.Unit.ToBaseValue(this.Value);
}

/// <summary>
/// Represents a typed physical quantity, such as length, mass, or time.
/// </summary>
/// <typeparam name="TSelfQuantity">
/// The concrete quantity type implementing this interface.
/// </typeparam>
/// <typeparam name="TUnit">The unit type.</typeparam>
/// <typeparam name="TValue">The numeric value type.</typeparam>
public interface IQuantity<out TSelfQuantity, TValue, out TUnit> : IQuantity<TValue>
    where TSelfQuantity : IQuantity<TSelfQuantity, TValue, TUnit>
    where TUnit : Unit<TValue>, IUnit<TUnit,TValue>
    where TValue : struct, INumber<TValue>
{
    /// <summary>
    /// Gets the unit of the physical quantity.
    /// </summary>
    new TUnit Unit { get; }
}
