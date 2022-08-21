using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using SQLI_Analyzer.Extensions;
using SQLI_Analyzer.Rule;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLI_Analyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MakeSealedClassFixe)), Shared]
    public sealed class MakeSealedClassFixe : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(RuleIdentifier.MakeSealedClass);
        private const string _RuleId = RuleIdentifier.MakeSealedClass;
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public  override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root?.FindNode(context.Span, getInnermostNodeForTie: true) is not ClassDeclarationSyntax nodeToFix)
                return;

            var title = "Add sealed modifier";
            var codeAction = CodeAction.Create(
                title,
                ct => AddSealedModifierClass(context.Document, nodeToFix, ct),
                equivalenceKey: title);

            context.RegisterCodeFix(codeAction, context.Diagnostics);
        }

        private static async Task<Document> AddSealedModifierClass(Document document, SyntaxNode nodeToFix, CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

            var classNode = (ClassDeclarationSyntax)nodeToFix;
            var modifiers = classNode.Modifiers.Add(SyntaxKind.SealedKeyword);
            editor.ReplaceNode(classNode, classNode.WithModifiers(modifiers).WithAdditionalAnnotations(Formatter.Annotation));
            return editor.GetChangedDocument();
        }
    }
}
