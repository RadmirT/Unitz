namespace Unitz.Core.Generators.Models;

using System.Globalization;
using Microsoft.CodeAnalysis;

internal sealed class DimensionModel
{
    public int L { get; private set; }

    public int M { get; private set; }

    public int T { get; private set; }

    public int I { get; private set; }

    public int Th { get; private set; }

    public int N { get; private set; }

    public int J { get; private set; }

    public static DimensionModel From(AttributeData attribute)
    {
        var dimension = new DimensionModel();
        foreach (var argument in attribute.NamedArguments)
        {
            if (argument.Value.Value is not int value)
            {
                continue;
            }

            switch (argument.Key)
            {
                case "L":
                    dimension.L = value;
                    break;
                case "M":
                    dimension.M = value;
                    break;
                case "T":
                    dimension.T = value;
                    break;
                case "I":
                    dimension.I = value;
                    break;
                case "Th":
                    dimension.Th = value;
                    break;
                case "N":
                    dimension.N = value;
                    break;
                case "J":
                    dimension.J = value;
                    break;
            }
        }

        return dimension;
    }

    public string ToMessage()
    {
        return $"Dimension(L: {this.L.ToString(CultureInfo.InvariantCulture)}, M: {this.M.ToString(CultureInfo.InvariantCulture)}, T: {this.T.ToString(CultureInfo.InvariantCulture)}, I: {this.I.ToString(CultureInfo.InvariantCulture)}, Th: {this.Th.ToString(CultureInfo.InvariantCulture)}, N: {this.N.ToString(CultureInfo.InvariantCulture)}, J: {this.J.ToString(CultureInfo.InvariantCulture)})";
    }
}
