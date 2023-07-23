using System;
using System.Collections.Immutable;

using VNC;

using static System.Net.Mime.MediaTypeNames;

namespace Minsk.CodeAnalysis.Text
{
    public sealed class SourceText
    {
        private string _propertyName;
        private readonly string _text;

        private SourceText(string text)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter text:{text}", Common.LOG_CATEGORY);

            _text = text;
            Lines = ParseLines(this, text);

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public ImmutableArray<TextLine> Lines { get; }

        public char this[int index] => _text[index];

        public int Length => _text.Length;

        // NOTE(crhodes)
        // Do a binary search to make this a bit more efficient
        // versus a linear progression to find the line containing the position
        public int GetLineIndex(int position)
        {
            Int64 startTicks = Log.TEXT_LOW($"Enter position:{position}", Common.LOG_CATEGORY);

            var lower = 0;
            var upper = Lines.Length - 1;

            while (lower <= upper)
            {
                var index = lower + (upper - lower) / 2;
                var start = Lines[index].Start;

                if (position == start)
                {
                    Log.TEXT_LOW($"Exit {index}", Common.LOG_CATEGORY, startTicks);

                    return index;
                }

                if (start > position)
                {
                    // Discard upper window
                    upper = index - 1;
                }
                else
                {
                    // Discard lower window
                    lower = index + 1;
                }
            }

            // We found the line

            Log.TEXT_LOW($"Exit {lower - 1}", Common.LOG_CATEGORY, startTicks);

            return lower - 1;
        }

        private static ImmutableArray<TextLine> ParseLines(SourceText sourceText, string text)
        {
            Int64 startTicks = Log.TEXT($"Enter sourceText:{sourceText}, text:{text}", Common.LOG_CATEGORY);

            var result = ImmutableArray.CreateBuilder<TextLine>();
            var position = 0;
            var lineStart = 0;

            while (position < text.Length)
            {
                var lineBreakWidth = GetLineBreakWidth(text, position);

                if (lineBreakWidth == 0)
                {
                    position++;
                }
                else
                {
                    AddLine(result, sourceText, position, lineStart, lineBreakWidth);

                    position += lineBreakWidth;
                    lineStart = position;
                }
            }

            if (position >= lineStart)
            {
                AddLine(result, sourceText, position, lineStart, 0);
            }

            Log.TEXT($"Exit", Common.LOG_CATEGORY, startTicks);

            return result.ToImmutable();
        }

        private static void AddLine(ImmutableArray<TextLine>.Builder result, SourceText sourceText, int position, int lineStart, int lineBreakWidth)
        {
            Int64 startTicks = Log.TEXT($"Enter result:{result}, sourceText:{sourceText}, position:{position}, lineStart:{lineStart}, lineBreakWidth:{lineBreakWidth}", Common.LOG_CATEGORY);

            var lineLength = position - lineStart;
            var lineLengthIncludingLineBreak = lineLength + lineBreakWidth;
            var line = new TextLine(sourceText, lineStart, lineLength, lineLengthIncludingLineBreak);

            result.Add(line);

            Log.TEXT($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        private static int GetLineBreakWidth(string text, int position)
        {
            Int64 startTicks = Log.TEXT_LOW($"Enter text:{text}, position:{position}", Common.LOG_CATEGORY);

            var c = text[position];
            var l = position + 1 >= text.Length ? '\0' : text[position + 1];

            if (c == '\r' && l == '\n')
            {
                Log.TEXT_LOW($"Exit 2", Common.LOG_CATEGORY, startTicks);

                return 2;
            }

            if (c == '\r' || c == '\n')
            {
                Log.TEXT_LOW($"Exit 1", Common.LOG_CATEGORY, startTicks);

                return 1;
            }

            Log.TEXT_LOW($"Exit 0", Common.LOG_CATEGORY, startTicks);

            return 0;
        }

        public static SourceText From(string text)
        {
            Int64 startTicks = Log.TEXT($"Enter text:{text}", Common.LOG_CATEGORY);

            Log.TEXT($"Exit new SourceText()", Common.LOG_CATEGORY, startTicks);

            return new SourceText(text);
        }

        public override string ToString() => _text;

        public string ToString(int start, int length) => _text.Substring(start, length);

        public string ToString(TextSpan span) => ToString(span.Start, span.Length);

    }
}
