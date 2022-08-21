using Verify = Microsoft.CodeAnalysis.CSharp.Testing.MSTest.AnalyzerVerifier<SQLI_Analyzer.Analyzer.CountLineInMethodAnalyzer>;

namespace Analyser_Tests.AnalyzerTests;

    public class CountLineMethod
    {
        [Fact]
        public async void Should_Trigger_Diagnostic()
        {
            var source = @"
                            using System;

                            class Program
                            {
                                static void Main(){
                                    var a = 4;
                                    var b = 4;
                                    var c = 4;
                                    var d = 4;}
                            }
                            ";
            var expected = Verify.Diagnostic().WithSpan(6, 33, 10, 48);
            await Verify.VerifyAnalyzerAsync(source, expected);
        }


        [Fact]
        public async void Should_Not_Trigger_Diagnostic()
        {
            var source = @"
                            using System;

                            class Program
                            {
                                static void Main(){
                                    var a = 4;
                                    var b = 4;
                                    var c = 4;}
                            }
                            ";
            await Verify.VerifyAnalyzerAsync(source);
        }
    }

