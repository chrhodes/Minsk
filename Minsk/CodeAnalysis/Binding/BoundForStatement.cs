using System;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundForStatement : BoundStatement
    {
        public BoundForStatement(VariableSymbol variable, BoundExpression lowerBound, BoundExpression upperBound, BoundStatement body)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter variable:{variable.Name} lowerBound:{lowerBound.Kind} upperBound:{upperBound.Kind} body:{body.Kind}", Common.LOG_CATEGORY);

            Variable = variable;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Body = body;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override BoundNodeKind Kind => BoundNodeKind.ForStatement;

        public VariableSymbol Variable { get; }
        public BoundExpression LowerBound { get; }
        public BoundExpression UpperBound { get; }
        public BoundStatement Body { get; }
    }
}
