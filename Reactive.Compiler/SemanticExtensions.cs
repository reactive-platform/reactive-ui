using System.Linq;
using Microsoft.CodeAnalysis;

namespace Reactive.Compiler;

internal static class SemanticExtensions {
    public static TypedConstant? GetNamedArgument(this AttributeData data, string argument) {
        var arg = data.NamedArguments.FirstOrDefault(x => x.Key == argument);
        return arg.Key == null ? null : arg.Value;
    }

    #region GetAttribute

    public static AttributeData? GetAttribute(this ISymbol symbol, INamedTypeSymbol attribute) {
        return symbol
            .GetAttributes()
            .FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attribute));
    }

    public static AttributeData? GetAttribute(this ISymbol symbol, SemanticModel semanticModel, string name) {
        var compilation = semanticModel.Compilation;
        var attribute = compilation.GetTypeByMetadataName(name);

        return GetAttribute(symbol, attribute!);
    }

    #endregion

    #region GetDerivedAttribute

    public static AttributeData? GetDerivedAttribute(this IMethodSymbol symbol, SemanticModel semanticModel, string name) {
        var compilation = semanticModel.Compilation;
        var attribute = compilation.GetTypeByMetadataName(name);

        return GetDerivedAttribute(symbol, attribute!);
    }

    public static AttributeData? GetDerivedAttribute(this IMethodSymbol symbol, INamedTypeSymbol attribute) {
        IMethodSymbol? currentMethod = symbol;

        do {
            var attrs = currentMethod.GetAttributes();
            var attr = attrs.FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attribute));

            if (attr != null) {
                return attr;
            }

            currentMethod = currentMethod.OverriddenMethod;
        } while (currentMethod != null);

        return null;
    }

    #endregion
}