namespace Unitz.Core.UnitTests;

using Unitz.Core;
using Xunit;

public class QuantityTests
{
    [Fact]
    public void To_TypedQuantityWithDifferentScale_ConvertsValueAndUnit()
    {
        var quantity = new Quantity<double, TestLengthUnit<double>>(1, TestLengthUnit<double>.Kilometer);

        var result = quantity.To(TestLengthUnit<double>.Meter);

        Assert.IsType<Quantity<double, TestLengthUnit<double>>>(result);
        Assert.Equal(1000, result.Value, 12);
        Assert.Same(TestLengthUnit<double>.Meter, result.Unit);
    }

    [Fact]
    public void Addition_LinearQuantitiesWithDifferentScales_ReturnsLeftUnitValue()
    {
        var left = new Quantity<double>(1, TestLengthUnit<double>.Kilometer);
        var right = new Quantity<double>(500, TestLengthUnit<double>.Meter);

        var result = left + right;

        Assert.Equal(TestLengthUnit<double>.LengthDimension, result.Unit.Dimension);
        Assert.Equal(1.5, result.Value, 12);
        Assert.Equal(1500, result.BaseValue, 12);
    }

    [Fact]
    public void Subtraction_LinearQuantitiesWithDifferentScales_ReturnsLeftUnitValue()
    {
        var left = new Quantity<double>(1, TestLengthUnit<double>.Kilometer);
        var right = new Quantity<double>(250, TestLengthUnit<double>.Meter);

        var result = left - right;

        Assert.Equal(0.75, result.Value, 12);
        Assert.Equal(750, result.BaseValue, 12);
    }

    [Fact]
    public void Multiplication_LengthByLength_ReturnsSquaredDimension()
    {
        var left = new Quantity<double>(2, TestLengthUnit<double>.Meter);
        var right = new Quantity<double>(3, TestLengthUnit<double>.Kilometer);

        var result = left * right;

        Assert.Equal(new Dimension(L: 2), result.Unit.Dimension);
        Assert.Equal(6, result.Value, 12);
        Assert.Equal(6000, result.BaseValue, 12);
    }

    [Fact]
    public void Division_LengthByLength_ReturnsDimensionlessQuantity()
    {
        var left = new Quantity<double>(500, TestLengthUnit<double>.Meter);
        var right = new Quantity<double>(2, TestLengthUnit<double>.Kilometer);

        var result = left / right;

        Assert.Equal(Dimension.Dimensionless, result.Unit.Dimension);
        Assert.Equal(250, result.Value, 12);
        Assert.Equal(0.25, result.BaseValue, 12);
    }

    [Fact]
    public void Division_ByZeroBaseValue_ThrowsDivideByZeroException()
    {
        var left = new Quantity<double>(1, TestLengthUnit<double>.Meter);
        var right = new Quantity<double>(0, TestLengthUnit<double>.Meter);

        Assert.Throws<DivideByZeroException>(() => left / right);
    }

    [Fact]
    public void Addition_DifferentDimensions_ThrowsInvalidOperationException()
    {
        var length = new Quantity<double>(1, TestLengthUnit<double>.Meter);
        var time = new Quantity<double>(1, TestTimeUnit<double>.Second);

        Assert.Throws<InvalidOperationException>(() => length + time);
    }

    [Fact]
    public void Addition_AffineQuantities_ThrowsInvalidOperationException()
    {
        var left = new Quantity<double>(20, TestTemperatureUnit<double>.Celsius);
        var right = new Quantity<double>(10, TestTemperatureUnit<double>.Celsius);

        Assert.Throws<InvalidOperationException>(() => left + right);
    }

    [Fact]
    public void From_DifferentDimensionUnit_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => TestLengthUnit<double>.From(TestTimeUnit<double>.Second));
    }

    [Fact]
    public void AffineSubtraction_UntypedQuantity_ReturnsLinearDifferenceUnit()
    {
        var left = new Quantity<double>(20, TestTemperatureUnit<double>.Celsius);
        var right = new Quantity<double>(10, TestTemperatureUnit<double>.Celsius);

        var result = left - right;

        Assert.Equal(TestTemperatureUnit<double>.TemperatureDimension, result.Unit.Dimension);
        Assert.Equal(UnitFamily.Linear, result.Unit.Family);
        Assert.Equal(10, result.Value, 12);
        Assert.Equal(10, result.BaseValue, 12);
    }
}
