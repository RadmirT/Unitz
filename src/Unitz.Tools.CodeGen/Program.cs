using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Unitz.Core.Generators;

var projectDirectory = args.Length > 0
    ? Path.GetFullPath(args[0])
    : Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Unitz"));

var specsDirectory = Path.Combine(projectDirectory, "Specs");
var generatedDirectory = Path.Combine(projectDirectory, "Generated");

if (!Directory.Exists(specsDirectory))
{
    Console.Error.WriteLine($"Specs directory does not exist: {specsDirectory}");
    return 1;
}

Directory.CreateDirectory(generatedDirectory);

var syntaxTrees = Directory
    .EnumerateFiles(specsDirectory, "*.cs", SearchOption.AllDirectories)
    .OrderBy(static file => file, StringComparer.OrdinalIgnoreCase)
    .Select(static file => CSharpSyntaxTree.ParseText(File.ReadAllText(file), path: file))
    .ToArray();

var references = GetReferencePaths()
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .Select(static path => MetadataReference.CreateFromFile(path))
    .ToArray();

var compilation = CSharpCompilation.Create(
    "Unitz.CodeGeneration",
    syntaxTrees,
    references,
    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

var candidates = syntaxTrees
    .SelectMany(tree =>
    {
        var semanticModel = compilation.GetSemanticModel(tree);
        return tree.GetRoot()
            .DescendantNodes()
            .OfType<Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax>()
            .Where(static declaration => declaration.AttributeLists.Count > 0)
            .Select(declaration => semanticModel.GetDeclaredSymbol(declaration) as INamedTypeSymbol)
            .OfType<INamedTypeSymbol>();
    })
    .ToArray();

var generatedSources = QuantityGenerator.GenerateSources(compilation, candidates);
var generatedFileNames = new HashSet<string>(
    generatedSources.Select(static source => source.FileName),
    StringComparer.OrdinalIgnoreCase);

foreach (var existingFile in Directory.EnumerateFiles(generatedDirectory, "*.g.cs", SearchOption.TopDirectoryOnly))
{
    if (!generatedFileNames.Contains(Path.GetFileName(existingFile)))
    {
        File.Delete(existingFile);
    }
}

foreach (var source in generatedSources)
{
    var targetPath = Path.Combine(generatedDirectory, source.FileName);
    File.WriteAllText(targetPath, source.Source);
    Console.WriteLine($"Generated {Path.GetRelativePath(projectDirectory, targetPath)}");
}

Console.WriteLine($"Generated {generatedSources.Count} file(s).");
return 0;

static IEnumerable<string> GetReferencePaths()
{
    var trustedPlatformAssemblies = ((string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))
        ?.Split(Path.PathSeparator)
        ?? Array.Empty<string>();

    foreach (var assemblyPath in trustedPlatformAssemblies)
    {
        yield return assemblyPath;
    }

    yield return typeof(Unitz.Core.Dimension).Assembly.Location;
    yield return typeof(Attribute).Assembly.Location;
    yield return typeof(Enumerable).Assembly.Location;
    yield return typeof(Assembly).Assembly.Location;
}
