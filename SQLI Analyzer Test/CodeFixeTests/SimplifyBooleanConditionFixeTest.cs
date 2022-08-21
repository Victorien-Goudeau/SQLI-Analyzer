using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.CodeFixVerifier<SQLI_Analyzer.Analyzer.SimplifyBooleanConditionAnalyzer, SQLI_Analyzer.CodeFixes.SimplifyBooleanConditionFixe>;

namespace SQLI_Analyzer_Test.CodeFixeTests
{
    public class SimplifyBooleanConditionFixeTest
    {
        [Fact]
        public async void Should_Fixe_Diagnostic()
        {
            var source = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public bool Foo()
            {
                bool a = true;
                if(a == false) return false;

                return true;
            }
        }
    }";

            var codeFixed = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public bool Foo()
            {
                bool a = true;
                if(!a) return false;

                return true;
            }
        }
    }";

            var exepected = Verify.Diagnostic().WithSpan(11, 20, 11, 30);
            await Verify.VerifyCodeFixAsync(source, exepected, codeFixed);
        }

    }
}
