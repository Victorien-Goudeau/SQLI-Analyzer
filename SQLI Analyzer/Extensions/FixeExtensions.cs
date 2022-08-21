using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;

namespace SQLI_Analyzer.Extensions
{
    public static class FixeExtensions
    {
        public static async Task<Document> GetDocumentWithReplacedNode(
            this CodeFixContext context,
            SyntaxNode oldNode,
            SyntaxNode newNode,
            SyntaxNode root = null)
        {
            if (root == null)
            {
                root = await context.Document
                    .GetSyntaxRootAsync(context.CancellationToken)
                    .ConfigureAwait(false);
            }

            var newRoot = root.ReplaceNode(oldNode, newNode);
            return context.Document
                .WithSyntaxRoot(newRoot);
        }

        public static SyntaxTokenList Add(this SyntaxTokenList list, SyntaxKind syntaxKind)
        {
            return Add(list, SyntaxFactory.Token(syntaxKind));
        }

        public static SyntaxTokenList Add(this SyntaxTokenList list, SyntaxToken syntaxToken)
        {
            var newSyntaxOrder = IndexOf(syntaxToken);
            if (newSyntaxOrder >= 0)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    var index = IndexOf(list[i]);
                    if (index > newSyntaxOrder || index < 0)
                    {
                        return list.Insert(i, syntaxToken);
                    }
                }
            }

            return list.Add(syntaxToken);
        }

        private static int IndexOf(SyntaxToken token)
        {
            for (var i = 0; i < s_modifiersSortOrder.Length; i++)
            {
                if (s_modifiersSortOrder[i] == token.Text)
                    return i;
            }

            return -1;
        }

        private static readonly string[] s_modifiersSortOrder = GetModifiersOrder();

        private static string[] GetModifiersOrder()
        {
            return "public,private,protected,internal,const,static,extern,new,virtual,abstract,sealed,override,readonly,unsafe,volatile,async".Split(',').ToArray();
        }
    }
}
