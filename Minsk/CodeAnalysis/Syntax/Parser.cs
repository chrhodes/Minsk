using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Minsk.CodeAnalysis.Text;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    //NOTE(crhodes)
    // Parser assembles the words into sentences
    // by producing Syntax Trees (sentences)
    // from the Syntax Tokens (words)

    internal sealed class Parser
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly SourceText _text;
        private readonly ImmutableArray<SyntaxToken> _tokens;
        private int _position;

        public Parser(SourceText text)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter: text:>{text}<", Common.LOG_CATEGORY);

            var tokens = new List<SyntaxToken>();
            var lexer = new Lexer(text);
            SyntaxToken token;

            do
            {
                token = lexer.Lex();

                if (token.Kind != SyntaxKind.WhiteSpaceToken
                    && token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }

            } while (token.Kind != SyntaxKind.EndOfFileToken);

            _text = text;
            _tokens = tokens.ToImmutableArray();
            _diagnostics.AddRange(lexer.Diagnostics);

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        // NOTE(crhodes)
        // This lets you look ahead to see how to parse what you have already seen.

        private SyntaxToken Peek(int offset)
        {
            Int64 startTicks = Log.PARSER($"Enter offset: ({offset})", Common.LOG_CATEGORY);

            var index = _position + offset;

            if (index >= _tokens.Length)
            {
                Log.PARSER($"Exit: ({_tokens[_tokens.Length - 1].Kind})", Common.LOG_CATEGORY, startTicks);

                return _tokens[_tokens.Length - 1];
            }

            Log.PARSER($"Exit: ({_tokens[index].Kind})", Common.LOG_CATEGORY, startTicks);

            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var current = Current;
            _position++;

            Log.PARSER($"Exit: ({current.Kind})", Common.LOG_CATEGORY, startTicks);

            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            Int64 startTicks = Log.PARSER($"Enter kind: ({kind})", Common.LOG_CATEGORY);

            if (Current.Kind == kind)
            {
                // HACK(crhodes)
                // This is so we can print the kind

                var nextT = NextToken();

                Log.PARSER($"Exit {nextT.Kind}", Common.LOG_CATEGORY, startTicks);

                return nextT;

                //return NextToken();
            }

            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);

            // NOTE(crhodes)
            // This is super useful because ...

            Log.PARSER($"Exit new SyntaxToken()", Common.LOG_CATEGORY, startTicks);

            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var statement = ParseStatement();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);

            Log.PARSER($"Exit new CompilationUnitSyntax()", Common.LOG_CATEGORY, startTicks);

            return new CompilationUnitSyntax(statement, endOfFileToken);
        }

        private StatementSyntax ParseStatement()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            switch (Current.Kind)
            {
                case SyntaxKind.OpenBraceToken:
                    Log.PARSER($"Exit ParseBlockStatement()", Common.LOG_CATEGORY, startTicks);

                    return ParseBlockStatement();

                case SyntaxKind.LetKeyword:
                case SyntaxKind.VarKeyword:
                    Log.PARSER($"Exit ParseVariableDeclaration()", Common.LOG_CATEGORY, startTicks);

                    return ParseVariableDeclaration();

                case SyntaxKind.IfKeyword:
                    Log.PARSER($"Exit ParseIfStatement()", Common.LOG_CATEGORY, startTicks);

                    return ParseIfStatement();

                case SyntaxKind.WhileKeyword:
                    Log.PARSER($"Exit ParseWhileStatement()", Common.LOG_CATEGORY, startTicks);

                    return ParseWhileStatement();

                case SyntaxKind.ForKeyword:
                    Log.PARSER($"Exit ParseForStatement()", Common.LOG_CATEGORY, startTicks);

                    return ParseForStatement();

                default:
                    Log.PARSER($"Exit ParseExpressionStatement()", Common.LOG_CATEGORY, startTicks);

                    return ParseExpressionStatement();
            }
        }

        private BlockStatementSyntax ParseBlockStatement()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var statements = ImmutableArray.CreateBuilder<StatementSyntax>();

            var openBraceToken = MatchToken(SyntaxKind.OpenBraceToken);

            while (Current.Kind != SyntaxKind.EndOfFileToken
                && Current.Kind != SyntaxKind.CloseBraceToken)
            {
                var startToken = Current;

                var statement = ParseStatement();
                statements.Add(statement);

                // If ParseStatement() did not consume any tokens,
                // we need to skip the current token and continue
                // in order to avoid an infinite loop.
                // We do not need to report an error,
                // because we already tried to parse an expression statement
                // and reported one.

                if (Current == startToken)
                {
                    NextToken();
                }
            }

            var closeBraceToken = MatchToken(SyntaxKind.CloseBraceToken);

            Log.PARSER($"Exit new BlockStatementSyntax()", Common.LOG_CATEGORY, startTicks);

            return new BlockStatementSyntax(openBraceToken, statements.ToImmutable(), closeBraceToken);
        }

        private StatementSyntax ParseVariableDeclaration()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var expected = Current.Kind == SyntaxKind.LetKeyword ? SyntaxKind.LetKeyword : SyntaxKind.VarKeyword;

            var keyword = MatchToken(expected);
            var identifier = MatchToken(SyntaxKind.IdentifierToken);
            var equals = MatchToken(SyntaxKind.EqualsToken);
            var initializer = ParseExpression();

            Log.PARSER($"Exit ParseExpressionStatement()", Common.LOG_CATEGORY, startTicks);

            return new VariableDeclarationSyntax(keyword, identifier, equals, initializer);
        }

        private StatementSyntax ParseIfStatement()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var keyword = MatchToken(SyntaxKind.IfKeyword);
            var condition = ParseExpression();
            var statement = ParseStatement();
            var elseClause = ParseElseClause();

            Log.PARSER($"Exit ParseExpressionStatement()", Common.LOG_CATEGORY, startTicks);
            return new IfStatementSyntax(keyword, condition, statement, elseClause);
        }

        private ElseClauseSyntax ParseElseClause()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            if (Current.Kind != SyntaxKind.ElseKeyword)
            {
                return null;
            }

            var keyword = NextToken();
            var statement = ParseStatement();

            Log.PARSER($"Exit ParseExpressionStatement()", Common.LOG_CATEGORY, startTicks);

            return new ElseClauseSyntax(keyword, statement);
        }

        private StatementSyntax ParseWhileStatement()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var keyword = MatchToken(SyntaxKind.WhileKeyword);
            var condition = ParseExpression();
            var body = ParseStatement();

            Log.PARSER($"Exit ParseExpressionStatement()", Common.LOG_CATEGORY, startTicks);

            return new WhileStatementSyntax(keyword, condition, body);
        }

        private StatementSyntax ParseForStatement()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var keyword = MatchToken(SyntaxKind.ForKeyword);
            var identifier = MatchToken(SyntaxKind.IdentifierToken);
            var equalsToken = MatchToken(SyntaxKind.EqualsToken);
            var lowerBound = ParseExpression();
            var toKeyword = MatchToken(SyntaxKind.ToKeyword);
            var upperBound = ParseExpression();
            var body = ParseStatement();

            Log.PARSER($"Exit ParseExpressionStatement()", Common.LOG_CATEGORY, startTicks);

            return new ForStatementSyntax(keyword, identifier, equalsToken, lowerBound, toKeyword, upperBound, body);
        }

        private ExpressionStatementSyntax ParseExpressionStatement()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var expression = ParseExpression();

            Log.PARSER($"Exit ParseExpressionStatement()", Common.LOG_CATEGORY, startTicks);

            return new ExpressionStatementSyntax(expression);
        }

        // NOTE(crhodes)
        // This is cheating.  For now just have new method vs a more generalized approach

        private ExpressionSyntax ParseExpression()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            Log.PARSER($"Exit", Common.LOG_CATEGORY, startTicks);

            return ParseAssignmentExpression();
        }

        private ExpressionSyntax ParseAssignmentExpression()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            // HACK(crhodes)
            // This would work, but only at top level

            if (Peek(0).Kind == SyntaxKind.IdentifierToken
                && Peek(1).Kind == SyntaxKind.EqualsToken)
            {
                var identifierToken = NextToken();
                var operatorToken = NextToken();
                var right = ParseAssignmentExpression();
                return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
            }

            Log.PARSER($"Exit", Common.LOG_CATEGORY, startTicks);

            return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            Int64 startTicks = Log.PARSER($"Enter parentPrecedence: ({parentPrecedence})", Common.LOG_CATEGORY);

            ExpressionSyntax left;

            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();

            if (unaryOperatorPrecedence != 0
                && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseBinaryExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();

                if (precedence == 0
                    || precedence <= parentPrecedence)
                {
                    break;
                }

                var operatorToken = NextToken();
                var right = ParseBinaryExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            Log.PARSER($"Exit left:{left.Kind}", Common.LOG_CATEGORY, startTicks);

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            switch (Current.Kind)
            {
                case SyntaxKind.OpenParenthesisToken:
                    Log.PARSER($"Exit ParseParenthesizedExpression", Common.LOG_CATEGORY, startTicks);

                    return ParseParenthesizedExpression();

                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    Log.PARSER($"Exit ParseBooleanLiteral", Common.LOG_CATEGORY, startTicks);

                    return ParseBooleanLiteral();

                case SyntaxKind.NumberToken:
                    Log.PARSER($"Exit ParseNumberLiteral", Common.LOG_CATEGORY, startTicks);

                    return ParseNumberLiteral();

                case SyntaxKind.IdentifierToken:
                default:
                    Log.PARSER($"Exit ParseNameExpression", Common.LOG_CATEGORY, startTicks);

                    return ParseNameExpression();
            }
        }

        private ExpressionSyntax ParseParenthesizedExpression()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var left = MatchToken(SyntaxKind.OpenParenthesisToken);
            var expression = ParseExpression();
            var right = MatchToken(SyntaxKind.CloseParenthesisToken);

            Log.PARSER($"Exit ParenthesizedExpressionSyntax", Common.LOG_CATEGORY, startTicks);

            return new ParenthesizedExpressionSyntax(left, expression, right);
        }

        private ExpressionSyntax ParseBooleanLiteral()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var isTrue = Current.Kind == SyntaxKind.TrueKeyword;
            var keywordToken = isTrue ? MatchToken(SyntaxKind.TrueKeyword) : MatchToken(SyntaxKind.FalseKeyword);

            Log.PARSER($"Exit LiteralExpressionSyntax", Common.LOG_CATEGORY, startTicks);

            return new LiteralExpressionSyntax(keywordToken, isTrue);
        }

        private ExpressionSyntax ParseNumberLiteral()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var numberToken = MatchToken(SyntaxKind.NumberToken);

            Log.PARSER($"Exit LiteralExpressionSyntax", Common.LOG_CATEGORY, startTicks);

            return new LiteralExpressionSyntax(numberToken);
        }

        private ExpressionSyntax ParseNameExpression()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var identifierToken = MatchToken(SyntaxKind.IdentifierToken);

            Log.PARSER($"Exit NameExpressionSyntax", Common.LOG_CATEGORY, startTicks);

            return new NameExpressionSyntax(identifierToken);
        }
    }
}