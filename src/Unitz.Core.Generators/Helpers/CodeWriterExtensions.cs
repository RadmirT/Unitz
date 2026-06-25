using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Helpers;

internal static class CodeWriterExtensions
{
    public static void WriteQuantityConstructors(this CodeWriter codeWriter, string name, string valueType, string unit)
    {
        codeWriter.WriteLine($"public {name}({valueType} value) : this(value, {unit}.Base)");
        codeWriter.WriteEmptyBlock();
        codeWriter.WriteLine();
        codeWriter.WriteLine($"public {name}({valueType} value, {unit} unit) : base(value, unit)");
        codeWriter.WriteEmptyBlock();
    }
    public static void WriteCreateMethod(this CodeWriter codeWriter, QuantityModel model)
    {
        codeWriter.WriteLine($"protected override {model.QuantityReference} Create({model.ValueTypeReference} value, {model.UnitReference} unit)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine($"return new {model.QuantityReference}(value, unit);");
        }
    }
    
    public static void GenerateLinearQuantityOperators(this CodeWriter codeWriter, string current, string valueType, string unit)
    {
        var baseType = $"global::Unitz.Core.LinearQuantity<{current}, {valueType} , {unit}>";
        codeWriter.WriteLine();
        codeWriter.WriteLine($"public static {current} operator +({current} left, {current} right)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine($"return ({baseType})left + right;");
        }

        codeWriter.WriteLine();
        codeWriter.WriteLine($"public static {current} operator -({current} left, {current} right)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine($"return ({baseType})left - right;");
        }

        codeWriter.WriteLine();
        codeWriter.WriteLine($"public static {current} operator *({current} left, {valueType} factor)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine($"return ({baseType})left * factor;");
        }
        codeWriter.WriteLine();
        codeWriter.WriteLine($"public static {current} operator /({current} left, {valueType} divisor)");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine($"return ({baseType})left / divisor;");
        }
    }
}