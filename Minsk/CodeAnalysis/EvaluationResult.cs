using System;
using System.Collections.Immutable;
using System.Linq;

using Minsk.CodeAnalysis.Syntax;

using VNC;

namespace Minsk.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(ImmutableArray<Diagnostic> diagnostics, object value)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter diagnostics:{diagnostics.Count()}, value:{value}", Common.LOG_CATEGORY);

            Diagnostics = diagnostics;
            Value = value;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public object Value { get; }
    }
}
