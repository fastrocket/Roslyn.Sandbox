namespace Roslyn.Sandbox
{
    using System;
    using System.Linq;
    using System.Security;
    using System.Security.Permissions;
    using System.Security.Policy;
    using Roslyn.Sandbox.Script;

    public static class SafeScript
    {
        public static object Evaluate(string code, TimeSpan? timeout = null)
        {
            var appDomain = CreateAppDomain("Sandbox");
            ConfigureAppDomain(appDomain);

            using (var isolated = new IsolatedCommand<Command>(appDomain))
            {
                return isolated.Value.Evaluate(code, timeout);
            }
        }

        private static void ConfigureAppDomain(AppDomain appDomain)
        {
            foreach (var assemblyName in SurfaceArea.GetAssemblies())
            {
                appDomain.Load(assemblyName.FullName);
            }
        }

        private static AppDomain CreateAppDomain(string name)
        {
            var evidence = new Evidence();
            evidence.AddHostEvidence(new Zone(SecurityZone.MyComputer));

            var securityManager = SecurityManager.GetStandardSandbox(evidence);
            securityManager.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            securityManager.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
            securityManager.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, @"C:\"));

            var appDomainSetup = new AppDomainSetup()
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
            };
            var fullTrustAssemblies = SurfaceArea
                .GetAssemblies()
                .Select(x => x.Evidence.GetHostEvidence<StrongName>())
                .ToArray();
            return AppDomain.CreateDomain(name + ":" + Guid.NewGuid(), null, appDomainSetup, securityManager, fullTrustAssemblies);
        }
    }
}
