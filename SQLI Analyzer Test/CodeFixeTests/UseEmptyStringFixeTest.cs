using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.CodeFixVerifier<SQLI_Analyzer.Analyzer.UseEmptyString, SQLI_Analyzer.CodeFixes.UseEmptyStringFixe>;

namespace Analyser_Tests.CodeFixeTests
{
    public class UseEmptyStringFixeTest
    {
        [Fact]
        public async void Should_Fix_Diagnostic()
        {
            const string source = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public void Foo()
            {
                string a = """";
            }
        }
    }";

            const string expectedFixe = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public void Foo()
            {
                string a = string.Empty;
            }
        }
    }";

            var expected = Verify.Diagnostic().WithSpan(10, 28, 10, 30);

            await Verify.VerifyCodeFixAsync(source, expected, expectedFixe);
        }
    }
}
