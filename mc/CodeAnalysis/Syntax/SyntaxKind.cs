namespace Minsk.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {   
        // Tokens:
        BadToken,
        EOFToken,
        WhitespaceToken,
        NumberToken,        // represents i32 at the moment

        // Operators
        PlusToken,
        MinusToken,
        MultiplicationToken,
        DivideToken,
        OpenParenthesisToken,
        CloseParenthesisToken,

        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpressionSyntax,
        UnaryExpression
    }
}