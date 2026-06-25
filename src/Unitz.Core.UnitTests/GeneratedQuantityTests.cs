using Rationalz;

namespace Unitz.Core.UnitTests;

using System.Numerics;
using Unitz.Core;
using Unitz;
using Xunit;

public class GeneratedQuantityTests
{
    [Fact]
    public void LinearQuantityAttribute_GeneratesQuantityAndUnits()
    {
        var length = new LengthQuantity<double>(1000, LengthUnit<double>.Millimeter);

        Assert.Equal(1, length.BaseValue, 12);
        Assert.Equal(new Dimension(L: 1), LengthUnit<Rational<int>>.Dimension);
        Assert.Equal(0.0254, LengthUnit<double>.Inch.Scale, 12);
        Assert.Equal(0.9144, LengthUnit<double>.Yard.Scale, 12);
    }

    [Fact]
    public void SiLinearUnitsAttribute_ForSquaredBaseUnit_UsesSquaredPrefixScale()
    {
        Assert.Equal(0.000001, AreaUnit<double>.SquareMillimeter.Scale, 12);
        Assert.Equal(0.0001, AreaUnit<double>.SquareCentimeter.Scale, 12);
        Assert.Equal(1_000_000, AreaUnit<double>.SquareKilometer.Scale, 12);
    }

    [Fact]
    public void LinearUnitsAttribute_GeneratesCustomUnits()
    {
        var hectare = new AreaQuantity<double>(1, AreaUnit<double>.Hectare);

        Assert.Equal(10_000, hectare.BaseValue, 12);
        Assert.Equal(4046.856422, AreaUnit<double>.Acre.Scale, 12);
    }

    [Fact]
    public void MultiplyOperationAttribute_GeneratesTypedOperator()
    {
        var left = new LengthQuantity<double>(2, LengthUnit<double>.Meter);
        var right = new LengthQuantity<double>(3, LengthUnit<double>.Meter);

        var result = left * right;

        Assert.IsType<AreaQuantity<double>>(result);
        Assert.Equal(6, result.Value, 12);
        Assert.Equal(AreaUnit<double>.SquareMeter, result.Unit);
    }

    [Fact]
    public void DivideOperationAttribute_GeneratesTypedOperator()
    {
        var length = new LengthQuantity<double>(10, LengthUnit<double>.Meter);
        var time = new TimeQuantity<double>(2, TimeUnit<double>.Second);

        var result = length / time;

        Assert.IsType<SpeedQuantity<double>>(result);
        Assert.Equal(5, result.Value, 12);
        Assert.Equal(SpeedUnit<double>.MeterPerSecond, result.Unit);
    }

    [Fact]
    public void LinearQuantityAttribute_GeneratesGenericMathInterfaces()
    {
        var left = new LengthQuantity<double>(2, LengthUnit<double>.Meter);
        var right = new LengthQuantity<double>(50, LengthUnit<double>.Centimeter);

        var sum = Add<LengthQuantity<double>>(left, right);
        var difference = Subtract<LengthQuantity<double>>(left, right);
        var product = MultiplyByScalar<LengthQuantity<double>>(left, 3);
        var quotient = DivideByScalar<LengthQuantity<double>>(left, 2);

        Assert.Equal(2.5, sum.Value, 12);
        Assert.Equal(1.5, difference.Value, 12);
        Assert.Equal(6, product.Value, 12);
        Assert.Equal(1, quotient.Value, 12);
    }

    [Fact]
    public void OperationAttributes_GenerateGenericMathInterfacesForQuantitiesAndUnits()
    {
        var area = Multiply<LengthQuantity<double>, LengthQuantity<double>, AreaQuantity<double>>(
            new LengthQuantity<double>(2, LengthUnit<double>.Meter),
            new LengthQuantity<double>(3, LengthUnit<double>.Meter));
        var speed = Divide<LengthQuantity<double>, TimeQuantity<double>, SpeedQuantity<double>>(
            new LengthQuantity<double>(10, LengthUnit<double>.Meter),
            new TimeQuantity<double>(2, TimeUnit<double>.Second));
        var areaUnit = Multiply<LengthUnit<double>, LengthUnit<double>, AreaUnit<double>>(LengthUnit<double>.Meter, LengthUnit<double>.Meter);
        var speedUnit = Divide<LengthUnit<double>, TimeUnit<double>, SpeedUnit<double>>(LengthUnit<double>.Meter, TimeUnit<double>.Second);

        Assert.Equal(6, area.Value, 12);
        Assert.Equal(5, speed.Value, 12);
        Assert.Equal(AreaUnit<double>.SquareMeter, areaUnit);
        Assert.Equal(SpeedUnit<double>.MeterPerSecond, speedUnit);
    }

    [Fact]
    public void CommonUnits_GenerateExpectedScales()
    {
        Assert.Equal(0.45359237, MassUnit<double>.Pound.Scale, 12);
        Assert.Equal(0.003785411784, VolumeUnit<double>.GallonUs.Scale, 12);
        Assert.Equal(101325, PressureUnit<double>.Atmosphere.Scale, 12);
        Assert.Equal(3_600_000, EnergyUnit<double>.KilowattHour.Scale, 12);
        Assert.Equal(745.6998715822702, PowerUnit<double>.Horsepower.Scale, 12);
        Assert.Equal(0.0001, MagneticFluxDensityUnit<double>.Gauss.Scale, 12);
        Assert.Equal(0.017453292519943295, PlaneAngleUnit<double>.Degree.Scale, 12);
        Assert.Equal(0.01, DimensionlessUnit<double>.Percent.Scale, 12);
        Assert.Equal(1024, InformationUnit<double>.Kibibyte.Scale, 12);
    }

    [Fact]
    public void CommonUnits_GenerateDerivedOperations()
    {
        var force = Multiply<MassQuantity<double>, AccelerationQuantity<double>, ForceQuantity<double>>(
            new MassQuantity<double>(2, MassUnit<double>.Kilogram),
            new AccelerationQuantity<double>(3, AccelerationUnit<double>.MeterPerSecondSquared));
        var pressure = Divide<ForceQuantity<double>, AreaQuantity<double>, PressureQuantity<double>>(
            new ForceQuantity<double>(10, ForceUnit<double>.Newton),
            new AreaQuantity<double>(2, AreaUnit<double>.SquareMeter));
        var power = Divide<EnergyQuantity<double>, TimeQuantity<double>, PowerQuantity<double>>(
            new EnergyQuantity<double>(100, EnergyUnit<double>.Joule),
            new TimeQuantity<double>(4, TimeUnit<double>.Second));
        var capacitance = Divide<ElectricChargeQuantity<double>, VoltageQuantity<double>, CapacitanceQuantity<double>>(
            new ElectricChargeQuantity<double>(10, ElectricChargeUnit<double>.Coulomb),
            new VoltageQuantity<double>(2, VoltageUnit<double>.Volt));
        Assert.Equal(6, force.Value, 12);
        Assert.Equal(5, pressure.Value, 12);
        Assert.Equal(25, power.Value, 12);
        Assert.Equal(5, capacitance.Value, 12);
    }

    [Fact]
    public void DerivedQuantity_CanBeBuiltFromBaseQuantities()
    {
        var length = new LengthQuantity<double>(2, LengthUnit<double>.Meter);
        var mass = new MassQuantity<double>(3, MassUnit<double>.Kilogram);
        var time = new TimeQuantity<double>(4, TimeUnit<double>.Second);
        var current = new ElectricCurrentQuantity<double>(5, ElectricCurrentUnit<double>.Ampere);

        var area = length * length;
        var acceleration = (length / time) / time;
        var force = mass * acceleration;
        var energy = force * length;
        var power = energy / time;
        var voltage = power / current;
        var magneticFlux = voltage * time;
        var magneticFluxDensity = magneticFlux / area;

        Assert.IsType<MagneticFluxDensityQuantity<double>>(magneticFluxDensity);
        Assert.Equal(new Dimension(M: 1, T: -2, I: -1), ((Unit<double>)magneticFluxDensity.Unit).Dimension);
        Assert.Equal(0.0375, magneticFluxDensity.Value, 12);
        Assert.Equal(MagneticFluxDensityUnit<double>.Tesla, magneticFluxDensity.Unit);
    }

    private static TSelf Add<TSelf>(TSelf left, TSelf right)
        where TSelf : IAdditionOperators<TSelf, TSelf, TSelf>
    {
        return left + right;
    }

    private static TResult Add<TLeft, TRight, TResult>(TLeft left, TRight right)
        where TLeft : IAdditionOperators<TLeft, TRight, TResult>
    {
        return left + right;
    }

    private static TSelf Subtract<TSelf>(TSelf left, TSelf right)
        where TSelf : ISubtractionOperators<TSelf, TSelf, TSelf>
    {
        return left - right;
    }

    private static TResult Subtract<TLeft, TRight, TResult>(TLeft left, TRight right)
        where TLeft : ISubtractionOperators<TLeft, TRight, TResult>
    {
        return left - right;
    }

    private static TSelf MultiplyByScalar<TSelf>(TSelf left, double right)
        where TSelf : IMultiplyOperators<TSelf, double, TSelf>
    {
        return left * right;
    }

    private static TSelf DivideByScalar<TSelf>(TSelf left, double right)
        where TSelf : IDivisionOperators<TSelf, double, TSelf>
    {
        return left / right;
    }

    private static TResult Multiply<TLeft, TRight, TResult>(TLeft left, TRight right)
        where TLeft : IMultiplyOperators<TLeft, TRight, TResult>
    {
        return left * right;
    }

    private static TResult Divide<TLeft, TRight, TResult>(TLeft left, TRight right)
        where TLeft : IDivisionOperators<TLeft, TRight, TResult>
    {
        return left / right;
    }
}
