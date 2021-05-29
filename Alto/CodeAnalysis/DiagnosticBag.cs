using System;
using System.Collections;
using System.Collections.Generic;
using Alto.CodeAnalysis.Symbols;
using Alto.CodeAnalysis.Syntax;
using Alto.CodeAnalysis.Text;

namespace Alto.CodeAnalysis
{
    public sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void AddRange(DiagnosticBag diagnostics)
        {
            _diagnostics.AddRange(diagnostics._diagnostics);
        }

        public void Concat(DiagnosticBag diagnostics)
        {
            _diagnostics.AddRange(diagnostics._diagnostics);
        }

        private void Report(TextSpan span, string message)
        {
            var diagnostic = new Diagnostic(span, message);
            _diagnostics.Add(diagnostic);
        }

        public void ReportInvalidNumber(TextSpan textSpan, string text, TypeSymbol type)
        {
            var message = $"The number '{text}' isn't a valid '{type.ToString()}'";
            Report(textSpan, message);
        }

        public void ReportBadCharacter(int position, char character)
        {
            var message = $"Bad character in input: '{character}'";
            TextSpan span = new TextSpan(position, 1);
            Report(span, message);
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxKind kind, SyntaxKind expectedKind)
        {
            var message = $"Unexpeced token <{kind}>, expected <{expectedKind}>.";
            Report(span, message);
        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, TypeSymbol operandType)
        {
            var message = $"Unary operator '{operatorText}' is not defined for type '{operandType.ToString()}'.";
            Report(span, message);
        }

        public void ReportUndefinedName(TextSpan span, string name)
        {
           var message = $"Variable '{name}' doesn't exist";
           Report(span, message);
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string text, TypeSymbol leftType, TypeSymbol rightType)
        {
            var message = $"Binary operator '{text}' is not defined for type '{leftType.ToString()}' and '{rightType.ToString()}'.";
            Report(span, message);
        }

        public void ReportSymbolAlreadyDeclared(TextSpan span, string name)
        {
            var message = $"'{name}' is already declared in the current scope.";
            Report(span, message);
        }

        public void VariableCannotConvert(TextSpan span, TypeSymbol fromType, TypeSymbol toType)
        {
            var message = $"Cannot convert type '{fromType.ToString()}' to type '{toType.ToString()}'.";
            Report(span, message);
        }

        public void ReportCannotAssign(TextSpan span, string name)
        {
            var message = $"Cannot assign to variable '{name}'.";
            Report(span, message);
        }

        public void ReportCannotConvert(TextSpan span, TypeSymbol type, TypeSymbol targetType)
        {
            var message = $"Cannot convert type '{type.ToString()}' to type '{targetType.ToString()}'.";
            Report(span, message);
        }

        public void ReportUnterminatedString(TextSpan span)
        {
            var message = $"Unterminated string literal.";
            Report(span, message);
        }

        public void ReportUndefinedFunction(TextSpan span, string name)
        {
            var message = $"Function '{name}' is not defined.";
            Report(span, message);
        }

        public void ReportWrongArgumentCount(TextSpan span, string name, int expectedCount, int count)
        {
            var message = $"Function '{name}' expects '{expectedCount}' argument/s, got '{count}'.";
            Report(span, message);
        }

        public void ReportWrongArgumentType(TextSpan span, string funcName, string paramName, TypeSymbol paramType, TypeSymbol argumentType)
        {
            var message = $"Parameter '{paramName}' in '{funcName}' has to be of type '{paramType.ToString()} is of type '{argumentType.ToString()}'.";
            Report(span, message);
        }

        internal void ReportExpressionMustHaveAValue(TextSpan span)
        {
            var message = $"Expression must have a different value than void.";
            Report(span, message);
        }
    }
}