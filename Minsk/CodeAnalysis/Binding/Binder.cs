using System;
using System.Collections.Generic;
using System.Linq;

using Minsk.CodeAnalysis.Syntax;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    // NOTE(crhodes)
    // Binder does the type checking for now

    internal sealed class Binder
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly Dictionary<VariableSymbol, object> _variables;

        public Binder(Dictionary<VariableSymbol, object> variables)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            _variables = variables;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);

        }

        public DiagnosticBag Diagnostics => _diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax.Kind:{syntax.Kind}", Common.LOG_CATEGORY);

            switch (syntax.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    Log.BINDER($"Exit BindLiteralExpression()", Common.LOG_CATEGORY, startTicks);

                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);

                case SyntaxKind.UnaryExpression:
                    Log.BINDER($"Exit BindUnaryExpression()", Common.LOG_CATEGORY, startTicks);

                    return BindUnaryExpression((UnaryExpressionSyntax)syntax);

                case SyntaxKind.BinaryExpression:
                    Log.BINDER($"Exit BindBinaryExpression()", Common.LOG_CATEGORY, startTicks);

                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);

                case SyntaxKind.ParenthesizedExpression:
                    Log.BINDER($"Exit BindParenthesizedExpression()", Common.LOG_CATEGORY, startTicks);

                    return BindParenthesizedExpression((ParenthesizedExpressionSyntax)syntax);

                case SyntaxKind.NameExpression:
                    Log.BINDER($"Exit BindNameExpression()", Common.LOG_CATEGORY, startTicks);

                    return BindNameExpression((NameExpressionSyntax)syntax);

                case SyntaxKind.AssignmentExpression:
                    Log.BINDER($"Exit BindAssignmentExpression()", Common.LOG_CATEGORY, startTicks);

                    return BindAssignmentExpression((AssignmentExpressionSyntax)syntax);

                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");

            }
        }

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter", Common.LOG_CATEGORY);
            Log.BINDER($"Exit BindExpression()", Common.LOG_CATEGORY, startTicks);

            return BindExpression(syntax.Expression);
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter", Common.LOG_CATEGORY);

            var value = syntax.Value ?? 0;

            Log.BINDER($"Exit new BoundLiteralExpression()", Common.LOG_CATEGORY, startTicks);

            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter", Common.LOG_CATEGORY);

            var name = syntax.IdentifierToken.Text;
            var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);

            if (variable == null)
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);

                Log.BINDER($"Exit new BoundLiteralExpression()", Common.LOG_CATEGORY, startTicks);

                return new BoundLiteralExpression(0);
            }

            Log.BINDER($"Exit new BoundVariableExpression()", Common.LOG_CATEGORY, startTicks);

            return new BoundVariableExpression(variable);
        }

        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter", Common.LOG_CATEGORY);

            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);

            var existingVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);

            if (existingVariable != null)
            {
                _variables.Remove(existingVariable);
            }

            var variable = new VariableSymbol(name, boundExpression.Type);
            _variables[variable] = null;

            Log.BINDER($"Exit new BoundAssignmentExpression()", Common.LOG_CATEGORY, startTicks);

            return new BoundAssignmentExpression(variable, boundExpression);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);

            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperator == null)
            {
                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperand.Type);

                // NOTE(crhodes)
                // Return something for now to avoid cascading errors.

                Log.BINDER($"Exit {boundOperand.Kind}", Common.LOG_CATEGORY, startTicks);

                return boundOperand;
            }

            Log.BINDER($"Exit new BoundUnaryExpression()", Common.LOG_CATEGORY, startTicks);

            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter", Common.LOG_CATEGORY);

            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperator == null)
            {
                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundLeft.Type, boundRight.Type);

                // NOTE(crhodes)
                // Return something for now to avoid cascading errors.

                Log.BINDER($"Exit {boundLeft.Kind}", Common.LOG_CATEGORY, startTicks);

                return boundLeft;
            }

            Log.BINDER($"Exit new BoundBinaryExpression()", Common.LOG_CATEGORY, startTicks);

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }
    }
}
