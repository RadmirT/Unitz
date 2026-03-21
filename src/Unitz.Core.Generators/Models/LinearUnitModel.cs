namespace Unitz.Core.Generators.Models;

internal sealed class LinearUnitModel
{
    public LinearUnitModel(string name, double scale)
    {
        this.Name = name;
        this.Scale = scale;
    }

    public string Name { get; }

    public double Scale { get; }
}
