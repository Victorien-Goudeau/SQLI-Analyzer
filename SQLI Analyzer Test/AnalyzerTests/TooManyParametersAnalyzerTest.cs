using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.AnalyzerVerifier<SQLI_Analyzer.Analyzer.TooManyParametersAnalyzer>;

namespace Analyser_Tests.AnalyzerTests
{
    public class TooManyParametersAnalyzerTest
    {
        [Fact]
        public async void Should_Trigger_Diagnostic()
        {
            var source = @"
                            using System;

                            class Program
                            {
                                public void Main(int a, int b, int c, int d){ }
                            }
                            ";
            var expected = Verify.Diagnostic().WithSpan(6, 33, 6, 80);
            await Verify.VerifyAnalyzerAsync(source, expected);
        }
    }
}
