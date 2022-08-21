using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using SQLI_Analyzer.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SQLI_Analyzer.Analyzer;

    public sealed class UseEmptyString : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor rule = new(
            RuleIdentifier.UseEmptyString,
            title:"Use EmptyString",
            messageFormat:"Use EmptyString rather than \"\"",
            category:"Style",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.StringLiteralExpression);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var literal = context.Node as LiteralExpressionSyntax;
            bool isLiteralExpresionIsNotEmptryString = literal.ToString() is not "\"\"";

            if (isLiteralExpresionIsNotEmptryString) 
                return;

            context.ReportDiagnostic(Diagnostic.Create(rule, literal.GetLocation()));
        }       
    }