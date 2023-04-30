namespace Minsk.CodeAnalysis
{
    // The main Interface/API to interact with to start the parsing process
    public sealed class SyntaxTree
    {
        public SyntaxTree( IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken eof)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = eof;
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        // the supposed main entry to parse our Tree from a string
        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
}