using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis
{

    // calculates 
    public sealed class Evaluator
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
            if (node is LiteralExpressionSyntax n)
                return (int) n.LiteralToken.Value;   // asserting int -> can only call this when parsing was success
            if (node is UnaryExpressionSyntax u)
                {
                    var operand = EvaluateExpression(u.Operand);

                    if (u.OperatorToken.Kind == SyntaxKind.PlusToken)
                        return operand;
                    if (u.OperatorToken.Kind == SyntaxKind.MinusToken)
                        return -operand;
                    else
                        throw new Exception($"Unexpected UnaryOperator '{u.OperatorToken.Kind}' , in Evaluator.EvaluateExpression()");
                }
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