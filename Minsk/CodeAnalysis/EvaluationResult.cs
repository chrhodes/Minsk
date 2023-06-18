using System;
using System.Collections.Generic;
using System.Linq;

using VNC;

namespace Minsk.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(IEnumerable<Diagnostic> diagnostics, object value)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter diagnostics:{diagnostics.Count()}, value:{value}", Common.LOG_CATEGORY);

            Diagnostics = diagnostics.ToArray();
            Value = value;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public IReadOnlyList<Diagnostic> Diagnostics { get; }
        public object Value { get; }
    }
}
