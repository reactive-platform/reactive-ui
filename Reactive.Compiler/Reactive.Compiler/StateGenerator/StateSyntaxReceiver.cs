using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Reactive.Compiler;

internal class StateSyntaxReceiver : ISyntaxContextReceiver {
    public readonly List<(ISymbol targetProp, string prefix)> Candidates = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context) {
        if (context.Node is not AssignmentExpressionSyntax { RawKind: (int)SyntaxKind.SimpleAssignmentExpression } assignment) {
            return;
        }

        var semanticModel = context.SemanticModel;

        var tree = SyntaxExtensions.BuildAccessTree(assignment.Right)
            .Select(x => (x, semanticModel.GetSymbolInfo(x).Symbol))
            .Where(x => x.Symbol != null);

        var endpoint = tree
            .Select(x => SyntaxExtensions.GetReturnType(x.Symbol!))
            .FirstOrDefault(x => x != null);
        
        // Ignoring state type here as we simply need to ensure that the resulting 
        // object is IState, the target type is taken from the target property
        if (endpoint == null || StateGeneratorUtils.GetStateTargetType(endpoint) == null) {
            return;
        }

        // We only need unresolved symbols
        if (semanticModel.GetSymbolInfo(assignment.Left).Symbol != null) {
            return;
        }

        if (GetTargetProperty(assignment, semanticModel) is not { } targetProp) {
            return;
        }

        Candidates.Add((targetProp, "s"));
    }

    private static ISymbol? GetTargetProperty(AssignmentExpressionSyntax assignment, SemanticModel semanticModel) {
        // Get type of the object that is being initialized
        var containingType = SyntaxExtensions.FindInitializerType(assignment.Parent!, semanticModel);
        if (containingType == null) {
            return null;
        }

        var propName = assignment.Left.ToString().TrimStart('s');
        var members = containingType.GetMembers(propName);
        
        return members.FirstOrDefault();
    }
}