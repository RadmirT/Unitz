using Rationalz;
using Unitz;
using Xunit;

namespace Unitz.Core.UnitTests;

public class GeneratedQuantityRationalTests
{
    [Fact]
    public void LengthQuantity_WithRationalValueType_ConvertsToBaseValue()
    {
        var length = new LengthQuantity<Rational<int>>(
            new Rational<int>(1000, 1),
            LengthUnit<Rational<int>>.Millimeter);

        Assert.Equal(new Rational<int>(1, 1), length.BaseValue);
    }

    [Fact]
    public void GeneratedTypedOperation_WithRationalValueType_ComputesExpectedResult()
    {
        var length = new LengthQuantity<Rational<int>>(
            new Rational<int>(10, 1),
            LengthUnit<Rational<int>>.Meter);
        var time = new TimeQuantity<Rational<int>>(
            new Rational<int>(2, 1),
            TimeUnit<Rational<int>>.Second);

        var speed = length / time;

        Assert.IsType<SpeedQuantity<Rational<int>>>(speed);
        Assert.Equal(new Rational<int>(5, 1), speed.Value);
        Assert.Equal(SpeedUnit<Rational<int>>.MeterPerSecond, speed.Unit);
    }
}
