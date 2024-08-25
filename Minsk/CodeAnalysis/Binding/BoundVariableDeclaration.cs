using System.Linq.Expressions;
using System;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundVariableDeclaration : BoundStatement
    {
        public BoundVariableDeclaration(VariableSymbol variable, BoundExpression initializer)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter variable:{variable.Name}, initializer:{initializer.Kind}", Common.LOG_CATEGORY);

            Variable = variable;
            Initializer = initializer;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override BoundNodeKind Kind => BoundNodeKind.VariableDeclaration;

        public VariableSymbol Variable { get; }
        public BoundExpression Initializer { get; }
    }
}
