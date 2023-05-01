using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
    // Beside the AST for parsing the logic, this Tree will store info
    // about types to enable type checking later on.

    // The binder walks our Already Existing Syntax Tree and creates this
    // structure.
    internal enum BoundNodeKind{
        LiteralExpression,
        UnaryExpression
    }

    internal abstract class BoundNode{
        public abstract BoundNodeKind Kind {get;}
    }

    internal abstract class BoundExpression : BoundNode{
        public abstract Type Type {get;}
    }

    internal enum BoundUnaryOperatorKind{
        Identity,
        Negation,
    }

    internal sealed class BoundLiteralExpression : BoundExpression
    {
        public BoundLiteralExpression(object value){
            Value = value;
        }
        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public override Type Type => Value.GetType();
        public object Value {get;}
    }

    internal sealed class BoundUnaryExpression : BoundExpression{

        public BoundUnaryExpression(BoundUnaryOperatorKind operatorKind, BoundExpression operand){
            OperatorKind = operatorKind;
            Operand = operand;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Operand.Type;
        public BoundUnaryOperatorKind OperatorKind { get; }
        public BoundExpression Operand { get; }
    }

    internal enum BoundBinaryOperatorKind{
        Addition,
        Subtraction,
        Multiplication,
        Division,

    }

    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorKind operatorKind, BoundExpression right){
            Left = left;
            OperatorKind = operatorKind;
            Right = right;
        }
        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public override Type Type => Left.Type;

        public BoundExpression Left {get;}
        public BoundBinaryOperatorKind OperatorKind {get;}
        public BoundExpression Right {get;}
    }

    // Our construct to walk the (already existing) Syntax-Tree and create
    // our representation of Types
    internal sealed class Binder{
        public BoundExpression BindExpression(ExpressionSyntax syntax){
            switch (syntax.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);
                default:
                    throw new Exception($"ERROR: Unexpected syntayx {syntax.Kind} in Binder.BindExpression()");
            }
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax){
            var value = syntax.LiteralToken.Value as int? ?? 0;
            return new BoundLiteralExpression(value);

        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax){
            var boundOperatorKind = BindUnaryOperatorKind(syntax.OperatorToken.Kind);
            var boundOperand = BindExpression(syntax.Operand);
            return new BoundUnaryExpression(boundOperatorKind, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax){
            var boundLeft = BindExpression(syntax.Left);
            var boundOperatorKind = BindBinaryOperatorKind(syntax.OperatorToken.Kind);
            var boundRight = BindExpression(syntax.Right);
            return new BoundBinaryExpression(boundLeft, boundOperatorKind, boundRight);
        }

        private BoundUnaryOperatorKind BindUnaryOperatorKind(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                    return BoundUnaryOperatorKind.Identity;
                case SyntaxKind.MinusToken:
                    return BoundUnaryOperatorKind.Negation;
                default:
                    throw new Exception($"ERROR: Unexpected unary operator {kind} in Binder.BindExpression()");
            }
        }

        private BoundBinaryOperatorKind BindBinaryOperatorKind(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                    return BoundBinaryOperatorKind.Addition;
                case SyntaxKind.MinusToken:
                    return BoundBinaryOperatorKind.Subtraction;
                case SyntaxKind.MultiplicationToken:
                    return BoundBinaryOperatorKind.Multiplication;
                case SyntaxKind.DivideToken:
                    return BoundBinaryOperatorKind.Division;
                default:
                    throw new Exception($"ERROR: Unexpected binary operator {kind} in Binder.BindExpression()");
            }
        }
    }
}