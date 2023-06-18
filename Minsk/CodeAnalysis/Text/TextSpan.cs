using System;

using VNC;

namespace Minsk.CodeAnalysis.Text
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

        public static TextSpan FromBounds(int start, int end)
        {
            var length = end - start;

            return new TextSpan(start, length);
        }

        public override string ToString() => $"{Start}..{End}";
    }
}