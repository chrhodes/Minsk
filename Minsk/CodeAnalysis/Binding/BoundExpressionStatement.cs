using System;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundExpressionStatement : BoundStatement
    {
        public BoundExpressionStatement(BoundExpression expression)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter expression:{expression.Kind}", Common.LOG_CATEGORY);

            Expression = expression;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override BoundNodeKind Kind => BoundNodeKind.ExpressionStatement;

        public BoundExpression Expression { get; }
    }
}
