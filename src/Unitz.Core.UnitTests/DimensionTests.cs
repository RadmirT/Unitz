namespace Unitz.Core.UnitTests;

using Unitz.Core;
using Xunit;

public class DimensionTests
{
    [Fact]
    public void Dimensionless_HasZeroExponents()
    {
        var dimension = Dimension.Dimensionless;

        Assert.Equal(new Dimension(), dimension);
        Assert.True(dimension.IsDimensionless);
    }

    [Fact]
    public void IsDimensionless_WithNonZeroExponent_ReturnsFalse()
    {
        var dimension = new Dimension(L: 1);

        Assert.False(dimension.IsDimensionless);
    }

    [Fact]
    public void Addition_AddsAllExponents()
    {
        var left = new Dimension(L: 1, M: 2, T: -3, I: 4, Th: -5, N: 6, J: -7);
        var right = new Dimension(L: -2, M: 3, T: 4, I: -5, Th: 6, N: -7, J: 8);

        var result = left + right;

        Assert.Equal(new Dimension(L: -1, M: 5, T: 1, I: -1, Th: 1, N: -1, J: 1), result);
    }

    [Fact]
    public void Subtraction_SubtractsAllExponents()
    {
        var left = new Dimension(L: 1, M: 2, T: -3, I: 4, Th: -5, N: 6, J: -7);
        var right = new Dimension(L: -2, M: 3, T: 4, I: -5, Th: 6, N: -7, J: 8);

        var result = left - right;

        Assert.Equal(new Dimension(L: 3, M: -1, T: -7, I: 9, Th: -11, N: 13, J: -15), result);
    }

    [Fact]
    public void Addition_WithOppositeDimension_ReturnsDimensionless()
    {
        var dimension = new Dimension(L: 1, T: -2, Th: 1);

        var result = dimension + new Dimension(L: -1, T: 2, Th: -1);

        Assert.Equal(Dimension.Dimensionless, result);
        Assert.True(result.IsDimensionless);
    }

    [Fact]
    public void Subtraction_SameDimension_ReturnsDimensionless()
    {
        var dimension = new Dimension(L: 1, M: -1, T: -2);

        var result = dimension - dimension;

        Assert.Equal(Dimension.Dimensionless, result);
        Assert.True(result.IsDimensionless);
    }
}
