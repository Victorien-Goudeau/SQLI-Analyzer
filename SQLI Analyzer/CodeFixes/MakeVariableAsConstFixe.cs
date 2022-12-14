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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;

namespace SQLI_Analyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MakeVariableAsConstFixe)), Shared]
    public sealed class MakeVariableAsConstFixe : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(RuleIdentifier.MakeVariableAsConst);
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
        
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
           var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

           var diagnostic = context.Diagnostics.First();
           var diagnosticSpan = diagnostic.Location.SourceSpan;

            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<LocalDeclarationStatementSyntax>().First();

            context.RegisterCodeFix(
                            CodeAction.Create(
                                "MakeVariableAsConst",
                                createChangedDocument: c => MakeConstAsync(context.Document, declaration, c),
                                equivalenceKey: "MakeVariableAsConst"),
                            diagnostic);
        }

        private static async Task<Document> MakeConstAsync(Document document,
                                            LocalDeclarationStatementSyntax localDeclaration,
                                            CancellationToken cancellationToken)
        {

            SyntaxToken firstToken = localDeclaration.GetFirstToken();
            SyntaxTriviaList leadingTrivia = firstToken.LeadingTrivia;
            LocalDeclarationStatementSyntax trimmedLocal = localDeclaration.ReplaceToken(
                firstToken, firstToken.WithLeadingTrivia(SyntaxTriviaList.Empty));


            SyntaxToken constToken = SyntaxFactory.Token(leadingTrivia, SyntaxKind.ConstKeyword, SyntaxFactory.TriviaList(SyntaxFactory.ElasticMarker));


            SyntaxTokenList newModifiers = trimmedLocal.Modifiers.Insert(0, constToken);

            LocalDeclarationStatementSyntax newLocal = trimmedLocal
                .WithModifiers(newModifiers)
                .WithDeclaration(localDeclaration.Declaration);

            LocalDeclarationStatementSyntax formattedLocal = newLocal.WithAdditionalAnnotations(Formatter.Annotation);

            SyntaxNode oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            SyntaxNode newRoot = oldRoot.ReplaceNode(localDeclaration, formattedLocal);

            return document.WithSyntaxRoot(newRoot);
        }
       
    }
}
