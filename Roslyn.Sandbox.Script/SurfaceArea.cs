namespace Roslyn.Sandbox.Script
{
    using System.Reflection;

    public static class SurfaceArea
    {
        public static Assembly[] GetAssemblies()
        {
            var mscorlib = typeof(object).Assembly;
            var systemCore = typeof(System.Linq.Enumerable).Assembly;
            var systemCollectionsImmutable = typeof(System.Collections.Immutable.ImmutableArray).Assembly;
            var systemNetHttp = typeof(System.Net.Http.HttpClient).Assembly;
            var systemRuntimeSerialization = typeof(System.Runtime.Serialization.DataContractSerializer).Assembly;
            var systemXml = typeof(System.Xml.XmlDocument).Assembly;
            var systemXmlLinq = typeof(System.Xml.Linq.XDocument).Assembly;
            var microsoftCodeAnalysis = typeof(Microsoft.CodeAnalysis.Compilation).Assembly;
            var microsoftCodeAnalysisCSharp = typeof(Microsoft.CodeAnalysis.CSharp.CSharpCompilation).Assembly;
            var microsoftCodeAnalysisCSharpScripting = typeof(Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript).Assembly;
            var microsoftCodeAnalysisScripting = typeof(Microsoft.CodeAnalysis.Scripting.Script).Assembly;
            var newtonsoftJson = typeof(Newtonsoft.Json.JsonConvert).Assembly;
            var roslynSandboxScript = typeof(SurfaceArea).Assembly;

            return new Assembly[]
            {
                mscorlib,
                systemCore,
                systemCollectionsImmutable,
                systemNetHttp,
                systemRuntimeSerialization,
                systemXml,
                systemXmlLinq,
                microsoftCodeAnalysis,
                microsoftCodeAnalysisCSharp,
                microsoftCodeAnalysisCSharpScripting,
                microsoftCodeAnalysisScripting,
                newtonsoftJson,
                roslynSandboxScript
            };
        }

        public static string[] GetNamespaces()
        {
            return new string[]
            {
                "System",
                "System.IO",
                "System.Linq",
                "System.Collections.Generic",
                "System.Collections.Immutable",
                "System.Net",
                "System.Net.Http",
                "System.Runtime.Serialization",
                "System.Text",
                "System.Text.RegularExpressions",
                "System.Threading.Tasks",
                "System.Xml",
                "System.Xml.Linq",
                "Newtonsoft.Json"
            };
        }
    }
}
