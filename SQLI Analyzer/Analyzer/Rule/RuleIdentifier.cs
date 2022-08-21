using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLI_Analyzer.Rule
{
    internal static class RuleIdentifier
    {
        public const string MakeVariableAsConst = "SQ0001";
        public const string MakeSealedClass = "SQ0002";
        public const string MergeIf = "SQ0003";
        public const string UseEmptyString = "SQ0004";
        public const string MethodTooLong = "SQ0005";
        public const string TooManyParameters = "SQ0006";
    }
}
