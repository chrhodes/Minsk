using System;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed class WhileStatementSyntax : StatementSyntax
    {
        public WhileStatementSyntax(SyntaxToken whileKeyword, ExpressionSyntax condition, StatementSyntax body)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter whileKeyword:{whileKeyword.Kind}, condition:{condition.Kind}, body:{body.Kind}", Common.LOG_CATEGORY);

            WhileKeyword = whileKeyword;
            Condition = condition;
            Body = body;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override SyntaxKind Kind => SyntaxKind.WhileStatement;

        public SyntaxToken WhileKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public StatementSyntax Body { get; }
    }
}
