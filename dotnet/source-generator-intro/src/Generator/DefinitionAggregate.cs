using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator;

public class DefinitionAggregate : ISyntaxReceiver
{
    private const string IdentifierName = "Define";
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

        var methodDeclarationSyntax = attributeSyntax.GetParent<MethodDeclarationSyntax>();
        var key = methodDeclarationSyntax.Identifier.Text;
        Captures.Add(new (key, methodDeclarationSyntax));
    }

    public record Capture(string Key, MethodDeclarationSyntax Method);
}