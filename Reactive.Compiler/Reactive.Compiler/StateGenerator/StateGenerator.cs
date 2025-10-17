using System.Linq;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace Reactive.Compiler;

//TODO: rewrite to incremental
[Generator]
[SuppressMessage("Usage", "RS1035")]
[SuppressMessage("Usage", "RS1042")]
internal class StateGenerator : ISourceGenerator {
    public void Initialize(GeneratorInitializationContext context) {
        context.RegisterForSyntaxNotifications(() => new StateSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context) {
        if (context.SyntaxContextReceiver is not StateSyntaxReceiver receiver) {
            return;
        }

        var grouped = receiver.Candidates
            .Distinct()
            .GroupBy(
                x => x.targetProp,
                x => x.genName,
                SymbolEqualityComparer.Default
            )
            .GroupBy(
                x => x.Key.ContainingType,
                SymbolEqualityComparer.Default
            );

        foreach (var typeGroup in grouped) {
            var type = typeGroup.Key;
            var ext = GenerateTypeExtension(type!, typeGroup);

            var file = $"Reactive_{type!.Name}StateExt.g.cs";
            var content = SourceText.From(ext, Encoding.UTF8);
            
            context.AddSource(file, content);
        }
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