using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Reactive.Compiler;

internal class StateSyntaxReceiver : ISyntaxContextReceiver {
    public readonly List<(ISymbol targetProp, string genName)> Candidates = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context) {
        if (context.Node is not AssignmentExpressionSyntax { RawKind: (int)SyntaxKind.SimpleAssignmentExpression } assignment) {
            return;
        }

        var semanticModel = context.SemanticModel;

        if (GetPatterns(assignment, semanticModel) is not { } patterns) {
            return;
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
            return;
        }

        // We only need unresolved symbols
        if (semanticModel.GetSymbolInfo(assignment.Left).Symbol != null) {
            return;
        }

        if (GetTargetProperty(patterns, assignment, semanticModel) is not { } tuple) {
            return;
        }

        Candidates.Add(tuple);
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
}