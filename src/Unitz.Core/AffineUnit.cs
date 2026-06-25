using System.Numerics;

namespace Unitz.Core;

/// <summary>
/// Represents an affine unit of measure.
/// </summary>
/// <remarks>
/// An affine unit is converted to the base unit using:
/// <c>baseValue = (value * Scale) + Offset</c>.
/// Such units have an offset relative to the zero of the base scale.
/// </remarks>
public class AffineUnit<T> : Unit<T>, IEquatable<AffineUnit<T>>
where T : struct, INumber<T>
{

    /// <summary>
    /// Initializes a new instance of the <see cref="AffineUnit{T}"/> class.
    /// </summary>
    /// <param name="dimension">The unit dimension.</param>
    /// <param name="scale">The scale factor relative to the base unit.</param>
    /// <param name="offset">The offset relative to the zero of the base unit.</param>
    protected internal AffineUnit(Dimension dimension, T? scale = null, T? offset = null)
        : base(dimension, UnitFamily.Affine)
    {
        if (scale == T.Zero)
        {
            throw new ArgumentException("Scale cannot be equal to 0.", nameof(scale));
        }
        
        this.Scale = scale ?? T.One;
        this.Offset = offset ?? T.Zero;
    }

    /// <summary>
    /// Gets the scale factor relative to the base unit.
    /// </summary>
    public T Scale { get; }

    /// <summary>
    /// Gets the offset relative to the zero of the base unit.
    /// </summary>
    public T Offset { get; }

    /// <summary>
    /// Determines whether two affine units of measure are equal.
    /// </summary>
    /// <param name="other">The other affine unit of measure.</param>
    /// <returns><c>true</c> if the units are equal; otherwise, <c>false</c>.</returns>
    public bool Equals(AffineUnit<T>? other)
    {
        return this.Equals((Unit<T>?)other);
    }

    /// <inheritdoc />
    public override T ToBaseValue(T value)
    {
        return (value * this.Scale) + this.Offset;
    }

    /// <inheritdoc />
    public override T FromBaseValue(T value)
    {
        return (value - this.Offset) / this.Scale;
    }
    
    /// <inheritdoc />
    protected override bool EqualsCore(IUnit other)
    {
        if (other is AffineUnit<T> unit)
        {
            return this.Scale.Equals(unit.Scale) &&
                   this.Offset.Equals(unit.Offset);
        }
        return false;
    }

    /// <inheritdoc />
    protected override int GetHashCodeCore()
    {
        return HashCode.Combine(this.Scale, this.Offset);
    }
}
