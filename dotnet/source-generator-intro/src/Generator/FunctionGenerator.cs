using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Generator;

[Generator]
public class FunctionGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxReceiver as MainSyntaxReceiver;
        foreach (var giveth in receiver.Giveths.Captures)
        {
            var definition = receiver.Definitions.Captures.FirstOrDefault(capture => capture.Key == giveth.TargetImplementation);
            if(definition is null)
                continue;
            var output = giveth.Class
                .WithMembers(new(CreateMethodDeclaration(giveth.Method, definition.Method)))
                .NormalizeWhitespace();
            context.AddSource($"{giveth.Class.Identifier.ValueText}.g.cs", output.GetText(Encoding.UTF8));
        }
    }

    private MethodDeclarationSyntax CreateMethodDeclaration(MethodDeclarationSyntax givethMethod,MethodDeclarationSyntax defMethod)
    {
        return MethodDeclaration(givethMethod.ReturnType, givethMethod.Identifier)
            .WithModifiers(givethMethod.Modifiers)
            .WithBody(defMethod.Body);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new MainSyntaxReceiver());
    }
}