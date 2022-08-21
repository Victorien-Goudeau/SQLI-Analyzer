using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SQLI_Analyzer.Rule;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SQLI_Analyzer.Analyzer;

    public sealed class RemoveRedundantCommaAnalyzer : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor rule = new(
            RuleIdentifier.RemoveRedundantComma,
            "Remove redundant comma",
            "Remove redundant comma",
            "Usage",
            DiagnosticSeverity.Info,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.ArrayInitializerExpression);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var line = (InitializerExpressionSyntax)context.Node;

            var expression = line.Expressions;

            if (!expression.Any() && expression.Count != expression.SeparatorCount)
                return;

            SyntaxToken token = expression.GetSeparator(expression.Count - 1);

            context.ReportDiagnostic(Diagnostic.Create(rule, context.Node.GetLocation(),token));
        }
    }