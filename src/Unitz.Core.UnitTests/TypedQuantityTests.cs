namespace Unitz.Core.UnitTests;

using Microsoft.CSharp.RuntimeBinder;
using Xunit;

public class TypedQuantityTests
{
    [Fact]
    public void Addition_SameTypedQuantities_ReturnsTypedQuantity()
    {
        var left = new TestLengthQuantity<double>(1, TestLengthUnit<double>.Kilometer);
        var right = new TestLengthQuantity<double>(500, TestLengthUnit<double>.Meter);

        var result = left + right;

        Assert.IsType<TestLengthQuantity<double>>(result);
        Assert.Equal(1.5, result.Value, 12);
        Assert.Same(TestLengthUnit<double>.Kilometer, result.Unit);
    }

    [Fact]
    public void Subtraction_SameTypedQuantities_ReturnsTypedQuantity()
    {
        var left = new TestLengthQuantity<double>(1, TestLengthUnit<double>.Kilometer);
        var right = new TestLengthQuantity<double>(250, TestLengthUnit<double>.Meter);

        var result = left - right;

        Assert.IsType<TestLengthQuantity<double>>(result);
        Assert.Equal(0.75, result.Value, 12);
        Assert.Same(TestLengthUnit<double>.Kilometer, result.Unit);
    }

    [Fact]
    public void Multiplication_ByScalar_ReturnsTypedQuantity()
    {
        var quantity = new TestLengthQuantity<double>(2, TestLengthUnit<double>.Meter);

        var result = quantity * 3;

        Assert.IsType<TestLengthQuantity<double>>(result);
        Assert.Equal(6, result.Value, 12);
        Assert.Same(TestLengthUnit<double>.Meter, result.Unit);
    }

    [Fact]
    public void Division_ByScalar_ReturnsTypedQuantity()
    {
        var quantity = new TestLengthQuantity<double>(6, TestLengthUnit<double>.Meter);

        var result = quantity / 3;

        Assert.IsType<TestLengthQuantity<double>>(result);
        Assert.Equal(2, result.Value, 12);
        Assert.Same(TestLengthUnit<double>.Meter, result.Unit);
    }

    [Fact]
    public void Division_ByZeroScalar_ThrowsDivideByZeroException()
    {
        var quantity = new TestLengthQuantity<double>(6, TestLengthUnit<double>.Meter);

        Assert.Throws<DivideByZeroException>(() => quantity / 0);
    }

    [Fact]
    public void Multiplication_AffineQuantityByScalar_HasNoPublicOperator()
    {
        dynamic quantity = new TestTemperatureQuantity<double>(20, TestTemperatureUnit<double>.Celsius);

        Assert.Throws<RuntimeBinderException>(() => MultiplyByScalar(quantity));
    }

    [Fact]
    public void Division_AffineQuantityByScalar_HasNoPublicOperator()
    {
        dynamic quantity = new TestTemperatureQuantity<double>(20, TestTemperatureUnit<double>.Celsius);

        Assert.Throws<RuntimeBinderException>(() => DivideByScalar(quantity));
    }

    [Fact]
    public void Subtraction_AffineTypedQuantities_ReturnsLinearDifference()
    {
        var left = new TestTemperatureQuantity<double>(20, TestTemperatureUnit<double>.Celsius);
        var right = new TestTemperatureQuantity<double>(10, TestTemperatureUnit<double>.Celsius);

        var result = left - right;

        Assert.IsType<TestTemperatureDeltaQuantity<double>>(result);
        Assert.Equal(10, result.Value, 12);
        Assert.Equal(UnitFamily.Linear, result.Unit.Family);
        Assert.Equal(TestTemperatureUnit<double>.TemperatureDimension, result.Unit.Dimension);
    }

    [Fact]
    public void Addition_AffineAndLinearDifference_ReturnsAffineQuantity()
    {
        var left = new TestTemperatureQuantity<double>(20, TestTemperatureUnit<double>.Celsius);
        var right = new TestTemperatureDeltaQuantity<double>(10, TestTemperatureDeltaUnit<double>.Celsius);

        var result = left + right;

        Assert.IsType<TestTemperatureQuantity<double>>(result);
        Assert.Equal(30, result.Value, 12);
        Assert.Same(TestTemperatureUnit<double>.Celsius, result.Unit);
    }

    [Fact]
    public void Subtraction_AffineAndLinearDifference_ReturnsAffineQuantity()
    {
        var left = new TestTemperatureQuantity<double>(20, TestTemperatureUnit<double>.Celsius);
        var right = new TestTemperatureDeltaQuantity<double>(10, TestTemperatureDeltaUnit<double>.Celsius);

        var result = left - right;

        Assert.IsType<TestTemperatureQuantity<double>>(result);
        Assert.Equal(10, result.Value, 12);
        Assert.Same(TestTemperatureUnit<double>.Celsius, result.Unit);
    }

    [Fact]
    public void Addition_AffineAndFahrenheitDifference_ConvertsDeltaScale()
    {
        var left = new TestTemperatureQuantity<double>(20, TestTemperatureUnit<double>.Celsius);
        var right = new TestTemperatureDeltaQuantity<double>(18, TestTemperatureDeltaUnit<double>.Fahrenheit);

        var result = left + right;

        Assert.IsType<TestTemperatureQuantity<double>>(result);
        Assert.Equal(30, result.Value, 12);
        Assert.Same(TestTemperatureUnit<double>.Celsius, result.Unit);
    }

    [Fact]
    public void Subtraction_AffineAndFahrenheitDifference_ConvertsDeltaScale()
    {
        var left = new TestTemperatureQuantity<double>(20, TestTemperatureUnit<double>.Celsius);
        var right = new TestTemperatureDeltaQuantity<double>(18, TestTemperatureDeltaUnit<double>.Fahrenheit);

        var result = left - right;

        Assert.IsType<TestTemperatureQuantity<double>>(result);
        Assert.Equal(10, result.Value, 12);
        Assert.Same(TestTemperatureUnit<double>.Celsius, result.Unit);
    }

    private static void MultiplyByScalar(dynamic quantity)
    {
        _ = quantity * 2.0;
    }

    private static void DivideByScalar(dynamic quantity)
    {
        _ = quantity / 2.0;
    }
}
