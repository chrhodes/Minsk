using System;
using System.Collections.Generic;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    //NOTE(crhodes)
    // Parser assembles the tokens into sentences
    // Parser produces Syntax Trees (sentences)
    // from the Syntax Tokens (words)

    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;

        private List<string> _diagnostics = new List<string>();

        private int _position;

        public Parser(string text)
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

            _tokens = tokens.ToArray();

            _diagnostics.AddRange(lexer.Diagnostics);

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private SyntaxToken Current => Peek(0);

        public SyntaxTree Parse()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var expression = ParseExpression();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);

            Log.PARSER($"Exit new SyntaxTree", Common.LOG_CATEGORY, startTicks);

            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }

        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            Int64 startTicks = Log.PARSER($"Enter kind:{kind}", Common.LOG_CATEGORY);

            if (Current.Kind == kind)
            {
                // HACK(crhodes)
                // This is so we can print the kind

                var nextT = NextToken();

                Log.PARSER($"Exit {nextT.Kind}", Common.LOG_CATEGORY, startTicks);

                return nextT;

                //return NextToken();
            }

            _diagnostics.Add($"ERROR: Unexpected token:{Current.Kind}, expected:{kind}");

            // NOTE(crhodes)
            // This is super useful because ...

            Log.PARSER($"Exit: ERROR: Unexpected token:{Current.Kind}, expected:{kind}", Common.LOG_CATEGORY, startTicks);

            return new SyntaxToken(kind, Current.Position, null, null);
        }

        private SyntaxToken NextToken()
        {
            Int64 startTicks = Log.PARSER($"Enter", Common.LOG_CATEGORY);

            var current = Current;
            _position++;

            Log.PARSER($"Exit {current.Kind}", Common.LOG_CATEGORY, startTicks);

            return current;
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            Int64 startTicks = Log.PARSER($"Enter parentPrecedence:{parentPrecedence}", Common.LOG_CATEGORY);

            ExpressionSyntax left;

            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();

            if (unaryOperatorPrecedence != 0
                && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseExpression(unaryOperatorPrecedence);

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
                var right = ParseExpression(precedence);
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
                    var left = NextToken();
                    var expression = ParseExpression();
                    var right = MatchToken(SyntaxKind.CloseParenthesisToken);

                    Log.PARSER($"Exit new ParenthesizedExpressionSyntax({left.Kind},{expression},{right.Kind})", Common.LOG_CATEGORY, startTicks);

                    return new ParenthesizedExpressionSyntax(left, expression, right);

                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    var keywordToken = NextToken();

                    var value = keywordToken.Kind == SyntaxKind.TrueKeyword;

                    Log.PARSER($"Exit new LiteralExpressionSyntax({keywordToken.Kind}, {value})", Common.LOG_CATEGORY, startTicks);

                    return new LiteralExpressionSyntax(keywordToken, value);

                default:
                    var numberToken = MatchToken(SyntaxKind.NumberToken);

                    Log.PARSER($"Exit new LiteralExpressionSyntax({numberToken.Kind})", Common.LOG_CATEGORY, startTicks);

                    return new LiteralExpressionSyntax(numberToken);
            }

        }

        // NOTE(crhodes)
        // This lets you look ahead to see how to parse what you have already seen.

        private SyntaxToken Peek(int offset)
        {
            Int64 startTicks = Log.PARSER($"Enter offset:{offset}", Common.LOG_CATEGORY);

            var index = _position + offset;

            if (index >= _tokens.Length)
            {
                Log.PARSER($"Exit {(_tokens[_tokens.Length - 1]).Kind}", Common.LOG_CATEGORY, startTicks);

                return _tokens[_tokens.Length - 1];
            }

            Log.PARSER($"Exit {(_tokens[index]).Kind}", Common.LOG_CATEGORY, startTicks);

            return _tokens[index];
        }
    }
}