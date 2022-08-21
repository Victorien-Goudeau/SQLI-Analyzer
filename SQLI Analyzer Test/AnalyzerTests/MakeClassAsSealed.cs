using SQLI_Analyzer.Analyzer;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.AnalyzerVerifier<SQLI_Analyzer.Analyzer.MakeSealedClassAnalyzer>;
namespace Analyser_Tests.AnalyzerTests
{
    public class MakeClassAsSealedTest 
    {
        [Fact]
        public async void Analyzer_is_triggered()
        {
            var source = @"public class User{}";
            var expected = Verify.Diagnostic().WithLocation(1, 14);
            await Verify.VerifyAnalyzerAsync(source, expected);            
        }

        [Fact]
        public async void Analyzer_is_Not_triggered()
        {
            var source = @"public sealed class User{}";            
            await Verify.VerifyAnalyzerAsync(source);
        }
    }
}

