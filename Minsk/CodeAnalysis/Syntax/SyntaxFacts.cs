using System;
using System.Collections.Generic;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    public static class SyntaxFacts
    {
        // NOTE(crhodes)
        // There is interaction between Unary and Binary Operators
        //
        // -1 * 3
        //
        //     -
        //     |
        //     *
        //    / \
        //   1   3

        // or

        //     *
        //    / \
        //   -   3
        //   |
        //   1

        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            Int64 startTicks = Log.SYNTAX($"Enter kind:{kind}", Common.LOG_CATEGORY);

            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    Log.SYNTAX($"Exit 6", Common.LOG_CATEGORY, startTicks);
                    return 6;

                default:
                    Log.SYNTAX($"Exit 0", Common.LOG_CATEGORY, startTicks);
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            Int64 startTicks = Log.SYNTAX($"Enter kind:{kind}", Common.LOG_CATEGORY);

            switch (kind)
            {
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    Log.SYNTAX($"Exit 5", Common.LOG_CATEGORY, startTicks);
                    return 5;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    Log.SYNTAX($"Exit 4", Common.LOG_CATEGORY, startTicks);
                    return 4;

                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.BangEqualsToken:
                case SyntaxKind.LessToken:
                case SyntaxKind.LessOrEqualsToken:
                case SyntaxKind.GreaterToken:
                case SyntaxKind.GreaterOrEqualsToken:
                    Log.SYNTAX($"Exit 3", Common.LOG_CATEGORY, startTicks);
                    return 3;

                case SyntaxKind.AmpersandAmpersandToken:
                    Log.SYNTAX($"Exit 2", Common.LOG_CATEGORY, startTicks);
                    return 2;

                case SyntaxKind.PipePipeToken:
                    Log.SYNTAX($"Exit 1", Common.LOG_CATEGORY, startTicks);
                    return 1;

                default:
                    Log.SYNTAX($"Exit 0", Common.LOG_CATEGORY, startTicks);
                    return 0;
            }
        }

        public static SyntaxKind GetKeyWordKind(string text)
        {
            Int64 startTicks = Log.SYNTAX($"Enter text:{text}", Common.LOG_CATEGORY);

            switch (text)
            {
                case "else":
                    Log.SYNTAX($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.ElseKeyword;

                case "false":
                    Log.SYNTAX($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.FalseKeyword;

                case "for":
                    Log.SYNTAX($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.ForKeyword;

                case "if":
                    Log.SYNTAX($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.IfKeyword;

                case "let":
                    Log.SYNTAX($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.LetKeyword;

                case "to":
                    Log.SYNTAX($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.ToKeyword;

                case "true":
                    Log.SYNTAX($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.TrueKeyword;

                case "var":
                    Log.SYNTAX($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.VarKeyword;

                case "while":
                    Log.SYNTAX($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.WhileKeyword;

                default:
                    Log.SYNTAX($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.IdentifierToken;
            }
        }

        public static IEnumerable<SyntaxKind> GetBinaryOperatorKinds()
        {
            var kinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));

            foreach (var kind in kinds)
            {
                if (GetBinaryOperatorPrecedence(kind) > 0)
                {
                    yield return kind;
                }
            }
        }

        public static IEnumerable<SyntaxKind> GetUnaryOperatorKinds()
        {
            var kinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));

            foreach (var kind in kinds)
            {
                if (GetUnaryOperatorPrecedence(kind) > 0)
                {
                    yield return kind;
                }
            }
        }

        public static string GetText(SyntaxKind kind)
        {
            Int64 startTicks = Log.SYNTAX($"Enter kind:{kind} Exit <text>", Common.LOG_CATEGORY);

            switch (kind)
            {
                case SyntaxKind.PlusToken:
                    return "+";

                case SyntaxKind.MinusToken:
                    return "-";

                case SyntaxKind.StarToken:
                    return "*";

                case SyntaxKind.SlashToken:
                    return "/";

                case SyntaxKind.BangToken:
                    return "!";

                case SyntaxKind.EqualsToken:
                    return "=";

                case SyntaxKind.LessToken:
                    return "<";

                case SyntaxKind.LessOrEqualsToken:
                    return "<=";

                case SyntaxKind.GreaterToken:
                    return ">";

                case SyntaxKind.GreaterOrEqualsToken:
                    return ">=";

                case SyntaxKind.AmpersandAmpersandToken:
                    return "&&";

                case SyntaxKind.PipePipeToken:
                    return "||";

                case SyntaxKind.EqualsEqualsToken:
                    return "==";

                case SyntaxKind.BangEqualsToken:
                    return "!=";

               case SyntaxKind.OpenParenthesisToken:
                    return "(";

               case SyntaxKind.CloseParenthesisToken:
                    return ")";

                case SyntaxKind.OpenBraceToken:
                    return "{";

                case SyntaxKind.CloseBraceToken:
                    return "}";

                case SyntaxKind.ElseKeyword:
                    return "else";

                case SyntaxKind.FalseKeyword:
                    return "false";

                case SyntaxKind.ForKeyword:
                    return "for";

                case SyntaxKind.IfKeyword:
                    return "if";

                case SyntaxKind.LetKeyword:
                    return "let";

                case SyntaxKind.ToKeyword:
                    return "to";

                case SyntaxKind.TrueKeyword:
                    return "true";

                case SyntaxKind.VarKeyword:
                    return "var";

                case SyntaxKind.WhileKeyword:
                    return "while";

                default:
                    return null;
            }
        }
    }
}
