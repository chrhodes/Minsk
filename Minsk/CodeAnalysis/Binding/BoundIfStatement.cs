using System;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundIfStatement : BoundStatement
    {
        public BoundIfStatement(BoundExpression condition, BoundStatement thenStatement, BoundStatement elseStatement)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter condition:{condition.Kind} thenStatement:{thenStatement.Kind} elseStatement:{elseStatement.Kind}", Common.LOG_CATEGORY);


            Condition = condition;
            ThenStatement = thenStatement;
            ElseStatement = elseStatement;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override BoundNodeKind Kind => BoundNodeKind.IfStatement;

        public BoundExpression Condition { get; }
        public BoundStatement ThenStatement { get; }
        public BoundStatement ElseStatement { get; }
    }

    internal sealed class BoundWhileStatement : BoundStatement
    {
        public BoundWhileStatement(BoundExpression condition, BoundStatement body)
        {
            Condition = condition;
            Body = body;
        }

        public override BoundNodeKind Kind => BoundNodeKind.WhileStatement;

        public BoundExpression Condition { get; }
        public BoundStatement Body { get; }
    }
}
