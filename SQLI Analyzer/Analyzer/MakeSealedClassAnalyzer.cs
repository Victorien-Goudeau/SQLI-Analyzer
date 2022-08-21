
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
using System.Threading;
using System.Threading.Tasks;
using SQLI_Analyzer.Extensions;

namespace SQLI_Analyzer.Analyzer;

    public sealed class MakeSealedClassAnalyzer : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor rule = new(
            RuleIdentifier.MakeSealedClass,
            "Make sealed",
            "Make class as sealed",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault:true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);
        }

        private static void Analyze(SymbolAnalysisContext context)
        {
            ISymbol symbol = context.Symbol;
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.TypeKind is not TypeKind.Class)
                return;

            if (!CouldBeSealed(symbol))
                return;
           
            var classDeclaration = (ClassDeclarationSyntax)namedTypeSymbol.GetSyntax(context.CancellationToken);

            context.ReportDiagnostic(Diagnostic.Create(rule, classDeclaration.Identifier.GetLocation()));
           
        }

        private static bool CouldBeSealed(ISymbol symbol)
        {
            if(symbol.IsAbstract || symbol.IsSealed || symbol.IsStatic)
                return false;

            return true;
        }      
    }