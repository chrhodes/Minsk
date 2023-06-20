using Newtonsoft.Json.Linq;
using System;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{

    // for i = 0 to 9
    // {
    //
    // }
    public sealed class ForStatementSyntax : StatementSyntax
    {
        public ForStatementSyntax(SyntaxToken keyword, SyntaxToken identifier, SyntaxToken equalsToken,
            ExpressionSyntax lowerBound, SyntaxToken toKeyword, ExpressionSyntax upperBound, StatementSyntax body)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter keyword:{keyword.Kind}, identifier:{identifier.Kind}, equalsToken:{equalsToken.Kind}, lowerBound:{lowerBound.Kind}, toKeyword:{toKeyword.Kind}, upperBound:{upperBound.Kind}, body:{body.Kind}", Common.LOG_CATEGORY);

            Keyword = keyword;
            Identifier = identifier;
            EqualsToken = equalsToken;
            LowerBound = lowerBound;
            ToKeyword = toKeyword;
            UpperBound = upperBound;
            Body = body;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public override SyntaxKind Kind => SyntaxKind.ForStatement;

        public SyntaxToken Keyword { get; }
        public SyntaxToken Identifier { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntax LowerBound { get; }
        public SyntaxToken ToKeyword { get; }
        public ExpressionSyntax UpperBound { get; }
        public StatementSyntax Body { get; }
    }
}