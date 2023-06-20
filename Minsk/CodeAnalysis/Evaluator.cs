using System;
using System.Collections.Generic;
using System.Xml.Linq;

using Minsk.CodeAnalysis.Binding;

using VNC;

namespace Minsk.CodeAnalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundStatement _root;
        private readonly Dictionary<VariableSymbol, object> _variables;

        // Cheat a bit

        private object _lastValue;

        public Evaluator(BoundStatement root, Dictionary<VariableSymbol, object> variables)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter root:{root.Kind} varables:{variables.Count}", Common.LOG_CATEGORY);

            _root = root;
            _variables = variables;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public object Evaluate()
        {
            Int64 startTicks = Log.EVALUATOR($"Enter", Common.LOG_CATEGORY);

            EvaluateStatement(_root);

            Log.EVALUATOR($"Exit {_lastValue}", Common.LOG_CATEGORY, startTicks);

            return _lastValue;
        }

        private void EvaluateStatement(BoundStatement node)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter node:{node.Kind}", Common.LOG_CATEGORY);

            switch (node.Kind)
            {
                case BoundNodeKind.BlockStatement:
                    {
                        EvaluateBlockStatement((BoundBlockStatement)node);
                        break;
                    }

                case BoundNodeKind.VariableDeclaration:
                    {
                        EvaluateVariableDeclaration((BoundVariableDeclaration)node);
                        break;
                    }

                case BoundNodeKind.IfStatement:
                    {
                        EvaluateIfStatement((BoundIfStatement)node);
                        break;
                    }

                case BoundNodeKind.WhileStatement:
                    {
                        EvaluateWhileStatement((BoundWhileStatement)node);
                        break;
                    }

                case BoundNodeKind.ForStatement:
                    {
                        EvaluateForStatement((BoundForStatement)node);
                        break;
                    }

                case BoundNodeKind.ExpressionStatement:
                    {
                        EvaluateExpressionStatement((BoundExpressionStatement)node);
                        break;
                    }

                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }

            Log.EVALUATOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void EvaluateVariableDeclaration(BoundVariableDeclaration node)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter node:{node.Kind}", Common.LOG_CATEGORY);

            var value = EvaluateExpression(node.Initializer);
            _variables[node.Variable] = value;
            _lastValue = value;

            Log.EVALUATOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void EvaluateBlockStatement(BoundBlockStatement node)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter node:{node.Kind}", Common.LOG_CATEGORY);

            foreach (var statement in node.Statements )
            {
                EvaluateStatement(statement);
            }

            Log.EVALUATOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void EvaluateIfStatement(BoundIfStatement node)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter node:{node.Kind}", Common.LOG_CATEGORY);

            var condition = (Boolean)EvaluateExpression(node.Condition);

            if (condition)
            {
                EvaluateStatement(node.ThenStatement);
            }
            else if (node.ElseStatement != null)
            {
                EvaluateStatement(node.ElseStatement);
            }

            Log.EVALUATOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void EvaluateWhileStatement(BoundWhileStatement node)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter node:{node.Kind}", Common.LOG_CATEGORY);

            while ((Boolean)EvaluateExpression(node.Condition))
            {
                EvaluateStatement(node.Body);
            }

            Log.EVALUATOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void EvaluateForStatement(BoundForStatement node)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter node:{node.Kind}", Common.LOG_CATEGORY);

            var lowerBound = (Int32)EvaluateExpression(node.LowerBound);
            var upperBound = (Int32)EvaluateExpression(node.UpperBound);

            for (int i = lowerBound; i <= upperBound; i++)
            {
                _variables[node.Variable] = i;
                EvaluateStatement(node.Body);
            }

            Log.EVALUATOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void EvaluateExpressionStatement(BoundExpressionStatement node)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter node:{node.Kind}", Common.LOG_CATEGORY);

            _lastValue = EvaluateExpression(node.Expression);

            Log.EVALUATOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter node:{node.Kind}", Common.LOG_CATEGORY);

            switch (node.Kind)
            {
                case BoundNodeKind.LiteralExpression:
                    {
                        var value = EvaluateLiteralExpression((BoundLiteralExpression)node);

                        Log.EVALUATOR($"Exit value:{value}", Common.LOG_CATEGORY, startTicks);

                        return value;
                    }

                case BoundNodeKind.VariableExpression:
                    {
                        var value = EvaluateVariableExpression((BoundVariableExpression)node);

                        Log.EVALUATOR($"Exit value:{value}", Common.LOG_CATEGORY, startTicks);

                        return value;
                    }

                case BoundNodeKind.AssignmentExpression:
                    {
                        var value = EvaluateAssignmentExpression((BoundAssignmentExpression)node);

                        Log.EVALUATOR($"Exit value:{value}", Common.LOG_CATEGORY, startTicks);

                        return value;
                    }

                case BoundNodeKind.UnaryExpression:
                    {
                        var value = EvaluateUnaryExpression((BoundUnaryExpression)node);

                        Log.EVALUATOR($"Exit value:{value}", Common.LOG_CATEGORY, startTicks);

                        return value;
                    }

                case BoundNodeKind.BinaryExpression:
                    {
                        var value = EvaluateBinaryExpression((BoundBinaryExpression)node);

                        Log.EVALUATOR($"Exit value:{value}", Common.LOG_CATEGORY, startTicks);

                        return value;
                    }


                //if (node is BoundParenthesizedExpression p)
                //{
                //    Log.Trace($"Exit", Common.LOG_CATEGORY, startTicks);

                //    return EvaluateExpression(p.Expression);
                //}

                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }

        private static object EvaluateLiteralExpression(BoundLiteralExpression n)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter n:{n.Kind}", Common.LOG_CATEGORY);
            Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

            return n.Value;
        }

        private object EvaluateVariableExpression(BoundVariableExpression v)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter v:{v.Kind}", Common.LOG_CATEGORY);
            Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

            return _variables[v.Variable];
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression a)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter a:{a.Kind}", Common.LOG_CATEGORY);

            var value = EvaluateExpression(a.Expression);
            _variables[a.Variable] = value;

            Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

            return value;
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression u)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter u:{u.Kind}", Common.LOG_CATEGORY);

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

        private object EvaluateBinaryExpression(BoundBinaryExpression b)
        {
            Int64 startTicks = Log.EVALUATOR($"Enter b:{b.Kind}", Common.LOG_CATEGORY);

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

                case BoundBinaryOperatorKind.Less:
                    Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                    return (int)left < (int)right;

                case BoundBinaryOperatorKind.LessOrEquals:
                    Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                    return (int)left <= (int)right;

                case BoundBinaryOperatorKind.Greater:
                    Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                    return (int)left > (int)right;

                case BoundBinaryOperatorKind.GreaterOrEquals:
                    Log.EVALUATOR($"Exit", Common.LOG_CATEGORY, startTicks);

                    return (int)left >= (int)right;

                default:
                    throw new Exception($"Unexpected Binary Operator {b.Op}");
            }
        }
    }
}