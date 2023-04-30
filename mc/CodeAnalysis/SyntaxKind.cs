namespace Minsk.CodeAnalysis
{
    // 1 + 2 * 3
    // gets parsed into a treel like:
    //
    //    +
    //   / \
    //  1   *
    //     / \
    //    2   3



    public enum SyntaxKind
    {   
        // Tokens:
        BadToken,
        EOFToken,
        WhitespaceToken,
        NumberToken,        // i32 at the moment

        // Operators
        PlusToken,
        MinusToken,
        MultiplicationToken,
        DivideToken,
        OpenParenthesisToken,
        CloseParenthesisToken,

        // Expressions
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpressionSyntax
    }
}