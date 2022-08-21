using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using SQLI_Analyzer.Rule;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Formatting;

namespace SQLI_Analyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SimplifyBooleanConditionFixe)), Shared]
    public sealed class SimplifyBooleanConditionFixe : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(RuleIdentifier.SimplifyBooleanCondition);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var title = "Simplify boolean condition";
            var codeAction = CodeAction.Create(
                title,
                ct => SimplifyBooleanCondition(context.Document, context.Diagnostics.First(), root, ct),
                equivalenceKey: title);

            context.RegisterCodeFix(codeAction, context.Diagnostics);
        }

        private static async Task<Document> SimplifyBooleanCondition(Document document, Diagnostic diagnostic, SyntaxNode nodeToFix, CancellationToken cancellationToken)
        {
            var comparison = nodeToFix.FindToken(diagnostic.Location.SourceSpan.Start)
                 .Parent.AncestorsAndSelf()
                 .OfType<BinaryExpressionSyntax>()
                 .First(bes => !bes.IsKind(SyntaxKind.IsExpression));

            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            bool constValue;
            ExpressionSyntax replacer;
            var rightConst = semanticModel.GetConstantValue(comparison.Right);
            if (rightConst.HasValue)
            {
                constValue = (bool)rightConst.Value;
                replacer = comparison.Left;
            }
            else
            {
                var leftConst = semanticModel.GetConstantValue(comparison.Left);
                constValue = (bool)leftConst.Value;
                replacer = comparison.Right;
            }


            if ((!constValue && comparison.IsKind(SyntaxKind.EqualsExpression)) ||
                (constValue && comparison.IsKind(SyntaxKind.NotEqualsExpression)))
            {
                if (comparison.Left is BinaryExpressionSyntax)
                {
                    replacer = SyntaxFactory.ParenthesizedExpression(replacer);
                }
                replacer = SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, replacer);
            }

            replacer = replacer
                .WithAdditionalAnnotations(Formatter.Annotation);


            var newRoot = nodeToFix.ReplaceNode(comparison, replacer);
            var newDocument = document.WithSyntaxRoot(newRoot);
            return newDocument;
        }
    }
}
