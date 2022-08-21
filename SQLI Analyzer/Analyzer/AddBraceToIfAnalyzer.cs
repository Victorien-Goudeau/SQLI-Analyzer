using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SQLI_Analyzer.Rule;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLI_Analyzer.Analyzer
{
    public sealed class AddBraceToIfAnalyzer : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor rule = new(
            RuleIdentifier.AddBraceToIf,
            "Add brace",
            "The block does not contain brace",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.IfStatement);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var body = default(StatementSyntax);
            var token = default(SyntaxToken);
            switch (context.Node)
            {
                case IfStatementSyntax ifstate:
                    body = ifstate.Statement;
                    token = ifstate.IfKeyword;
                    break;
                case ForEachStatementSyntax foreachstate:
                    body = foreachstate.Statement;
                    token = foreachstate.ForEachKeyword;
                    break;
                case ForStatementSyntax forstate:
                    body = forstate.Statement;
                    token = forstate.ForKeyword;
                    break;
                case WhileStatementSyntax whilestate:
                    body = whilestate.Statement;
                    token = whilestate.WhileKeyword;
                    break;                   
            }

            if (!body.IsKind(SyntaxKind.Block))
            {
                var diagnostic = Diagnostic.Create(rule, token.GetLocation(), context.Node.Kind().ToString());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
