namespace Unitz.Core.UnitTests;

using Unitz.Core;
using Xunit;

public class UnitTests
{
    [Fact]
    public void LinearUnit_WithZeroScale_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new LinearUnit<double>(TestLengthUnit<double>.LengthDimension, 0));
    }

    [Fact]
    public void UnitMultiplication_LinearUnits_CombinesDimensionsAndScales()
    {
        var result = TestLengthUnit<double>.Kilometer * TestLengthUnit<double>.Centimeter;

        Assert.Equal(new Dimension(L: 2), result.Dimension);
        Assert.Equal(10, ((LinearUnit<double>)result).Scale, 12);
    }

    [Fact]
    public void UnitDivision_LinearUnits_SubtractsDimensionsAndDividesScales()
    {
        var result = TestLengthUnit<double>.Kilometer / TestTimeUnit<double>.Second;

        Assert.Equal(new Dimension(L: 1, T: -1), result.Dimension);
        Assert.Equal(1000, ((LinearUnit<double>)result).Scale, 12);
    }

    [Fact]
    public void ConvertValueTo_SameDimension_ConvertsThroughBaseValue()
    {
        var result = TestLengthUnit<double>.Kilometer.ConvertValueTo(1, TestLengthUnit<double>.Meter);

        Assert.Equal(1000, result, 12);
    }
}
