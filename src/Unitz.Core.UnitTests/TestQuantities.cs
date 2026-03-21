namespace Unitz.Core.UnitTests;

using System.Numerics;
using Unitz.Core;

internal sealed class TestLengthQuantity<T> : LinearQuantity<TestLengthQuantity<T>, T, TestLengthUnit<T>>
    where T : struct, INumber<T>
{
    internal TestLengthQuantity(T value, TestLengthUnit<T> unit)
        : base(value, unit)
    {
    }

    protected override TestLengthQuantity<T> Create(T value, TestLengthUnit<T> unit)
    {
        return new TestLengthQuantity<T>(value, unit);
    }
}

internal sealed class TestTemperatureDeltaQuantity<T>
    : LinearQuantity<TestTemperatureDeltaQuantity<T>, T, TestTemperatureDeltaUnit<T>>
    where T : struct, INumber<T>
{
    internal TestTemperatureDeltaQuantity(T value, TestTemperatureDeltaUnit<T> unit)
        : base(value, unit)
    {
    }

    protected override TestTemperatureDeltaQuantity<T> Create(T value, TestTemperatureDeltaUnit<T> unit)
    {
        return new TestTemperatureDeltaQuantity<T>(value, unit);
    }
}

internal sealed class TestTemperatureQuantity<T>
    : AffineQuantity<TestTemperatureQuantity<T>, TestTemperatureDeltaQuantity<T>, T, TestTemperatureUnit<T>, TestTemperatureDeltaUnit<T>>
    where T : struct, INumber<T>
{
    internal TestTemperatureQuantity(T value, TestTemperatureUnit<T> unit)
        : base(value, unit)
    {
    }

    protected override TestTemperatureQuantity<T> Create(T value, TestTemperatureUnit<T> unit)
    {
        return new TestTemperatureQuantity<T>(value, unit);
    }

    protected override TestTemperatureDeltaQuantity<T> CreateDelta(T value, TestTemperatureDeltaUnit<T> unit)
    {
        return new TestTemperatureDeltaQuantity<T>(value, unit);
    }
}
