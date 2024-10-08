using System;
using System.Collections.Generic;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter left:{left.Kind} operatorToken:{operatorToken.Kind} right:{right.Kind}", Common.LOG_CATEGORY);

            Left = left;
            OperatorToken = operatorToken;
            Right = right;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        public ExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Right { get; }

<<<<<<< HEAD
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            Int64 startTicks = Log.SYNTAX($"Enter/Exit", Common.LOG_CATEGORY);

            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
=======
        // NOTE(crhodes)
        // This got replaced with non-abstract implementation in SyntaxNode

        //public override IEnumerable<SyntaxNode> GetChildren()
        //{
        //    yield return Left;
        //    yield return OperatorToken;
        //    yield return Right;
        //}
>>>>>>> 75dd8c82acb0768ca4610d1fbbb445b28a8fd144
    }
}