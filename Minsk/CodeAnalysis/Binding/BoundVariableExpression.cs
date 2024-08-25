using System;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundVariableExpression : BoundExpression
    {
        public BoundVariableExpression(VariableSymbol variable)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter varable{variable.Name}", Common.LOG_CATEGORY);

            Variable = variable;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;

        public override Type Type => Variable.Type;
        public VariableSymbol Variable { get; }
    }
}
