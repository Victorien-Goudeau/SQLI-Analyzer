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

namespace SQLI_Analyzer.Analyzer
{
    public sealed class RemoveUnusedParameterInMethod : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor rule = new(
            RuleIdentifier.RemoveUnusedParameterInMethod,
            "Remove unused parameter",
            "Remove unused parameter in method",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
           
        }

        private static void Analyze(SyntaxNodeAnalysisContext context)
        {
           var methodDeclaration = (MethodDeclarationSyntax)context.Node;

            
        }

    }
}
