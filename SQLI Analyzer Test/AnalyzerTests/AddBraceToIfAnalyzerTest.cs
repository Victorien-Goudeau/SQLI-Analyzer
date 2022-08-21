using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.AnalyzerVerifier<SQLI_Analyzer.Analyzer.AddBraceToIfAnalyzer>;

namespace SQLI_Analyzer_Test.AnalyzerTests
{
    public class AddBraceToIfAnalyzerTest
    {
        [Fact]
        public async void Should_Trigger_Analyzer()
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
                if(a == false){} 
                return true;
            }
        }
    }";
            
            await Verify.VerifyAnalyzerAsync(source);
        }
    }
}
