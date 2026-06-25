using Unitz;

PrintHeader("Length addition and base values");
{
    var distance = new LengthQuantity<double>(1, LengthUnit<double>.Kilometer);
    var extra = new LengthQuantity<double>(500, LengthUnit<double>.Meter);

    var total = distance + extra;

    Print("Total distance", $"{total.Value} km");
    Print("Total base value", $"{total.BaseValue} m");
}

PrintHeader("Unit conversion");
{
    var length = new LengthQuantity<double>(1000, LengthUnit<double>.Millimeter);
    var meters = length.To(LengthUnit<double>.Meter);

    Print("Length", $"{meters.Value} m");
}

PrintHeader("Typed derived operation: area");
{
    var width = new LengthQuantity<double>(2, LengthUnit<double>.Meter);
    var height = new LengthQuantity<double>(3, LengthUnit<double>.Meter);

    AreaQuantity<double> area = width * height;

    Print("Area", $"{area.Value} m^2");
    Print("Uses square meter", area.Unit == AreaUnit<double>.SquareMeter);
}

PrintHeader("Typed derived operation: speed");
{
    var distance = new LengthQuantity<double>(10, LengthUnit<double>.Kilometer);
    var duration = new TimeQuantity<double>(30, TimeUnit<double>.Minute);

    SpeedQuantity<double> speed = distance / duration;
    var kilometersPerHour = speed.To(SpeedUnit<double>.KilometerPerHour);

    Print("Speed", $"{kilometersPerHour.Value} km/h");
}

PrintHeader("Mass and density");
{
    var mass = new MassQuantity<double>(2, MassUnit<double>.Kilogram);
    var volume = new VolumeQuantity<double>(1, VolumeUnit<double>.Liter);

    DensityQuantity<double> density = mass / volume;
    var gramsPerCubicCentimeter = density.To(DensityUnit<double>.GramPerCubicCentimeter);

    Print("Density", $"{gramsPerCubicCentimeter.Value} g/cm^3");
}

PrintHeader("Mechanical chain: force, energy, power");
{
    var mass = new MassQuantity<double>(2, MassUnit<double>.Kilogram);
    var acceleration = new AccelerationQuantity<double>(3, AccelerationUnit<double>.MeterPerSecondSquared);
    var distance = new LengthQuantity<double>(5, LengthUnit<double>.Meter);
    var duration = new TimeQuantity<double>(4, TimeUnit<double>.Second);

    var force = mass * acceleration;
    var energy = force * distance;
    var power = energy / duration;

    Print("Force", $"{force.Value} N");
    Print("Energy", $"{energy.Value} J");
    Print("Power", $"{power.Value} W");
}

static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(title);
    Console.WriteLine(new string('-', title.Length));
}

static void Print(string label, object value)
{
    Console.WriteLine($"{label}: {value}");
}
