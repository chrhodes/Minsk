using System;

using Minsk.CodeAnalysis.Text;

using VNC;

namespace Minsk.CodeAnalysis
{
    public sealed class Diagnostic
    {
        public Diagnostic(TextSpan span, string message)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter span:{span} operand:{message}", Common.LOG_CATEGORY);

            Span = span;
            Message = message;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public TextSpan Span { get; }
        public string Message { get; }

        public override string ToString() => Message;

    }
}