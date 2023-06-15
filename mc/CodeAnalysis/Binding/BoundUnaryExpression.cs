using System;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperator op, BoundExpression operand)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter: op:{op.Kind} operand:{operand.Kind}", Common.LOG_CATEGORY);

            Op= op;
            Operand = operand;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public BoundUnaryOperator Op { get; }
        public BoundExpression Operand { get; }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override Type Type => Op.Type;
    }
}
