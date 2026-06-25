namespace Unitz.Core.Generators.Models;

internal sealed class AffineUnitModel
{
    public AffineUnitModel(string name, double scale, double offset)
    {
        this.Name = name;
        this.Scale = scale;
        this.Offset = offset;
    }

    public string Name { get; }

    public double Scale { get; }

    public double Offset { get; }
}
