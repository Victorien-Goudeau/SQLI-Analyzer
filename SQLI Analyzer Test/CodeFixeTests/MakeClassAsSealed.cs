using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.CodeFixVerifier<SQLI_Analyzer.Analyzer.MakeSealedClassAnalyzer, SQLI_Analyzer.CodeFixes.MakeSealedClassFixe>;

namespace Analyser_Tests.CodeFixeTests
{
    public class MakeClassAsSealedCodeFixe 
    {
        [Fact]
        public async void Should_Fixe_Code()
        {
            const string codeToFix = @"
public class User
{ }";
            const string codeTofie = @"
public sealed class User
{ }";

            var expected = Verify.Diagnostic().WithLocation(2,14);

            await Verify.VerifyCodeFixAsync(codeToFix, expected, codeTofie);
        }
    }
}
