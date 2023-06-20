using Newtonsoft.Json.Linq;
using System;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed class ExpressionStatementSyntax : StatementSyntax
    {
        // In C, C#, etc would also take a ; Syntax 
        public ExpressionStatementSyntax(ExpressionSyntax expression)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter expression:{expression.Kind}", Common.LOG_CATEGORY);

            Expression = expression;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;

        public ExpressionSyntax Expression { get; }
    }
}