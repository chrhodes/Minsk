using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Minsk.CodeAnalysis.Syntax;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private BoundScope _scope;

        public Binder(BoundScope parent)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter parent:{parent}", Common.LOG_CATEGORY);

            _scope = new BoundScope(parent);

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnitSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter previous:{previous} syntax:{syntax.Kind}", Common.LOG_CATEGORY);

            var parentScope = CreateParentScopes(previous);
            var binder = new Binder(parentScope);
            var statement = binder.BindStatement(syntax.Statement);
            var variables = binder._scope.GetDeclaredVariables();
            var diagnostics = binder.Diagnostics.ToImmutableArray();

            if (previous != null)
            {
                diagnostics = diagnostics.InsertRange(0, previous.Diagnostics);
            }

            Log.CONSTRUCTOR($"Exit new BoundGlobalScope()", Common.LOG_CATEGORY, startTicks);

            return new BoundGlobalScope(previous, diagnostics, variables, statement);
        }

        private static BoundScope CreateParentScopes(BoundGlobalScope previous)
        {
            Int64 startTicks = Log.BINDER($"Enter previous:{previous}", Common.LOG_CATEGORY);

            // submission 3 -> submission 2 -> submission 1 from REPL

            var stack = new Stack<BoundGlobalScope>();

            while (previous != null)
            {
                stack.Push(previous);
                previous = previous.Previous;
            }

            BoundScope parent = null;

            while (stack.Count > 0)
            {
                previous = stack.Pop();
                var scope = new BoundScope(parent);

                foreach (var v in previous.Variables )
                {
                    scope.TryDeclare(v);
                }

                parent = scope;
            }

            Log.CONSTRUCTOR($"Exit {parent}", Common.LOG_CATEGORY, startTicks);

            return parent;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private BoundStatement BindStatement(StatementSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);

            switch (syntax.Kind)
            {
                case SyntaxKind.BlockStatement:
                    Log.BINDER($"Exit BindBlockStatement()", Common.LOG_CATEGORY, startTicks);

                    return BindBlockStatement((BlockStatementSyntax)syntax);

                case SyntaxKind.VariableDeclaration:
                    Log.BINDER($"Exit BindVariableDeclaration()", Common.LOG_CATEGORY, startTicks);

                    return BindVariableDeclaration((VariableDeclarationSyntax)syntax);

                case SyntaxKind.IfStatement:
                    Log.BINDER($"Exit BindIfStatement()", Common.LOG_CATEGORY, startTicks);

                    return BindIfStatement((IfStatementSyntax)syntax);

                case SyntaxKind.WhileStatement:
                    Log.BINDER($"Exit BindWhileStatement()", Common.LOG_CATEGORY, startTicks);

                    return BindWhileStatement((WhileStatementSyntax)syntax);

                case SyntaxKind.ForStatement:
                    Log.BINDER($"Exit BindForStatement()", Common.LOG_CATEGORY, startTicks);

                    return BindForStatement((ForStatementSyntax)syntax);

                case SyntaxKind.ExpressionStatement:
                    Log.BINDER($"Exit BindExpressionStatement()", Common.LOG_CATEGORY, startTicks);

                    return BindExpressionStatement((ExpressionStatementSyntax)syntax);

                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundStatement BindBlockStatement(BlockStatementSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);

            var statements = ImmutableArray.CreateBuilder<BoundStatement>();
            _scope = new BoundScope(_scope);

            foreach (var statementSyntax in syntax.Statements)
            {
                var statement = BindStatement(statementSyntax);
                statements.Add(statement);
            }

            _scope = _scope.Parent;

            Log.CONSTRUCTOR($"Exit new BoundBlockStatement()", Common.LOG_CATEGORY, startTicks);

            return new BoundBlockStatement(statements.ToImmutable());
        }

        private BoundStatement BindVariableDeclaration(VariableDeclarationSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);

            var name = syntax.Identifier.Text;
            var initializer = BindExpression(syntax.Initializer);
            var isReadOnly = syntax.Keyword.Kind == SyntaxKind.LetKeyword;
            var variable = new VariableSymbol(name, isReadOnly, initializer.Type);

            if (!_scope.TryDeclare(variable))
            {
                _diagnostics.ReportVariableAlreadyDeclared(syntax.Identifier.Span, name);
            }

            Log.CONSTRUCTOR($"Exit new BoundVariableDeclaration()", Common.LOG_CATEGORY, startTicks);

            return new BoundVariableDeclaration(variable, initializer);
        }

        private BoundStatement BindIfStatement(IfStatementSyntax syntax)
        {

            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);
            var condition = BindExpression(syntax.Condition, typeof(Boolean));
            var thenStatement = BindStatement(syntax.ThenStatement);
            var elseStatement = syntax.ElseClause == null
                ? null
                : BindStatement(syntax.ElseClause.ElseStatement);

            Log.CONSTRUCTOR($"Exit new BoundIfStatemen()", Common.LOG_CATEGORY, startTicks);

            return new BoundIfStatement(condition, thenStatement, elseStatement);
        }

        private BoundStatement BindWhileStatement(WhileStatementSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);

            var condition = BindExpression(syntax.Condition, typeof(Boolean));
            var body = BindStatement(syntax.Body);

            Log.CONSTRUCTOR($"Exit new BoundWhileStatemen()", Common.LOG_CATEGORY, startTicks);

            return new BoundWhileStatement(condition, body);
        }

        private BoundStatement BindForStatement(ForStatementSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);

            var lowerBound = BindExpression(syntax.LowerBound, typeof(Int32));
            var upperBound = BindExpression(syntax.UpperBound, typeof(Int32));

            _scope = new BoundScope(_scope);

            var name = syntax.Identifier.Text;
            var variable = new VariableSymbol(name, true, typeof(Int32));

            if (! _scope.TryDeclare(variable))
            {
                _diagnostics.ReportVariableAlreadyDeclared(syntax.Identifier.Span, name);
            }

            var body = BindStatement(syntax.Body);

            _scope = _scope.Parent;

            Log.CONSTRUCTOR($"Exit new BoundForStatement()", Common.LOG_CATEGORY, startTicks);

            return new BoundForStatement(variable, lowerBound, upperBound, body);
        }

        private BoundStatement BindExpressionStatement(ExpressionStatementSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);

            var expression = BindExpression(syntax.Expression);

            Log.CONSTRUCTOR($"Exit new BoundExpressionStatement()", Common.LOG_CATEGORY, startTicks);

            return new BoundExpressionStatement(expression);
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax, Type targetType)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind} targetType:{targetType}", Common.LOG_CATEGORY);

            var result = BindExpression(syntax);

            if (result.Type != targetType)
            {
                _diagnostics.ReportCannotConvert(syntax.Span, result.Type, targetType);
            }

            Log.CONSTRUCTOR($"Exit {result.Kind}", Common.LOG_CATEGORY, startTicks);

            return result;
        }

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);

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
            Int64 startTicks = Log.BINDER($"Enter syntax: {syntax.Kind}", Common.LOG_CATEGORY);

            Log.CONSTRUCTOR($"Exit BindExpression()", Common.LOG_CATEGORY, startTicks);

            return BindExpression(syntax.Expression);
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax: {syntax.Kind}", Common.LOG_CATEGORY);

            var value = syntax.Value ?? 0;

            Log.BINDER($"Exit new BoundLiteralExprression()", Common.LOG_CATEGORY, startTicks);

            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);

            var name = syntax.IdentifierToken.Text;

            if (string.IsNullOrEmpty(name))
            {
                // This means the token was inserted by the parser.
                // We already reported an error so we can just return
                // an error expression.

                Log.CONSTRUCTOR($"Exit new BoundLiteralExpression()", Common.LOG_CATEGORY, startTicks);

                return new BoundLiteralExpression(0);
            }

            if (! _scope.TryLookup(name, out var variable))
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
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);

            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);

            if (! _scope.TryLookup(name, out var variable))
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);

                Log.BINDER($"Exit {boundExpression.Kind}", Common.LOG_CATEGORY, startTicks);

                return boundExpression;
            }

            if (variable.IsReadOnly)
            {
                _diagnostics.ReportCannotAssign(syntax.EqualsToken.Span, name);
            }

            if (boundExpression.Type != variable.Type)
            {
                _diagnostics.ReportCannotConvert(syntax.Expression.Span, boundExpression.Type, variable.Type);

                Log.BINDER($"Exit {boundExpression.Kind}", Common.LOG_CATEGORY, startTicks);

                return boundExpression;
            }

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
            Int64 startTicks = Log.BINDER($"Enter syntax:{syntax.Kind}", Common.LOG_CATEGORY);

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
