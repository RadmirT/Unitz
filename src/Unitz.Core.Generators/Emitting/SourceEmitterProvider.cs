using Unitz.Core.Generators.Emitting.Quantities;
using Unitz.Core.Generators.Emitting.Unit;
using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting;

public static class SourceEmitterProvider
{
    private static readonly Dictionary<QuantityKind, ISourceEmitter[]> Emitters = new()
    {
        { QuantityKind.Linear, [new LinearQuantitySourceEmitter(), new LinearUnitSourceEmitter()] },
        { QuantityKind.Affine, [new AffineQuantitySourceEmitter(), new AffineUnitSourceEmitter()] },
    };
    
    internal static ISourceEmitter[] GetEmitters(QuantityModel model) => Emitters[model.Kind];
}
