namespace Roslyn.Sandbox.Script.Test
{
    using System;
    using System.Threading.Tasks;
    using Roslyn.Sandbox.Script;
    using Xunit;

    public class ScriptTest
    {
        [Theory]
        [InlineData("using System; var x = 10; x", 10)]
        [InlineData("var x = 10; x", 10)]
        [InlineData("var x = 10;", null)]
        [InlineData("var x = 10; return x;", 10)]
        [InlineData("class Foo { public string Bar { get; set; } } new Foo()", "{\r\n  \"Bar\": null\r\n}")]
        [InlineData("DayOfWeek.Monday", DayOfWeek.Monday)]
        public async Task EvaluateAsync_ExecuteCode_ReturnsExpectedValue(string code, object expectedResult)
        {
            var result = await Script.EvaluateAsync(code, TimeSpan.FromSeconds(10));

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("new HttpClient().GetStringAsync(\"http://google.com\").Result", "<html")]
        public async Task EvaluateAsync_ExecuteCode_ContainsExpectedValue(string code, string expectedResult)
        {
            var result = await Script.EvaluateAsync(code, TimeSpan.FromSeconds(10));

            Assert.Contains(expectedResult, result.ToString());
        }

        [Theory]
        [InlineData("int x = 10", 10, nameof(Microsoft.CodeAnalysis.Scripting.CompilationErrorException))]
        public async Task EvaluateAsync_ExecuteCode_ReturnsExpectedExceptionAsString(
            string code,
            int timeoutSeconds,
            string expectedExceptionType)
        {
            var result = await Script.EvaluateAsync(code, TimeSpan.FromSeconds(timeoutSeconds));

            Assert.IsType<string>(result);
            Assert.Contains(expectedExceptionType, result.ToString());
        }

        [Theory]
        [InlineData("long x = 0; while (true) { ++x; }", 0, typeof(OperationCanceledException))]
        [InlineData("throw new ArgumentException();", 10, typeof(ArgumentException))]
        public async Task EvaluateAsync_ExecuteCode_ReturnsExpectedException(
            string code,
            int timeoutSeconds,
            Type expectedExceptionType)
        {
            var result = await Script.EvaluateAsync(code, TimeSpan.FromSeconds(timeoutSeconds));

            Assert.IsType(expectedExceptionType, result);
        }
    }
}
