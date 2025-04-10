using Microsoft.CodeAnalysis;

namespace Generator.Utils.ScriptHandler
{
    public interface IScriptHandler
    {
        void HandleSyntaxRoot(SyntaxNode syntaxNode);
    }
}