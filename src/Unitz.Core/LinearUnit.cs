using System.Numerics;

namespace Unitz.Core;

/// <summary>
/// Represents a linear unit of measure.
/// </summary>
/// <remarks>
/// A linear unit is converted to the base unit using:
/// <c>baseValue = value * Scale</c>.
/// </remarks>
public class LinearUnit<T> : Unit<T>, IEquatable<LinearUnit<T>>
where  T : struct, INumber<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinearUnit{T}"/> class.
    /// </summary>
    /// <param name="dimension">The unit dimension.</param>
    public LinearUnit(Dimension dimension)
        : this(dimension, T.One)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinearUnit{T}"/> class.
    /// </summary>
    /// <param name="dimension">The unit dimension.</param>
    /// <param name="scale">The scale factor relative to the base unit.</param>
    public LinearUnit(Dimension dimension, T scale)
        : base(dimension, UnitFamily.Linear)
    {
        if (scale == T.Zero)
        {
            throw new ArgumentException("Scale cannot be equal to 0.", nameof(scale));
        }

        this.Scale = scale;
    }

    /// <summary>
    /// Gets the scale factor relative to the base unit.
    /// </summary>
    public T Scale { get; }
    
    /// <summary>
    /// Determines whether two linear units of measure are equal.
    /// </summary>
    /// <param name="other">The other linear unit of measure.</param>
    /// <returns><c>true</c> if the units are equal; otherwise, <c>false</c>.</returns>
    public bool Equals(LinearUnit<T>? other)
    {
        return this.Equals((Unit<T>?)other);
    }

    /// <inheritdoc />
    public override T ToBaseValue(T value)
    {
        return value * this.Scale;
    }

    /// <inheritdoc />
    public override T FromBaseValue(T value)
    {
        return value / this.Scale;
    }

    /// <inheritdoc />
    protected override bool EqualsCore(IUnit other)
    {
        if (other is LinearUnit<T> unit)
        {
            return this.Scale.Equals(unit.Scale);
        }
        return false;
    }


    /// <inheritdoc />
    protected override int GetHashCodeCore()
    {
        return this.Scale.GetHashCode();
    }
}
