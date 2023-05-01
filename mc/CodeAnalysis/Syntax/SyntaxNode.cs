namespace Minsk.CodeAnalysis.Syntax
{
    // Base type for our SyntaxNodes
    public abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        // used for walking the tree ->
        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}