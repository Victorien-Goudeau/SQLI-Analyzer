using SQLI_Analyzer.Analyzer;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.AnalyzerVerifier<SQLI_Analyzer.Analyzer.UseEmptyString>;

namespace Analyser_Tests.AnalyzerTests
{
    public class UsesStringEmptyTest
    {
        [Fact]
        public async void Analyzer_is_triggered()
        {
            var source = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public void Foo()
            {
                var a = ""abc"";
                var t = """";
            }
        }
    }";

            var exepected = Verify.Diagnostic().WithSpan(11, 25, 11, 27);
            await Verify.VerifyAnalyzerAsync(source, exepected);
        }
    }
}
