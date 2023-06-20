using System;

using Minsk.CodeAnalysis.Syntax;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator
    {
        public BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType)
            : this(syntaxKind, kind, operandType, operandType)
        {
            //Int64 startTicks = Log.CONSTRUCTOR($"Enter syntaxKind:{syntaxKind} kind:{kind} operandType:{operandType}", Common.LOG_CATEGORY);

            //Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter syntaxKind:{syntaxKind} kind:{kind} operandType:{operandType} resultType:{resultType}", Common.LOG_CATEGORY);

            SyntaxKind = syntaxKind;
            Kind = kind;
            OperandType = operandType;
            Type = resultType;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type Type { get; }

        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(Boolean)),
            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int))
        };

        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type operandType)
        {
            Int64 startTicks = Log.BINDER($"Enter syntaxKind:{syntaxKind} operandType:{operandType}", Common.LOG_CATEGORY);

            foreach (var op in _operators)
            {
                if (op.SyntaxKind == syntaxKind
                    && op.OperandType == operandType)
                {
                    Log.BINDER($"Exit {op.Kind}", Common.LOG_CATEGORY, startTicks);

                    return op;
                }
            }

            Log.BINDER($"Exit null", Common.LOG_CATEGORY, startTicks);

            return null;
        }
    }
}
