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



    enum SyntaxKind
    {
        NumberToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        DivideToken,
        MultiplicationToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BadToken,
        EOF,
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpressionSyntax
    }
}