namespace Minsk.CodeAnalysis
{
    // The Parser produces the Tree of our programm. Connecting the SyntaxTokens together to logical constructs
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private int _position;
        private List<string> _diagnostics = new List<string>();  // used for passing errors etc up.
        public Parser(string text){
            var tokens = new List<SyntaxToken>();
            var lexer = new Lexer(text);
            SyntaxToken token;
            do
            {
                token = lexer.NextToken();

                if (token.Kind != SyntaxKind.WhitespaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }

            } while (token.Kind != SyntaxKind.EOFToken);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);   // save errors the lexer propagates up
        }

        public IEnumerable<string> Diagnostics => _diagnostics; // exposing our Error Handling

        // utility if it becomes necessary to know the next token for context (!= == ...)
        private SyntaxToken Peek(int offset){
            var index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];

            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken(){
            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind){
            if (Current.Kind == kind)
                return NextToken();

            _diagnostics.Add($"ERROR: unexpected token: '{Current.Kind}', expected: '{kind}' ,in Parser.Match()");
            return new SyntaxToken(kind, Current.Positon, null, null);      // manifactured Artificial Token
        }

        public SyntaxTree Parse(){
            var expression = ParseExpression();
            var endOfFileToken = MatchToken(SyntaxKind.EOFToken);
            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }

        // to the order in wich ParseXxxs and make Them have Priority over eachother (! > * > +- ...)
        private ExpressionSyntax ParseExpression(int parentPrecedence = 0){
            var left = ParsePrimaryExpression();

            while (true){
                var precedence = GetBinaryOperatorPrecedence(Current.Kind);
                if (precedence == 0 || precedence <= parentPrecedence)
                    break;
                var operatorToken = NextToken();
                var right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);

            }
            return left;
        }

        private static int GetBinaryOperatorPrecedence(SyntaxKind kind){
            switch (kind){
                // higher nr gets parsed with priority
                case SyntaxKind.MultiplicationToken:
                case SyntaxKind.DivideToken:
                    return 2;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 1;

                default:
                    return 0;       // while Precedence==0 -> no Binary Operator
            }
        }

        // numbers (later probably booleans etc.)
        private ExpressionSyntax ParsePrimaryExpression(){
            if (Current.Kind == SyntaxKind.OpenParenthesisToken)
            {
                var left = NextToken();
                var expression = ParseExpression();
                var right = MatchToken(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);
            }

            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}