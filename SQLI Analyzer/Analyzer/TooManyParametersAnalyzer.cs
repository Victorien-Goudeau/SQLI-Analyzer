using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
    public class TooManyParametersAnalyzer : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor rule = new(
           RuleIdentifier.MethodTooLong,
           "Too many Parameters",
           "There is too many parameters",
           "Usage",
           DiagnosticSeverity.Warning,
           isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.MethodDeclaration);
        }


        private static void Analyze(SyntaxNodeAnalysisContext context)
        {
            
        }
    }
}
