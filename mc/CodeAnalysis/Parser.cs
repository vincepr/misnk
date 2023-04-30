namespace Minsk.CodeAnalysis
{
    // The Parser produces the Tree of our programm. Connecting the SyntaxTokens together to logical constructs
    class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private int _position;
        private List<string> _diagnostics = new List<string>();  // used for passing errors etc up.
        public Parser(string text)
        {
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

            } while (token.Kind != SyntaxKind.EOF);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);   // save errors the lexer propagates up
        }

        public IEnumerable<string> Diagnostics => _diagnostics; // exposing our Error Handling

        // utility if it becomes necessary to know the next token for context (!= == ...)
        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];

            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken Match(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            _diagnostics.Add($"ERROR: unexpected token: '{Current.Kind}', expected: '{kind}' ,in Parser.Match()");
            return new SyntaxToken(kind, Current.Positon, null, null);      // manifactured Artificial Token
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseTerm();
        }

        public SyntaxTree Parse()
        {
            var expression = ParseTerm();
            var endOfFileToken = Match(SyntaxKind.EOF);
            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }

        // addition || subtraction etc lower priority than ParseFactor (division, multiplication)
        private ExpressionSyntax ParseTerm()
        {
            var left = ParseFactor();

            while (Current.Kind == SyntaxKind.PlusToken 
                || Current.Kind == SyntaxKind.MinusToken)
            {
                var operatorToken = NextToken();
                var right = ParseFactor();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }

        // division || multiplication higher prio than ParseTerm (addition, subtraction)
        private ExpressionSyntax ParseFactor()
        {
            var left = ParsePrimaryExpression();

            while (Current.Kind == SyntaxKind.DivideToken
                || Current.Kind == SyntaxKind.MultiplicationToken)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }

        // numbers (later probably booleans etc.)
        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.Kind == SyntaxKind.OpenParenthesisToken)
            {
                var left = NextToken();
                var expression = ParseExpression();
                var right = Match(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);
            }

            var numberToken = Match(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }
    }
}