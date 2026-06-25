using Unitz.Core.Generators.Helpers;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting.Unit;

internal abstract class UnitSourceEmitterBase : BaseSourceEmitter
{
    protected static void WriteDimension(CodeWriter codeWriter, DimensionModel dimension)
    {
        codeWriter.Write("public new static readonly global::Unitz.Core.Dimension Dimension = new(");
        codeWriter.Write($"L:{dimension.L},");
        codeWriter.Write($"M:{dimension.M},");
        codeWriter.Write($"T:{dimension.T},");
        codeWriter.Write($"I:{dimension.I},");
        codeWriter.Write($"Th:{dimension.Th},");
        codeWriter.Write($"N:{dimension.N},");
        codeWriter.Write($"J:{dimension.J});");
        codeWriter.WriteLine();
    }

    protected static void WritePow10(CodeWriter codeWriter, string valueType)
    {
        codeWriter.WriteLine();
        codeWriter.WriteLine($"private static {valueType} Pow10(int exponent)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine($"var result = {NumberLiteralProvider.One(valueType)};");
            codeWriter.WriteLine($"var ten = {NumberLiteralProvider.CreateChecked(valueType, "10")};");
            codeWriter.WriteLine("for (var i = 0; i < global::System.Math.Abs(exponent); i++)");
            using (codeWriter.Block())
            {
                codeWriter.WriteLine("result *= ten;");
            }

            codeWriter.WriteLine();
            codeWriter.WriteLine($"return exponent < 0 ? {NumberLiteralProvider.One(valueType)} / result : result;");
        }
    }

    protected static void WriteLinearUnitFrom(CodeWriter codeWriter, string unitName, string unitReference, string valueType)
    {
        codeWriter.WriteLine();
        codeWriter.WriteLine($"public static {unitReference} From(global::Unitz.Core.Unit<{valueType}> unit)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine("global::System.ArgumentNullException.ThrowIfNull(unit);");
            codeWriter.WriteLine();
            codeWriter.WriteLine("if (Dimension != unit.Dimension)");
            using (codeWriter.Block())
            {
                codeWriter.WriteLine("throw new global::System.ArgumentException(\"Unit dimension does not match target unit type.\", nameof(unit));");
            }

            codeWriter.WriteLine();
            codeWriter.WriteLine($"if (unit is {unitReference} typedUnit)");
            using (codeWriter.Block())
            {
                codeWriter.WriteLine("return typedUnit;");
            }

            codeWriter.WriteLine();
            codeWriter.WriteLine($"var convertedUnit = global::Unitz.Core.UnitCast.To<global::Unitz.Core.LinearUnit<{valueType}>, {valueType}>(unit);");
            codeWriter.WriteLine("return new(convertedUnit.Scale);");
        }
    }

    protected static void WriteAffineUnitFrom(CodeWriter codeWriter, string unitName, string unitReference, string valueType)
    {
        codeWriter.WriteLine();
        codeWriter.WriteLine($"public static {unitReference} From(global::Unitz.Core.Unit<{valueType}> unit)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine("global::System.ArgumentNullException.ThrowIfNull(unit);");
            codeWriter.WriteLine();
            codeWriter.WriteLine("if (Dimension != unit.Dimension)");
            using (codeWriter.Block())
            {
                codeWriter.WriteLine("throw new global::System.ArgumentException(\"Unit dimension does not match target unit type.\", nameof(unit));");
            }

            codeWriter.WriteLine();
            codeWriter.WriteLine($"if (unit is {unitReference} typedUnit)");
            using (codeWriter.Block())
            {
                codeWriter.WriteLine("return typedUnit;");
            }

            codeWriter.WriteLine();
            codeWriter.WriteLine($"var convertedUnit = global::Unitz.Core.UnitCast.To<global::Unitz.Core.AffineUnit<{valueType}>, {valueType}>(unit);");
            codeWriter.WriteLine("return new(convertedUnit.Scale, convertedUnit.Offset);");
        }
    }
}
