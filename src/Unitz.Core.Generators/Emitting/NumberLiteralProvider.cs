namespace Unitz.Core.Generators.Emitting;

using System.Globalization;

internal static class NumberLiteralProvider
{
    public static string One(string valueType)
    {
        return IsDouble(valueType) ? "1d" : valueType + ".One";
    }

    public static string Zero(string valueType)
    {
        return IsDouble(valueType) ? "0d" : valueType + ".Zero";
    }

    public static string CreateChecked(string valueType, string value)
    {
        return IsDouble(valueType) ? value + "d" : valueType + ".CreateChecked(" + value + ")";
    }

    public static string ToGenericNumber(double value, string valueType)
    {
        if (value == 1)
        {
            return One(valueType);
        }

        if (value == 0)
        {
            return Zero(valueType);
        }

        return CreateChecked(valueType, value.ToString("R", CultureInfo.InvariantCulture));
    }

    public static bool IsDouble(string valueType)
    {
        return valueType == "double" || valueType == "global::System.Double" || valueType == "System.Double";
    }
}
