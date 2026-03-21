using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Unitz.Core.Generators.Analysis;
using Unitz.Core.Generators.Emitting;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators;

public sealed class GeneratedQuantitySource
{
    public GeneratedQuantitySource(string fileName, string source)
    {
        FileName = fileName;
        Source = source;
    }

    public string FileName { get; }

    public string Source { get; }
}

[Generator]
public sealed class QuantityGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var candidateTypes = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
                static (ctx, ct) => (INamedTypeSymbol?)ctx.SemanticModel.GetDeclaredSymbol((ClassDeclarationSyntax)ctx.Node, ct))
            .Where(static symbol => symbol is not null)
            .Select(static (symbol, _) => symbol!);

        var compilationAndCandidates = context.CompilationProvider.Combine(candidateTypes.Collect());
        context.RegisterSourceOutput(compilationAndCandidates, static (spc, source) =>
        {
            var generatedSources = GenerateSources(
                source.Left,
                source.Right,
                spc.CancellationToken,
                diagnostic => spc.ReportDiagnostic(diagnostic));
            foreach (var generatedSource in generatedSources)
            {
                spc.AddSource(generatedSource.FileName, SourceText.From(generatedSource.Source, Encoding.UTF8));
            }
        });
    }

    public static IReadOnlyList<GeneratedQuantitySource> GenerateSources(
        Compilation compilation,
        IEnumerable<INamedTypeSymbol> candidates,
        CancellationToken cancellationToken = default,
        Action<Diagnostic>? reportDiagnostic = null)
    {
        _ = reportDiagnostic;
        var models = new List<QuantityModel>();
        var seen = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        foreach (var typeSymbol in candidates)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!seen.Add(typeSymbol))
            {
                continue;
            }

            var quantityModels = QuantityModelBuilder.Create(typeSymbol);
            if (quantityModels.Count != 0)
            {
                models.AddRange(quantityModels);
            }
        }

        var resolvedModels = QuantityOperationResolver.AddReverseMultiplyOperations(models);
        var sources = new List<GeneratedQuantitySource>();
        foreach (var model in resolvedModels)
        {
            var emitters = SourceEmitterProvider.GetEmitters(model);
            foreach (var emitter in emitters)
            {
                var fileName = BuildHintName(model, emitter.GetTypeName(model), emitter.GetType().Name);
                sources.Add(new GeneratedQuantitySource(fileName, emitter.Generate(model)));
            }
        }

        return sources;
    }

    private static string BuildHintName(QuantityModel model, string typeName, string emitterName)
    {
        var ns = string.IsNullOrEmpty(model.Namespace) ? "Global" : model.Namespace.Replace('.', '_');
        return $"{ns}_{typeName}_{emitterName}.g.cs";
    }
}
