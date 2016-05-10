namespace Roslyn.Sandbox.Script
{
    using System;

    public class Command : MarshalByRefObject
    {
        public object Evaluate(string code, TimeSpan? timeout = null)
        {
            var task = Script.EvaluateAsync(code, timeout);
            task.Wait();
            return task.Result;
        }
    }
}
