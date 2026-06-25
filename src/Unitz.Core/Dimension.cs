namespace Unitz.Core;

/// <summary>
/// Describes the dimensional exponents of a unit of measure.
/// </summary>
/// <param name="L">Length exponent (L).</param>
/// <param name="M">Mass exponent (M).</param>
/// <param name="T">Time exponent (T).</param>
/// <param name="I">Electric current exponent (I).</param>
/// <param name="Th">Thermodynamic temperature exponent (Th).</param>
/// <param name="N">Amount of substance exponent (N).</param>
/// <param name="J">Luminous intensity exponent (J).</param>
public readonly record struct Dimension(int L = 0, int M = 0, int T = 0, int I = 0, int Th = 0, int N = 0, int J = 0)
{
    /// <summary>
    /// Dimensionless value where all exponents are zero.
    /// </summary>
    public static readonly Dimension Dimensionless = new(0, 0, 0, 0, 0, 0, 0);

    /// <summary>
    /// Gets a value indicating whether this dimension is dimensionless.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the dimension is dimensionless; otherwise, <see langword="false"/>.
    /// </value>
    public bool IsDimensionless => this == Dimensionless;

    /// <summary>Adds two <see cref="Dimension"/> values.</summary>
    /// <param name="a">The first <see cref="Dimension"/> operand.</param>
    /// <param name="b">The second <see cref="Dimension"/> operand.</param>
    /// <returns>A new <see cref="Dimension"/> value.</returns>
    /// <remarks>This is dimension algebra, for example L + L = 2L or I + T = IT.</remarks>
    public static Dimension operator +(Dimension a, Dimension b)
    {
        return new Dimension(
            a.L + b.L,
            a.M + b.M,
            a.T + b.T,
            a.I + b.I,
            a.Th + b.Th,
            a.N + b.N,
            a.J + b.J);
    }
    
    /// <summary>Subtracts one <see cref="Dimension"/> value from another.</summary>
    /// <param name="a">The first <see cref="Dimension"/> operand.</param>
    /// <param name="b">The second <see cref="Dimension"/> operand.</param>
    /// <returns>A new <see cref="Dimension"/> value.</returns>
    /// <remarks>This is dimension algebra, for example 2L - L = L or IT - T = I.</remarks>
    public static Dimension operator -(Dimension a, Dimension b)
    {
        return new Dimension(
            a.L - b.L,
            a.M - b.M,
            a.T - b.T,
            a.I - b.I,
            a.Th - b.Th,
            a.N - b.N,
            a.J - b.J);
    }
}
