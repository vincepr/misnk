namespace Minsk.CodeAnalysis
{
    // The Lexer/Tokenizer splits the Input(String) -> SyntaxTokens
    class Lexer
    {
        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new List<string>(); // used for passing errors etc up.

        public Lexer(string text)
        {
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics; // exposing our Error Handling
        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';
                return _text[_position];
            }
        }

        // iterator to return the next position
        private void Next()
        {
            _position++;
        }

        public SyntaxToken NextToken()
        {
            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EOF, _position, "\0", null);
            }    

            // Token is Digit 0-9
            if (char.IsDigit(Current))
            {
                var start = _position;
                while (char.IsDigit(Current))
                    Next();
                var length = _position - start;
                var text = _text.Substring(start, length);
                if (! int.TryParse(text, out var value))
                    _diagnostics.Add($"ERROR: failed TryParse '{text}' to INT32, in Lexer.NextToken()");
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            // Token is whitespace
            if (char.IsWhiteSpace(Current))
            {
                var start = _position;
                while (char.IsWhiteSpace(Current))
                    Next();
                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            // arithmetic operators + - * / ( )
            if (Current == '+')
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            if (Current == '-')
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
            if (Current == '/')
                return new SyntaxToken(SyntaxKind.DivideToken, _position++, "/", null);
            if (Current == '*')
                return new SyntaxToken(SyntaxKind.MultiplicationToken, _position++, "*", null);
            if (Current == '(')
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
            if (Current == ')')
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);

            _diagnostics.Add($"ERROR: bad character input: '{Current}' in Lexer.NextToken()");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1,1), null);
        }
    }
}