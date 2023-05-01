namespace Minsk.CodeAnalysis.Syntax
{
    // Stores information about the Syntax and how to parse and with what priority
    internal static class SyntaxFacts
    {
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind){
            switch (kind){
                // higher nr gets parsed with priority
                case SyntaxKind.MultiplicationToken:
                case SyntaxKind.DivideToken:
                    return 2;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 1;

                // while Precedence==0 -> no Binary Operator found
                default:
                    return 0;       
            }
        }

        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind){
            switch (kind){
                // higher nr gets parsed with priority
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 3;

                // while Precedence==0 -> no Binary Operator found
                default:
                    return 0;       
            }
        }
    }
}