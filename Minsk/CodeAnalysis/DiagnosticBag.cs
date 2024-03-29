using System;
using System.Collections;
using System.Collections.Generic;

using Minsk.CodeAnalysis.Syntax;
using Minsk.CodeAnalysis.Text;

using VNC;

namespace Minsk.CodeAnalysis
{
    internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void AddRange(DiagnosticBag diagnostics)
        {
            Int64 startTicks = Log.DIAGNOSTIC($"Enter/Exit", Common.LOG_CATEGORY);

            _diagnostics.AddRange(diagnostics._diagnostics);
        }

        private void Report(TextSpan span, string message)
        {
            Int64 startTicks = Log.DIAGNOSTIC($"Enter/Exit", Common.LOG_CATEGORY);

            var diagnostic = new Diagnostic(span, message);
            _diagnostics.Add(diagnostic);
        }

        public void ReportInvalidNumber(TextSpan span, string text, Type type)
        {
            Int64 startTicks = Log.DIAGNOSTIC($"Enter/Exit", Common.LOG_CATEGORY);

            var message = $"The number {text} isn't a valid {type}.";

            Report(span, message);
        }

        public void ReportBadCharacter(int position, char character)
        {
            Int64 startTicks = Log.DIAGNOSTIC($"Enter/Exit", Common.LOG_CATEGORY);

            var span = new TextSpan(position, 1);
            var message = $"Bad character input: '{character}'.";

            Report(span, message);
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxKind actualKind, SyntaxKind expectedKind)
        {
            Int64 startTicks = Log.DIAGNOSTIC($"Enter/Exit", Common.LOG_CATEGORY);

            var message = $"Unexpected token: <{actualKind}>, expected <{expectedKind}>.";

            Report(span, message);
        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, Type operandType)
        {
            Int64 startTicks = Log.DIAGNOSTIC($"Enter/Exit", Common.LOG_CATEGORY);

            var message = $"Unary operator '{operatorText}' is not defined for type '{operandType}'.";

            Report(span, message);
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string operatorText, Type leftType, Type rightType)
        {
            Int64 startTicks = Log.DIAGNOSTIC($"Enter/Exit", Common.LOG_CATEGORY);

            var message = $"Binary operator '{operatorText}' is not defined for types '{leftType}' and '{rightType}'.";

            Report(span, message);
        }

        public void ReportUndefinedName(TextSpan span, string name)
        {
            Int64 startTicks = Log.DIAGNOSTIC($"Enter/Exit", Common.LOG_CATEGORY);

            var message = $"Variable '{name}' doesn't exist.";

            Report(span, message);
        }

        public void ReportVariableAlreadyDeclared(TextSpan span, string name)
        {
            Int64 startTicks = Log.DIAGNOSTIC($"Enter/Exit", Common.LOG_CATEGORY);

            var message = $"Variable '{name}' is already declared.";

            Report(span, message);
        }

        public void ReportCannotConvert(TextSpan span, Type fromType, Type toType)
        {
            Int64 startTicks = Log.DIAGNOSTIC($"Enter/Exit", Common.LOG_CATEGORY);

            var message = $"Cannot convert type '{fromType}' to '{toType}'.";

            Report(span, message);
        }

        public void ReportCannotAssign(TextSpan span, string name)
        {
            Int64 startTicks = Log.DIAGNOSTIC($"Enter/Exit", Common.LOG_CATEGORY);

            var message = $"Variable '{name}' is read-only and cannot be assigned to.";

            Report(span, message);
        }
    }
}