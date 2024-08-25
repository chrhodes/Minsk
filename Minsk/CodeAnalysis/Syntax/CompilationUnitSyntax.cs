using Newtonsoft.Json.Linq;
using System;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed class CompilationUnitSyntax : SyntaxNode
    {
        public CompilationUnitSyntax(StatementSyntax statement, SyntaxToken endOfFileToken)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter statement:{statement.Kind} endOfFileToken:{endOfFileToken}", Common.LOG_CATEGORY);

            Statement = statement;
            EndOfFileToken = endOfFileToken;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override SyntaxKind Kind => SyntaxKind.CompilationUnit;

        public StatementSyntax Statement { get; }
        public SyntaxToken EndOfFileToken { get; }
    }
}