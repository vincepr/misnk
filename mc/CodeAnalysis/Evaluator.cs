namespace Minsk.CodeAnalysis
{

    // calculates 
    class Evaluator
    {
        private readonly ExpressionSyntax _root;
        public Evaluator(ExpressionSyntax _root)
        {
            this._root = _root;
        }


        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            // BinaryExpression
            // NumberExpression
            if (node is NumberExpressionSyntax n)
                return (int) n.NumberToken.Value;   // asserting int -> can only call this when parsing was success

            if (node is BinaryExpressionSyntax b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                if (b.OperatorToken.Kind == SyntaxKind.PlusToken)
                    return left + right;
                else if (b.OperatorToken.Kind == SyntaxKind.MinusToken)
                    return left - right;
                else if (b.OperatorToken.Kind == SyntaxKind.MultiplicationToken)
                    return left * right;
                else if (b.OperatorToken.Kind == SyntaxKind.DivideToken)
                    return left / right;
                else
                    throw new Exception($"Unexpected binary operator '{b.OperatorToken.Kind}' , in Evaluator.EvaluateExpression()");
            }

            if (node is ParenthesizedExpressionSyntax p)
            {
                return EvaluateExpression(p.Expression);
            }

            throw new Exception($"Unexpected node: '{node.Kind}' , in Evaluator.EvaluateExpression()");
        }
    }
}