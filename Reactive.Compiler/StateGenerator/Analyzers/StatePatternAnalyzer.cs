using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Reactive.Compiler;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class StatePatternAnalyzer : DiagnosticAnalyzer {
    private static readonly DiagnosticDescriptor PatternInvalidRule = new(
        "RV002",
        "Invalid pattern",
        "Pattern '{0}' is not valid. It should contain an identifier and a {{}} to match, e.g. 'state_{{}}' or '{{}}State'.",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor PatternIncompleteRule = new(
        "RV001",
        "Incomplete pattern",
        "Pattern '{0}' is incomplete. You should specify a rule to match.",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        PatternInvalidRule,
        PatternIncompleteRule
    );

    public override void Initialize(AnalysisContext context) {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(AnalyzePatternsArgument, SyntaxKind.AttributeArgument);
    }

    private static void AnalyzePatternsArgument(SyntaxNodeAnalysisContext context) {
        var arg = (AttributeArgumentSyntax)context.Node;

        if (arg.NameEquals?.Name is not { } name || name.ToString() != "Patterns") {
            return;
        }

        var symbol = context.SemanticModel.GetSymbolInfo(name).Symbol;
        if (symbol?.ContainingType.ToString() != StateGeneratorUtils.AttributePath) {
            return;
        }

        if (arg.Expression is not CollectionExpressionSyntax expr) {
            return;
        }

        foreach (var pattern in expr.Elements) {
            var literal = pattern.ToString().Replace("\"", "");

            Diagnostic? diagnostic = null;

            // Patterns without additional symbols are considered incomplete
            if (literal is "" or "{}") {
                diagnostic = Diagnostic.Create(PatternIncompleteRule, pattern.GetLocation(), literal);
            }

            // We allow only one placeholder
            if (literal.Count(x => x is '{') is not 1 ||
                literal.Count(x => x is '}') is not 1
            ) {
                diagnostic = Diagnostic.Create(PatternInvalidRule, pattern.GetLocation(), literal);
            }

            if (diagnostic != null) {
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}