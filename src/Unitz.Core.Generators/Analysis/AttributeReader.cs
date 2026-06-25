namespace Unitz.Core.Generators.Analysis;

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

internal static class AttributeReader
{
    public static IEnumerable<int> GetPrefixArguments(AttributeData attribute)
    {
        if (attribute.ConstructorArguments.Length == 0)
        {
            yield break;
        }

        var argument = attribute.ConstructorArguments[0];
        if (argument.Kind == TypedConstantKind.Array)
        {
            foreach (var value in argument.Values)
            {
                if (value.Value is int prefix)
                {
                    yield return prefix;
                }
            }
        }
        else if (argument.Value is int prefix)
        {
            yield return prefix;
        }
    }

    public static string GetConstructorArgument(AttributeData attribute, int index, string fallback)
    {
        return attribute.ConstructorArguments.Length <= index
            ? fallback
            : attribute.ConstructorArguments[index].Value as string ?? fallback;
    }

    public static T GetNamedArgument<T>(AttributeData attribute, string name, T fallback)
    {
        foreach (var argument in attribute.NamedArguments)
        {
            if (argument.Key == name && argument.Value.Value is T value)
            {
                return value;
            }
        }

        return fallback;
    }
}
