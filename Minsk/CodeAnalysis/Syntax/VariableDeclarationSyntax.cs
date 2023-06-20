using Newtonsoft.Json.Linq;
using System;

using VNC;

namespace Minsk.CodeAnalysis.Syntax
{

        // var x = 10
        // let x = 10
        public sealed class VariableDeclarationSyntax : StatementSyntax
        {
            public VariableDeclarationSyntax(SyntaxToken keyword, SyntaxToken identifier, SyntaxToken equalsToken, ExpressionSyntax initializer)
            {
                Int64 startTicks = Log.CONSTRUCTOR($"Enter keyword:{keyword.Kind}, identifier:{identifier.Kind}, equalsToken:{equalsToken.Kind}, initializer:{initializer.Kind}", Common.LOG_CATEGORY);

                Keyword = keyword;
                Identifier = identifier;
                EqualsToken = equalsToken;
                Initializer = initializer;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

            public override SyntaxKind Kind => SyntaxKind.VariableDeclaration;

            public SyntaxToken Keyword { get; }
            public SyntaxToken Identifier { get; }
            public SyntaxToken EqualsToken { get; }
            public ExpressionSyntax Initializer { get; }
        }

}