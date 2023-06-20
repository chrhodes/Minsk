using System;

using VNC;

namespace Minsk.CodeAnalysis.Text
{
    public sealed class TextLine
    {
        public TextLine(SourceText text, int start, int length, int lengthIncludingLineBreak)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter text:{text}, start:{start}, length:{length} lengthIncludingLineBreak{lengthIncludingLineBreak}", Common.LOG_CATEGORY);

            Text = text;
            Start = start;
            Length = length;
            LengthIncludingLineBreak = lengthIncludingLineBreak;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public SourceText Text { get; }
        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;
        public int LengthIncludingLineBreak { get; }

        public TextSpan Span => new TextSpan(Start, Length);
        public TextSpan SpanIncludingLineBreak => new TextSpan(Start, LengthIncludingLineBreak);

        public override string ToString() => Text.ToString(Span);
    }
}
