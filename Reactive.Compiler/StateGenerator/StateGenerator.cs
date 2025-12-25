using System.Linq;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Reactive.Compiler;

[Generator]
internal class StateGenerator : IIncrementalGenerator {
    public void Initialize(IncrementalGeneratorInitializationContext context) {
        // Filter assignment expressions and get semantic model
        var candidates = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is AssignmentExpressionSyntax { RawKind: (int)SyntaxKind.SimpleAssignmentExpression },
                transform: static (ctx, ct) => {
                    var assignment = (AssignmentExpressionSyntax)ctx.Node;
                    var semanticModel = ctx.SemanticModel;
                    return GetCandidate(assignment, semanticModel, ct);
                }
            )
            .Where(static candidate => candidate.HasValue)
            .Select(static (candidate, _) => candidate!.Value);

        // Group by containing type
        var groupedByType = candidates
            .Collect()
            .SelectMany((candidates, _) =>
                candidates
                    .Distinct()
                    .GroupBy(
                        x => x.targetProp,
                        x => x.genName,
                        SymbolEqualityComparer.Default
                    )
                    .GroupBy(
                        x => x.Key.ContainingType,
                        SymbolEqualityComparer.Default
                    )
            );

        // Generate source code
        context.RegisterSourceOutput(
            groupedByType,
            static (spc, typeGroup) => {
                var type = typeGroup.Key;
                if (type == null) return;

                var ext = GenerateTypeExtension(type, typeGroup);
                var file = $"Reactive_{type.Name}StateExt.g.cs";
                spc.AddSource(file, ext);
            }
        );
    }

    private static (ISymbol targetProp, string genName)? GetCandidate(
        AssignmentExpressionSyntax assignment,
        SemanticModel semanticModel,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();

        if (GetPatterns(assignment, semanticModel) is not { } patterns) {
            return null;
        }

        var tree = SyntaxExtensions.BuildAccessTree(assignment.Right)
            .Select(x => (x, semanticModel.GetSymbolInfo(x).Symbol))
            .Where(x => x.Symbol != null);

        var endpoint = tree
            .Select(x => SyntaxExtensions.GetReturnType(x.Symbol!))
            .FirstOrDefault(x => x != null);

        // Ignoring state type here as we simply need to ensure that the resulting 
        // object is IState, the target type is taken from the target property
        if (endpoint == null || StateGeneratorUtils.GetStateTargetType(endpoint) == null) {
            return null;
        }

        // We only need unresolved symbols
        if (semanticModel.GetSymbolInfo(assignment.Left).Symbol != null) {
            return null;
        }

        if (GetTargetProperty(patterns, assignment, semanticModel) is not { } tuple) {
            return null;
        }

        return tuple;
    }

    private static string[]? GetPatterns(AssignmentExpressionSyntax assignment, SemanticModel semanticModel) {
        var methodSymbol = semanticModel.GetEnclosingSymbol(assignment.SpanStart);
        if (methodSymbol is not IMethodSymbol method) {
            return null;
        }

        var attr = method.GetDerivedAttribute(semanticModel, StateGeneratorUtils.AttributePath);
        if (attr == null) {
            return null;
        }

        // This expression also checks if Enabled is not null (defined)
        // so in case it IS null, it won't return, defaulting to true 
        if (attr.GetNamedArgument("Enabled") is { Value: not true }) {
            return null;
        }

        string[] patterns;
        if (attr.GetNamedArgument("Patterns") is { } patternsArg) {
            patterns = patternsArg.Values.Select(x => x.Value).OfType<string>().ToArray();
        } else {
            patterns = ["s{}"];
        }

        return patterns;
    }

    private static (ISymbol, string)? GetTargetProperty(string[] patterns, AssignmentExpressionSyntax assignment, SemanticModel semanticModel) {
        // Get type of the object that is being initialized
        var containingType = SyntaxExtensions.FindInitializerType(assignment.Parent!, semanticModel);
        if (containingType == null) {
            return null;
        }

        var statePropName = assignment.Left.ToString();

        foreach (var pattern in patterns) {
            var rex = pattern.Replace("{}", "([A-Za-z0-9_]+)");
            if (Regex.Match(statePropName, rex) is not { Success: true } match) {
                continue;
            }

            var matchedPropName = match.Groups[1].Value;
            if (containingType.GetMembers(matchedPropName).FirstOrDefault() is { } member) {
                return (member, statePropName);
            }
        }

        return null;
    }

    private static string GenerateTypeExtension(ISymbol type, IEnumerable<IGrouping<ISymbol?, string>> propGroups) {
        var inner = new StringBuilder();

        foreach (var nameGroup in propGroups) {
            var prop = nameGroup.Key!;
            var propType = SyntaxExtensions.GetReturnType(prop);
            var propName = prop.Name;

            foreach (var name in nameGroup) {
                var definition = """
                    public {0}<{1}> {2} {{
                        set {{
                            value.ValueChangedEvent += x => obj.{3} = x;
                        }}
                    }}

                    """;

                // Replace placeholders
                definition = string.Format(
                    definition,
                    StateGeneratorUtils.StatePath, // State<>
                    propType,                      // State target type (State<T>)
                    name,                          // Prop name
                    propName                       // Target prop name
                );

                // Prettify
                definition = definition.Insert(0, "\t\t").Replace("\n", "\n\t\t");

                inner.AppendLine(definition);
            }
        }

        var outer = """
            [System.CodeDom.Compiler.GeneratedCode("Reactive_StateGenerator", "1.0")]
            internal static class Reactive_{0}StateGenExt {{
                extension({1} obj) {{
            {2}
                }}
            }}
            """;

        outer = string.Format(outer, type.Name, type, inner.ToString().TrimEnd());

        return outer;
    }
}