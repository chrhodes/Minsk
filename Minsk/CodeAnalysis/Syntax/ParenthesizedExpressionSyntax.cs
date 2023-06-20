using System;
using System.Collections.Generic;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    internal sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public ParenthesizedExpressionSyntax(SyntaxToken openParenthesisToken, ExpressionSyntax expression, SyntaxToken closeParenthesisToken)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter openParenthesisToken:{openParenthesisToken.Kind} expression: {expression.Kind} closeParenthesisToken: {closeParenthesisToken.Kind}", Common.LOG_CATEGORY);

            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;

        public SyntaxToken OpenParenthesisToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseParenthesisToken { get; }
        //public SyntaxToken OpenParenthesisToken { get; }

        //public override IEnumerable<SyntaxNode> GetChildren()
        //{
        //    yield return OpenParenthesisToken;
        //    yield return Expression;
        //    yield return CloseParenthesisToken;
        //}
    }
}