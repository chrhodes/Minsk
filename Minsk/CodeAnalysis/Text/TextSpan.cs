using System;

using VNC;

namespace Minsk.CodeAnalysis.Text
{
    public struct TextSpan
    {
        public TextSpan(int start, int length)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter start:{start}, length:{length}", Common.LOG_CATEGORY);

            Start = start;
            Length = length;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;

        public static TextSpan FromBounds(int start, int end)
        {
            Int64 startTicks = Log.TEXT($"Enter start:{start}, end:{end}", Common.LOG_CATEGORY);

            var length = end - start;

            Log.TEXT($"Exit", Common.LOG_CATEGORY, startTicks);

            return new TextSpan(start, length);
        }

        public override string ToString() => $"{Start}..{End}";
    }
}