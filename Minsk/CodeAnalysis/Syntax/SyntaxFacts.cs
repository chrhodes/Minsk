using System;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{

    internal static class SyntaxFacts
    {
        // NOTE(crhodes)
        // There is interaction between Unary and Binary Operators
        // 3 > 2, 1

        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter kind:{kind}", Common.LOG_CATEGORY);

            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
                    return 6;

                default:
                    Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter kind:{kind}", Common.LOG_CATEGORY);

            switch (kind)
            {
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
                    return 5;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
                    return 4;

                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.BangEqualsToken:
                    Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
                    return 3;

                case SyntaxKind.AmpersandAmpersandToken:
                    Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
                    return 2;

                case SyntaxKind.PipePipeToken:
                    Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
                    return 1;

                default:
                    Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
                    return 0;
            }
        }

        public static SyntaxKind GetKeyWordKind(string text)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter text:{text}", Common.LOG_CATEGORY);

            switch (text)
            {
                case "true":
                    Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.TrueKeyword;

                case "false":
                    Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.FalseKeyword;

                default:
                    Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
                    return SyntaxKind.IdentifierToken;
            }
        }
    }
}
