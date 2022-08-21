using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SQLI_Analyzer.Rule;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;

namespace SQLI_Analyzer.CodeFixes;

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UseEmptyStringFixe)), Shared]
    public sealed class UseEmptyStringFixe : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(RuleIdentifier.UseEmptyString);
        private const string _RuleId = RuleIdentifier.UseEmptyString;

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);            

            var title = "Use string.Empty";
            var codeAction = CodeAction.Create(
                title,
                ct => ChangeForEmptyString(context.Document, context.Diagnostics.First() ,root, ct),
                equivalenceKey: title);

            context.RegisterCodeFix(codeAction, context.Diagnostics);
        }

        private static async Task<Document> ChangeForEmptyString(Document document, Diagnostic diagnostic, SyntaxNode nodeToFix, CancellationToken cancellationToken)
        {
            var literal = nodeToFix.FindNode(diagnostic.Location.SourceSpan).DescendantNodesAndSelf().OfType<LiteralExpressionSyntax>().First();
            var newRoot = nodeToFix.ReplaceNode(literal, SyntaxFactory.ParseExpression("string.Empty").WithLeadingTrivia(literal.GetLeadingTrivia()).WithTrailingTrivia(literal.GetTrailingTrivia()));
            var newDocument = document.WithSyntaxRoot(newRoot);
            return document.WithSyntaxRoot(newRoot);
        }

    }
