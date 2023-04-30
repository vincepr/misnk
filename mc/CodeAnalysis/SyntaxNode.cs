namespace Minsk.CodeAnalysis
{
    // Base type for our SyntaxNodes
    abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        // used for walking the tree ->
        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}