using System;

using VNC;

namespace Minsk.CodeAnalysis
{
    public struct TextSpan
    {
        public TextSpan(int start, int length)
        {
            Int64 startTicks = Log.Trace($"Enter", Common.LOG_CATEGORY);

            Start = start;
            Length = length;

            Log.Trace($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;
    }
}