using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.AnalyzerVerifier<SQLI_Analyzer.Analyzer.EmptyMethodAnalyzer>;

namespace SQLI_Analyzer_Test.AnalyzerTests
{
    public class EmptyMethodAnalyzerTest
    {
        [Fact]
        public async void Should_Trigger_Analyzer()
        {
            var source = @"
                            using System;

                            class Program
                            {
                                public void Main(){ }
                            }
                            ";
            await Verify.VerifyAnalyzerAsync(source);
        }
    }
}
