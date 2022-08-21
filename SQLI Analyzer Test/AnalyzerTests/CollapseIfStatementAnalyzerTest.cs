using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.AnalyzerVerifier<SQLI_Analyzer.Analyzer.CollapseIfAnalyzer>;

namespace SQLI_Analyzer_Test.AnalyzerTests
{
    public class CollapseIfStatementAnalyzerTest
    {
        [Fact]
        public async void Shoudl_Trigger_Analyzer()
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
                if(a == false) {
                if(!a){}
                };

                return true;
            }
        }
    }";

            var exepected = Verify.Diagnostic().WithSpan(12, 17, 12, 25);
            await Verify.VerifyAnalyzerAsync(source, exepected);
        }
    }
}
