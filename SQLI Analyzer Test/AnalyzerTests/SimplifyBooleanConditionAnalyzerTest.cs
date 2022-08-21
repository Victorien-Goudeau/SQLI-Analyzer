using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.AnalyzerVerifier<SQLI_Analyzer.Analyzer.SimplifyBooleanConditionAnalyzer>;

namespace SQLI_Analyzer_Test.AnalyzerTests
{
    public class SimplifyBooleanConditionAnalyzerTest
    {
        [Fact]
        public async void Should_Trigger_Diagnostic()
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

            var exepected = Verify.Diagnostic().WithSpan(11, 20, 11, 30);
            await Verify.VerifyAnalyzerAsync(source, exepected);
        }
    }
}
