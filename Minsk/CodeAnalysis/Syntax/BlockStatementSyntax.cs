using System;
using System.Collections.Immutable;
using System.Linq;

using Newtonsoft.Json.Linq;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed class BlockStatementSyntax : StatementSyntax
    {

        public BlockStatementSyntax(SyntaxToken openBraceToken, ImmutableArray<StatementSyntax> statements, SyntaxToken closeBraceToken)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter openBraceToken:{openBraceToken.Kind} statements:{statements.Count()}, closeBraceToken:{closeBraceToken}", Common.LOG_CATEGORY);

            OpenBraceToken = openBraceToken;
            Statements = statements;
            CloseBraceToken = closeBraceToken;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override SyntaxKind Kind => SyntaxKind.BlockStatement;

        public SyntaxToken OpenBraceToken { get; }
        public ImmutableArray<StatementSyntax> Statements { get; }
        public SyntaxToken CloseBraceToken { get; }
    }
}