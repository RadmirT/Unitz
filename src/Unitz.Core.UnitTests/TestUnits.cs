namespace Unitz.Core.UnitTests;

using System.Numerics;
using Unitz.Core;

internal sealed class TestLengthUnit<T> : LinearUnit<T>, IUnit<TestLengthUnit<T>, T>
    where T : struct, INumber<T>
{
    public static readonly Dimension LengthDimension = new(L: 1);

    private TestLengthUnit(T scale)
        : base(LengthDimension, scale)
    {
    }

    internal static TestLengthUnit<T> Meter { get; } = new(T.One);

    internal static TestLengthUnit<T> Kilometer { get; } = new(T.CreateChecked(1000));

    internal static TestLengthUnit<T> Centimeter { get; } = new(T.One / T.CreateChecked(100));

    public static TestLengthUnit<T> From(Unit<T> unit)
    {
        ArgumentNullException.ThrowIfNull(unit);

        if (unit.Dimension != LengthDimension)
        {
            throw new ArgumentException("Unit dimension is not length.", nameof(unit));
        }

        return unit as TestLengthUnit<T> ?? new TestLengthUnit<T>(unit.To<LinearUnit<T>, T>().Scale);
    }
}

internal sealed class TestTimeUnit<T> : LinearUnit<T>, IUnit<TestTimeUnit<T>, T>
    where T : struct, INumber<T>
{
    internal static readonly Dimension TimeDimension = new(T: 1);

    private TestTimeUnit(T scale)
        : base(TimeDimension, scale)
    {
    }

    internal static TestTimeUnit<T> Second { get; } = new(T.One);

    public static TestTimeUnit<T> From(Unit<T> unit)
    {
        ArgumentNullException.ThrowIfNull(unit);

        if (unit.Dimension != TimeDimension)
        {
            throw new ArgumentException("Unit dimension is not time.", nameof(unit));
        }

        return unit as TestTimeUnit<T> ?? new TestTimeUnit<T>(unit.To<LinearUnit<T>, T>().Scale);
    }
}

internal sealed class TestTemperatureUnit<T> : AffineUnit<T>, IUnit<TestTemperatureUnit<T>, T>
    where T : struct, INumber<T>
{
    internal static readonly Dimension TemperatureDimension = new(Th: 1);

    private TestTemperatureUnit(T scale, T offset)
        : base(TemperatureDimension, scale, offset)
    {
    }

    internal static TestTemperatureUnit<T> Kelvin { get; } = new(T.One, T.Zero);

    internal static TestTemperatureUnit<T> Celsius { get; } = new(T.One, T.CreateChecked(273.15));

    internal static TestTemperatureUnit<T> Fahrenheit { get; } =
        new(T.CreateChecked(5) / T.CreateChecked(9), T.CreateChecked(255.3722222222222));

    public static TestTemperatureUnit<T> From(Unit<T> unit)
    {
        ArgumentNullException.ThrowIfNull(unit);

        if (unit.Dimension != TemperatureDimension)
        {
            throw new ArgumentException("Unit dimension is not temperature.", nameof(unit));
        }

        var affine = unit.To<AffineUnit<T>, T>();
        return unit as TestTemperatureUnit<T> ?? new TestTemperatureUnit<T>(affine.Scale, affine.Offset);
    }
}

internal sealed class TestTemperatureDeltaUnit<T> : LinearUnit<T>, IUnit<TestTemperatureDeltaUnit<T>, T>
    where T : struct, INumber<T>
{
    private TestTemperatureDeltaUnit(T scale)
        : base(TestTemperatureUnit<T>.TemperatureDimension, scale)
    {
    }

    internal static TestTemperatureDeltaUnit<T> Kelvin { get; } = new(T.One);

    internal static TestTemperatureDeltaUnit<T> Celsius { get; } = new(T.One);

    internal static TestTemperatureDeltaUnit<T> Fahrenheit { get; } =
        new(T.CreateChecked(5) / T.CreateChecked(9));

    public static TestTemperatureDeltaUnit<T> From(Unit<T> unit)
    {
        ArgumentNullException.ThrowIfNull(unit);

        if (unit.Dimension != TestTemperatureUnit<T>.TemperatureDimension)
        {
            throw new ArgumentException("Unit dimension is not temperature.", nameof(unit));
        }

        var linear = unit.To<LinearUnit<T>, T>();
        return unit as TestTemperatureDeltaUnit<T> ?? new TestTemperatureDeltaUnit<T>(linear.Scale);
    }
}
