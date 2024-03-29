using System;
using System.Collections.Generic;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed class AssignmentExpressionSyntax : ExpressionSyntax
    {
        public AssignmentExpressionSyntax(SyntaxToken identifierToken, SyntaxToken equalsToken, ExpressionSyntax expression)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter identifierToken:{identifierToken.Kind} identifierToken:{equalsToken.Kind} expression:{expression.Kind}", Common.LOG_CATEGORY);

            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Expression = expression;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override SyntaxKind Kind => SyntaxKind.AssignmentExpression;

        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntax Expression { get; }

        // NOTE(crhodes)
        // This got replaced with non-abstract implementation in SyntaxNode

        //public override IEnumerable<SyntaxNode> GetChildren()
        //{
        //    yield return IdentifierToken;
        //    yield return EqualsToken;
        //    yield return Expression;
        //}
    }
}