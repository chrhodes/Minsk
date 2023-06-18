using System;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundAssignmentExpression : BoundExpression
    {
        public BoundAssignmentExpression(VariableSymbol variable, BoundExpression expression)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter variable:{variable.Name}, expression:{expression.Kind}", Common.LOG_CATEGORY);

            Variable = variable;
            Expression = expression;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public VariableSymbol Variable { get; }
        public BoundExpression Expression { get; }

        public override Type Type => Expression.Type;
    }
}
