namespace Roslyn.Sandbox.Test
{
    using System;
    using Xunit;

    public class SafeScriptTest
    {
        [Fact]
        public void Evaluate()
        {
            var result = SafeScript.Evaluate("int x =10; x", TimeSpan.FromSeconds(10));

            Assert.Equal(10, result);
        }
    }
}
