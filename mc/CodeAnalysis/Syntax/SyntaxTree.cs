using System;
using System.Collections.Generic;
using System.Linq;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    // NOTE(crhodes)
    // SyntaxTree represent entire collection of nodes.

    public sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter: diagnostics: {diagnostics} root:{root} endOfFileToken:{endOfFileToken}", Common.LOG_CATEGORY);

            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntaxTree Parse(string text)
        {
            Int64 startTicks = Log.SYNTAX($"Enter: text: {text}", Common.LOG_CATEGORY);

            var parser = new Parser(text);

            Log.SYNTAX($"Exit", Common.LOG_CATEGORY, startTicks);

            return parser.Parse();
        }
    }
}