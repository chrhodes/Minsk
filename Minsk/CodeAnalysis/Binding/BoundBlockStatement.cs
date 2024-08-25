using System;
using System.Collections.Immutable;
using System.Linq;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundBlockStatement : BoundStatement
    {
        public BoundBlockStatement(ImmutableArray<BoundStatement> statements)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter statements:{statements.Count()}", Common.LOG_CATEGORY);

            Statements = statements;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public ImmutableArray<BoundStatement> Statements { get; }

        public override BoundNodeKind Kind => BoundNodeKind.BlockStatement;
    }
}
