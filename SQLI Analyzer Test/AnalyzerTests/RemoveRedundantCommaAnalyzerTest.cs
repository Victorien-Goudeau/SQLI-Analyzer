using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.AnalyzerVerifier<SQLI_Analyzer.Analyzer.RemoveRedundantCommaAnalyzer>;

namespace SQLI_Analyzer_Test.AnalyzerTests
{
    public class RemoveRedundantCommaAnalyzerTest
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
            public void Foo()
            {
                var arr = new string[] { ""a"", ""b"", ""c"", };
            }
        }
    }";

            var exepected = Verify.Diagnostic().WithSpan(10, 40, 10, 58).WithArguments(",");
            await Verify.VerifyAnalyzerAsync(source, exepected);

        }
    }
}
