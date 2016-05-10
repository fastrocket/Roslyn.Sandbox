namespace Roslyn.Sandbox.Script
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class Script
    {
        private static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

        public static async Task<object> EvaluateAsync(string code, TimeSpan? timeout = null)
        {
            var scriptOptions = GetScriptOptions();

            object result = null;

            try
            {
                if (timeout.HasValue)
                {
                    var cancellationTokenSource = new CancellationTokenSource(timeout.Value);
                    result = await CSharpScript.EvaluateAsync(
                        code,
                        options: scriptOptions,
                        cancellationToken: cancellationTokenSource.Token);
                }
                else
                {
                    result = await CSharpScript.EvaluateAsync(code, options: scriptOptions);
                }
            }
            catch (Exception exception)
            {
                result = exception;
            }

            if (result != null && !IsSerializable(result))
            {
                var exception = result as Exception;
                if (exception != null)
                {
                    result = exception.ToString();
                }
                else
                {
                    var jsonSerializerSettings = new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented
                    };
                    jsonSerializerSettings.Converters.Add(new StringEnumConverter());
                    result = JsonConvert.SerializeObject(result, jsonSerializerSettings);
                }
            }

            return result;
        }

        private static bool IsSerializable(object value)
        {
            if (value is bool ||
                value is DateTime ||
                value is DateTimeOffset ||
                value is TimeSpan ||
                value is char ||
                value is string ||
                value is decimal ||
                value is byte ||
                value is short ||
                value is int ||
                value is long ||
                value is double ||
                value is float ||
                value is Enum)
            {
                return true;
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    binaryFormatter.Serialize(memoryStream, value);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private static ScriptOptions GetScriptOptions()
        {
            return ScriptOptions
                .Default
                .AddReferences(SurfaceArea.GetAssemblies())
                .AddImports(SurfaceArea.GetNamespaces());
        }
    }
}
