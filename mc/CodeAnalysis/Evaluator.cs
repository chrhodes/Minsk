using System;

using Minsk.CodeAnalysis.Binding;

using VNC;

namespace Minsk.CodeAnalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter: root:{root.Kind}", Common.LOG_CATEGORY);

            _root = root;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public object Evaluate()
        {
            Int64 startTicks = Log.EVALUATOR($"Enter/Exit", Common.LOG_CATEGORY);

            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter node:{node.Kind}", Common.LOG_CATEGORY);

            if (node is BoundLiteralExpression n)
            {
                Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                return n.Value;
            }

            if (node is BoundUnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.Op.Kind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                        return (int)operand;

                    case BoundUnaryOperatorKind.Negation:
                        Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                        return -(int)operand;

                    case BoundUnaryOperatorKind.LogicalNegation:
                        Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                        return !(Boolean)operand;

                    default:
                        throw new Exception($"Unexpected Unary Operator {u.Op}");
                }
            }

            if (node is BoundBinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch (b.Op.Kind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                        return (int)left + (int)right;

                    case BoundBinaryOperatorKind.Subtraction:
                        Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                        return (int)left - (int)right;

                    case BoundBinaryOperatorKind.Multiplication:
                        Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                        return (int)left * (int)right;

                    case BoundBinaryOperatorKind.Division:
                        Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                        return (int)left / (int)right;


                    case BoundBinaryOperatorKind.LogicalAnd:
                        Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                        return (Boolean)left && (Boolean)right;


                    case BoundBinaryOperatorKind.LogicalOr:
                        Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                        return (Boolean)left || (Boolean)right;

                    case BoundBinaryOperatorKind.Equals:
                        Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                        return Equals(left, right);

                    case BoundBinaryOperatorKind.NotEquals:
                        Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                        return !Equals(left, right);

                    default:
                        throw new Exception($"Unexpected Binary Operator {b.Op}");
                }
            }

            //if (node is BoundParenthesizedExpression p)
            //{
            //    Log.Trace($"Exit", Common.LOG_CATEGORY, startTicks);

            //    return EvaluateExpression(p.Expression);
            //}

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}