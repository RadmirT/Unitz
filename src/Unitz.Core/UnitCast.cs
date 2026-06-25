using System.Numerics;

namespace Unitz.Core;

/// <summary>
/// Contains helper methods for casting units to concrete types.
/// </summary>
public static class UnitCast
{
    /// <summary>
    /// Casts a unit of measure to the specified type.
    /// </summary>
    /// <typeparam name="T">The target unit type.</typeparam>
    /// <typeparam name="TValue">The numeric value type.</typeparam>
    /// <param name="unit">The unit of measure.</param>
    /// <returns>The unit of the specified type.</returns>
    public static T To<T, TValue>(this Unit<TValue> unit) 
        where T : Unit<TValue> 
        where TValue : struct, INumber<TValue>
    {
        if (unit is not T typed)
        {
            throw new InvalidOperationException(
                $"Expected type {typeof(T).Name}, got {unit.GetType().Name}.");
        }

        return typed;
    }
}
