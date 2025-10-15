using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Reactive.Compiler;

internal static class SyntaxExtensions {
    /// <summary>
    /// Attempts to find a type of the initializer expression.
    /// </summary>
    /// <returns></returns>
    public static ITypeSymbol? FindInitializerType(SyntaxNode syntax, SemanticModel model) {
        var current = syntax;

        while (current != null) {
            switch (current) {
                // `new Foo { ... }`
                case ObjectCreationExpressionSyntax objectCreation:
                    return model.GetTypeInfo(objectCreation).Type;

                // `new Foo { Another = { ... } }`
                case AssignmentExpressionSyntax assignment:
                    var leftSymbol = model.GetSymbolInfo(assignment.Left).Symbol;

                    if (leftSymbol != null && GetReturnType(leftSymbol) is { } returnType) {
                        return returnType;
                    }

                    break;

                // `{ ... }` itself
                case InitializerExpressionSyntax:
                    break;
            }

            current = current.Parent;
        }

        return null;
    }

    /// <summary>
    /// Walks through the expression building a hierarchy of access methods.
    /// </summary>
    public static IEnumerable<ExpressionSyntax> BuildAccessTree(ExpressionSyntax expression) {
        yield return expression;

        while (true) {
            switch (expression) {
                case InvocationExpressionSyntax invocation:
                    // Extract the method target expression
                    switch (invocation.Expression) {
                        case MemberAccessExpressionSyntax member:
                            expression = member.Expression;
                            break;

                        default:
                            yield break;
                    }

                    yield return invocation;
                    continue;

                // This kind of syntax is skipped as it does not carry any sensitive info
                case PostfixUnaryExpressionSyntax unary:
                    expression = unary.Operand;
                    continue;

                case MemberAccessExpressionSyntax memberAccess:
                    expression = memberAccess.Expression;
                    break;

                case ConditionalAccessExpressionSyntax conditional:
                    expression = conditional.Expression;
                    break;

                case ElementAccessExpressionSyntax elementAccess:
                    expression = elementAccess.Expression;
                    break;

                case CastExpressionSyntax cast:
                    expression = cast.Expression;
                    break;

                case ParenthesizedExpressionSyntax paren:
                    expression = paren.Expression;
                    break;

                case IdentifierNameSyntax identifier:
                    yield return identifier;
                    yield break;

                default:
                    yield break;
            }

            yield return expression;
        }
    }

    public static ITypeSymbol? GetReturnType(ISymbol symbol) {
        return symbol switch {
            IMethodSymbol method => method.ReturnType,
            IFieldSymbol field => field.Type,
            ILocalSymbol local => local.Type,
            IPropertySymbol property => property.Type,
            _ => null
        };
    }
}