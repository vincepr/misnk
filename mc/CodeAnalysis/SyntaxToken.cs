namespace Minsk.CodeAnalysis
{
    // The "node-types" in our Tree that map to keywords, identifiers etc...
    class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Positon = position;
            Text = text;
            Value = value;
        }
        public override SyntaxKind Kind { get; }
        public int Positon { get; }
        public string Text { get; }
        public object Value { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}