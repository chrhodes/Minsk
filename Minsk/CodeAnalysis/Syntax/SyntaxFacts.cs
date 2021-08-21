using System;

namespace Minsk.CodeAnalysis.Syntax
{

    public static class SyntaxFacts
    {
        // NOTE(crhodes)
        // There is interaction between Unary and Binary Operators
        // 3 > 2, 1

        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 6;

                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 5;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;

                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.BangEqualsToken:
                    return 3;

                case SyntaxKind.AmpersandAmpersandToken:
                    return 2;

                case SyntaxKind.PipePipeToken:
                    return 1;

                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeyWordKind(string text)
        {
            switch (text)
            {
                case "true":
                    return SyntaxKind.TrueKeyWord;

                case "false":
                    return SyntaxKind.FalseKeyWord;

                default:
                    return SyntaxKind.IdentifierToken;
            }
        }

        public static string GetText(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.BangToken:
                    return "!";

                case SyntaxKind.EqualsToken:
                    return "=";

                case SyntaxKind.AmpersandAmpersandToken:
                    return "&&";

                case SyntaxKind.PipePipeToken:
                    return "||";

                case SyntaxKind.EqualsEqualsToken:
                    return "==";

                case SyntaxKind.BangEqualsToken:
                    return "!=";

                case SyntaxKind.PlusToken:
                    return "+";

                case SyntaxKind.MinusToken:
                    return "-";

               case SyntaxKind.StarToken:
                    return "*";

               case SyntaxKind.SlashToken:
                    return "/";

               case SyntaxKind.OpenParenthesisToken:
                    return "(";

               case SyntaxKind.CloseParenthesisToken:
                    return ")";

                case SyntaxKind.FalseKeyWord:
                    return "false";

                case SyntaxKind.TrueKeyWord:
                    return "true";

                default:
                    return null;
            }

        }
    }
}
