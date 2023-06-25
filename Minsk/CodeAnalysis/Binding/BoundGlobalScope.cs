using System;
using System.Collections.Immutable;
using System.Linq;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundGlobalScope
    {
        public BoundGlobalScope(BoundGlobalScope previous,
            ImmutableArray<Diagnostic> diagnostics,
            ImmutableArray<VariableSymbol> variables,
            BoundStatement statement)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter previous:{previous?.Statement.Kind}, diagnostics:{diagnostics.Count()}, variables:{variables.Count()}, statement:{statement.Kind}", Common.LOG_CATEGORY);

            Previous = previous;
            Diagnostics = diagnostics;
            Variables = variables;
            Statement = statement;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public BoundGlobalScope Previous { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ImmutableArray<VariableSymbol> Variables { get; }
        public BoundStatement Statement { get; }
    }
}
