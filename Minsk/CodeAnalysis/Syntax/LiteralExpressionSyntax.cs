using System;
using System.Collections.Generic;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed class LiteralExpressionSyntax : ExpressionSyntax
    {
        public LiteralExpressionSyntax(SyntaxToken literalToken)
            : this(literalToken, literalToken.Value)
        {
            //Int64 startTicks = Log.CONSTRUCTOR($"Enter literalToken:{literalToken.Kind}", Common.LOG_CATEGORY);

            //Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public LiteralExpressionSyntax(SyntaxToken literalToken, object value)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter literalToken:{literalToken.Kind} value:{value}", Common.LOG_CATEGORY);

            LiteralToken = literalToken;
            Value = value;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

        public SyntaxToken LiteralToken { get; }
        public object Value { get; }

        // NOTE(crhodes)
        // This got replaced with non-abstract implementation in SyntaxNode

        //public override IEnumerable<SyntaxNode> GetChildren()
        //{
        //    yield return LiteralToken;
        //}
    }
}
