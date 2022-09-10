using Microsoft.CodeAnalysis;

namespace Generator;

public static class SyntaxNodeExtensions
{
    public static T GetParent<T>(this SyntaxNode node)
    {
        var parent = node.Parent;
        while (true)
        {
            if (parent is null)
                throw new Exception("Parent is null");
            if (parent is T t)
                return t;
            parent = parent.Parent;
        }
    }
}