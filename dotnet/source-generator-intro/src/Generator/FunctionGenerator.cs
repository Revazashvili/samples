using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator;

public class MainSyntaxReceiver : ISyntaxReceiver
{
    public int Index { get; set; }
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax)
        {
            File.WriteAllText($@"/home/revazashvili/projects/samples/dotnet/source-generator-intro/src/Generator/{Index}.txt",syntaxNode.GetText().ToString());
            Index++;
        }
    }
}

[Generator]
public class FunctionGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var output = @"
public class Test
{
    public static void P() => Console.WriteLine(""Hello World!"");
}
";
        context.AddSource("hello.g.cs", output);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new MainSyntaxReceiver());
    }
}


