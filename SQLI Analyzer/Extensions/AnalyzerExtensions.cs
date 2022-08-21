using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLI_Analyzer.Extensions
{
    public static class AnalyzerExtensions
    {
        internal static SyntaxNode GetSyntax(this ISymbol symbol, CancellationToken cancellationToken = default)
        {
            return symbol
                .DeclaringSyntaxReferences[0]
                .GetSyntax(cancellationToken);
        }
    }
}
