namespace Unitz.Core;

/// <summary>
/// Marks a partial class as a generated linear quantity.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class LinearQuantityAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the base unit name.
    /// </summary>
    public string Base { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the length exponent.
    /// </summary>
    public int L { get; set; }

    /// <summary>
    /// Gets or sets the mass exponent.
    /// </summary>
    public int M { get; set; }

    /// <summary>
    /// Gets or sets the time exponent.
    /// </summary>
    public int T { get; set; }

    /// <summary>
    /// Gets or sets the electric current exponent.
    /// </summary>
    public int I { get; set; }

    /// <summary>
    /// Gets or sets the thermodynamic temperature exponent.
    /// </summary>
    public int Th { get; set; }

    /// <summary>
    /// Gets or sets the amount of substance exponent.
    /// </summary>
    public int N { get; set; }

    /// <summary>
    /// Gets or sets the luminous intensity exponent.
    /// </summary>
    public int J { get; set; }
}

/// <summary>
/// Marks a partial class as a generic quantity specification.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class GenericLinearQuantityAttribute : LinearQuantityAttribute
{
}

/// <summary>
/// Marks a partial class as a generated affine quantity.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AffineQuantityAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the base unit name.
    /// </summary>
    public string Base { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the length exponent.
    /// </summary>
    public int L { get; set; }

    /// <summary>
    /// Gets or sets the mass exponent.
    /// </summary>
    public int M { get; set; }

    /// <summary>
    /// Gets or sets the time exponent.
    /// </summary>
    public int T { get; set; }

    /// <summary>
    /// Gets or sets the electric current exponent.
    /// </summary>
    public int I { get; set; }

    /// <summary>
    /// Gets or sets the thermodynamic temperature exponent.
    /// </summary>
    public int Th { get; set; }

    /// <summary>
    /// Gets or sets the amount of substance exponent.
    /// </summary>
    public int N { get; set; }

    /// <summary>
    /// Gets or sets the luminous intensity exponent.
    /// </summary>
    public int J { get; set; }
}

/// <summary>
/// Marks a partial class as a generic affine quantity specification.
/// Concrete generated quantity classes are declared with <see cref="ConcreteAttribute{TConcrete}"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class GenericAffineQuantityAttribute : AffineQuantityAttribute
{
}

/// <summary>
/// Adds SI-prefixed linear units to a generated linear quantity.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class SiLinearUnitsAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SiLinearUnitsAttribute"/> class.
    /// </summary>
    /// <param name="prefixes">The SI prefix exponents.</param>
    public SiLinearUnitsAttribute(params int[] prefixes)
    {
        this.Prefixes = prefixes;
    }

    /// <summary>
    /// Gets the SI prefix exponents.
    /// </summary>
    public IReadOnlyList<int> Prefixes { get; }
}

/// <summary>
/// Adds a named linear unit to a generated linear quantity.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class LinearUnitsAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinearUnitsAttribute"/> class.
    /// </summary>
    /// <param name="name">The unit name.</param>
    public LinearUnitsAttribute(string name)
    {
        this.Name = name;
    }

    /// <summary>
    /// Gets the unit name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the scale relative to the base unit.
    /// </summary>
    public double Scale { get; set; } = 1;
}

/// <summary>
/// Adds a named affine unit to a generated affine quantity.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AffineUnitsAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AffineUnitsAttribute"/> class.
    /// </summary>
    /// <param name="name">The unit name.</param>
    public AffineUnitsAttribute(string name)
    {
        this.Name = name;
    }

    /// <summary>
    /// Gets the unit name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the scale relative to the base unit.
    /// </summary>
    public double Scale { get; set; } = 1;

    /// <summary>
    /// Gets or sets the offset relative to the base unit zero.
    /// </summary>
    public double Offset { get; set; }
}

/// <summary>
/// Adds a typed multiplication operation to a generated quantity.
/// </summary>
/// <typeparam name="TOperand">The right operand quantity type.</typeparam>
/// <typeparam name="TResult">The result quantity type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MultiplyOperationAttribute<TOperand, TResult> : Attribute
{
}

/// <summary>
/// Adds a typed division operation to a generated quantity.
/// </summary>
/// <typeparam name="TOperand">The right operand quantity type.</typeparam>
/// <typeparam name="TResult">The result quantity type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DivideOperationAttribute<TOperand, TResult> : Attribute
{
}
