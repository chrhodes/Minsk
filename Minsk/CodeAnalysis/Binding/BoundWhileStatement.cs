using System;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundWhileStatement : BoundStatement
    {
        public BoundWhileStatement(BoundExpression condition, BoundStatement body)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter condition:{condition.Kind} body:{body.Kind}", Common.LOG_CATEGORY);

            Condition = condition;
            Body = body;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override BoundNodeKind Kind => BoundNodeKind.WhileStatement;

        public BoundExpression Condition { get; }
        public BoundStatement Body { get; }
    }
}
