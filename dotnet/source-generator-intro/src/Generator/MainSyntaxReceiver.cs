using Microsoft.CodeAnalysis;

namespace Generator;

public class MainSyntaxReceiver : ISyntaxReceiver
{
    public DefinitionAggregate Definitions { get; } = new();
    public Giveths Giveths { get; } = new();
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        Definitions.OnVisitSyntaxNode(syntaxNode);
        Giveths.OnVisitSyntaxNode(syntaxNode);
    }
}