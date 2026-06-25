using Unitz.Core.Generators.Models;

namespace Unitz.Core.Generators.Emitting;

internal interface ISourceEmitter
{
    string Generate(QuantityModel model);
    
    string GetTypeName(QuantityModel model);
}