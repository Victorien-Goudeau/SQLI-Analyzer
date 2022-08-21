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
    public sealed class SimplifyBooleanConditionAnalyzer : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor rule = new(
           RuleIdentifier.SimplifyBooleanCondition,
           title: "Simplify boolean condition",
           messageFormat: "It is possible to simplify the condition",
           category: "Style",
           defaultSeverity: DiagnosticSeverity.Warning,
           isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.EqualsExpression, SyntaxKind.NotEqualsExpression);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var booleanComparaison  = (BinaryExpressionSyntax)context.Node;

            var leftComparaison = context.SemanticModel.GetTypeInfo(booleanComparaison.Left).Type;
            var rightComparaison = context.SemanticModel.GetTypeInfo(booleanComparaison.Right).Type;


            if (!IsBoolean(leftComparaison) && !IsBoolean(rightComparaison))
                return;

            var leftConstant = context.SemanticModel.GetConstantValue(booleanComparaison.Left);
            var rightConstant = context.SemanticModel.GetConstantValue(booleanComparaison.Right);
            if (!leftConstant.HasValue && !rightConstant.HasValue)
                return;

            context.ReportDiagnostic(Diagnostic.Create(rule, context.Node.GetLocation()));
        }

        private static bool IsBoolean(ITypeSymbol symbol) => symbol is not null && symbol.SpecialType == SpecialType.System_Boolean;
    }
}
