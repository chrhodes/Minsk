using Newtonsoft.Json.Linq;
using System;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed class ElseClauseSyntax : SyntaxNode
    {
        public ElseClauseSyntax(SyntaxToken elseKeyword, StatementSyntax elseStatement)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter elseKeyword:{elseKeyword.Kind} elseStatement:{elseStatement}", Common.LOG_CATEGORY);

            ElseKeyword = elseKeyword;
            ElseStatement = elseStatement;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override SyntaxKind Kind => SyntaxKind.ElseClause;

        public SyntaxToken ElseKeyword { get; }
        public StatementSyntax ElseStatement { get; }
    }
}