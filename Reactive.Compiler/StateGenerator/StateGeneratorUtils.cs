using System.Linq;
using Microsoft.CodeAnalysis;

namespace Reactive.Compiler;

internal static class StateGeneratorUtils {
    public const string StateType = "IState";
    public const string StateNamespace = "Reactive";
    
    public const string StatePath = $"{StateNamespace}.{StateType}";
    public const string AttributePath = "Reactive.Compiler.StateGenAttribute";

    public static ITypeSymbol? GetStateTargetType(ITypeSymbol symbol) {
        var state = symbol.AllInterfaces.FirstOrDefault(x =>
            x.ContainingNamespace?.ToString() == StateNamespace &&
            x.Name == StateType
        );

        return state?.TypeArguments.First();
    }
}