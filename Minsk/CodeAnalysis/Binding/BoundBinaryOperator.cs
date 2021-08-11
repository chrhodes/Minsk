using System;

using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {
        public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type)
            : this(syntaxKind, kind, type, type, type)
        {
        }

        public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type operandType, Type resultType)
            : this(syntaxKind, kind, operandType, operandType, resultType)
        {
        }

        public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            Type = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }

        public Type LeftType { get; }
        public Type RightType { get; }

        public Type Type { get; }

        private static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(int)),

            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equals, typeof(int), typeof(Boolean)),
            new BoundBinaryOperator(SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, typeof(int), typeof(Boolean)),

            new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(Boolean)),
            new BoundBinaryOperator(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(Boolean)),
            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equals, typeof(Boolean)),
            new BoundBinaryOperator(SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, typeof(Boolean)),
        };

        public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
        {
            foreach (var op in _operators)
            {
                if (op.SyntaxKind == syntaxKind
                    && op.LeftType == leftType
                    && op.RightType == rightType)
                {
                    return op;
                }
            }

            return null;
        }
    }
}