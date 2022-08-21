using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SQLI_Analyzer.Extensions;
using SQLI_Analyzer.Rule;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLI_Analyzer.Analyzer;

    public class CollapseIfAnalyzer : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor rule = new(
            RuleIdentifier.CollapseIf,
            "Collapse if statement",
            "Collapse if statement",
            "Usage",
            DiagnosticSeverity.Info,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.IfStatement);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var ifStatement = (IfStatementSyntax)context.Node;

            var parent = ifStatement.Parent;

            while (parent.IsKind(SyntaxKind.Block))
            {
                var block = (BlockSyntax)parent;

                if (block.Statements.Count != 1)
                {
                    return;
                }

                parent = parent.Parent;
            }

            context.ReportDiagnostic(Diagnostic.Create(rule, context.Node.GetLocation()));

        }
    }