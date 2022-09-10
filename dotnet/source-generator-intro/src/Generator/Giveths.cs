using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator;

public class Giveths : ISyntaxReceiver
{
    private const string IdentifierName = "Give";
    public List<Capture> Captures { get; } = new();
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax
            {
                Name: IdentifierNameSyntax { Identifier.Text: IdentifierName }
            } attributeSyntax)
        {
            return;
        }

        var target = (attributeSyntax.ArgumentList!.Arguments.Single().Expression as LiteralExpressionSyntax)!.Token.ValueText;
        var methodDeclarationSyntax = attributeSyntax.GetParent<MethodDeclarationSyntax>();
        var classDeclarationSyntax = methodDeclarationSyntax.GetParent<ClassDeclarationSyntax>();
        Captures.Add(new (target,methodDeclarationSyntax,classDeclarationSyntax));
    }
    
    public record Capture(string TargetImplementation, MethodDeclarationSyntax Method,ClassDeclarationSyntax Class);
}