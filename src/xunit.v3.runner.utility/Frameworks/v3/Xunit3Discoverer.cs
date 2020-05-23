using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit.Abstractions;

namespace Xunit.Frameworks.v3
{
    /// <summary>
    /// This class be used to do discovery of xUnit.net v3 tests, via any implementation
    /// of <see cref="IAssemblyInfo"/>, including AST-based runners like CodeRush and
    /// Resharper. Runner authors who are not using AST-based discovery are strongly
    /// encouraged to use <see cref="XunitFrontController"/> instead.
    /// </summary>
    public class Xunit3Discoverer : ITestFrameworkDiscoverer, ITestCaseDescriptorProvider
    {
        ITestCaseDescriptorProvider defaultTestCaseDescriptorProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Xunit2Discoverer"/> class.
        /// </summary>
        /// <param name="sourceInformationProvider">The source code information provider.</param>
        /// <param name="assemblyInfo">The assembly to use for discovery</param>
        /// <param name="xunitExecutionAssemblyPath">The path on disk of xunit.v3.execution.dll; if <c>null</c>, then
        /// the location of xunit.v3.execution.dll is implied based on the location of the test assembly</param>
        /// <param name="diagnosticMessageSink">The message sink which received <see cref="IDiagnosticMessage"/> messages.</param>
        public Xunit3Discoverer(ISourceInformationProvider sourceInformationProvider,
                                IAssemblyInfo assemblyInfo,
                                string xunitExecutionAssemblyPath = null,
                                IMessageSink diagnosticMessageSink = null)
            : this(sourceInformationProvider, assemblyInfo, null, xunitExecutionAssemblyPath ?? GetExecutionAssemblyFileName(), null, diagnosticMessageSink)
        { }

        // Used by Xunit3 when initializing for both discovery and execution.
        internal Xunit3Discoverer(ISourceInformationProvider sourceInformationProvider,
                                  string assemblyFileName,
                                  string configFileName,
                                  IMessageSink diagnosticMessageSink = null)
            : this(sourceInformationProvider, null, assemblyFileName, GetExecutionAssemblyFileName(), configFileName, diagnosticMessageSink)
        { }

        Xunit3Discoverer(ISourceInformationProvider sourceInformationProvider,
                         IAssemblyInfo assemblyInfo,
                         string assemblyFileName,
                         string xunitExecutionAssemblyPath,
                         string configFileName,
                         IMessageSink diagnosticMessageSink)
        {
            Guard.ArgumentNotNull("assemblyInfo", (object)assemblyInfo ?? assemblyFileName);

            DiagnosticMessageSink = diagnosticMessageSink ?? new NullMessageSink();

            var appDomainAssembly = assemblyFileName ?? xunitExecutionAssemblyPath;
            AppDomain = AppDomainManagerFactory.Create(false, appDomainAssembly, configFileName, false, null);

            TestFrameworkAssemblyName = GetTestFrameworkAssemblyName(xunitExecutionAssemblyPath);

            // If we didn't get an assemblyInfo object, we can leverage the reflection-based IAssemblyInfo wrapper
            if (assemblyInfo == null)
                assemblyInfo = AppDomain.CreateObject<IAssemblyInfo>(TestFrameworkAssemblyName, "Xunit.Sdk.ReflectionAssemblyInfo", assemblyFileName);

            Framework = AppDomain.CreateObject<ITestFramework>(TestFrameworkAssemblyName, "Xunit.Sdk.TestFrameworkProxy", assemblyInfo, sourceInformationProvider, DiagnosticMessageSink);

            RemoteDiscoverer = Framework.GetDiscoverer(assemblyInfo);
        }

        internal IAppDomainManager AppDomain { get; }

        /// <inheritdoc/>
        public bool CanUseAppDomains => false;

        /// <summary>
        /// Gets the message sink used to report diagnostic messages.
        /// </summary>
        public IMessageSink DiagnosticMessageSink { get; }

        /// <summary>
        /// Returns the test framework from the remote app domain.
        /// </summary>
        public ITestFramework Framework { get; }

        internal ITestFrameworkDiscoverer RemoteDiscoverer { get; }

        /// <inheritdoc/>
        public string TargetFramework => RemoteDiscoverer.TargetFramework;

        internal AssemblyName TestFrameworkAssemblyName { get; }

        /// <inheritdoc/>
        public string TestFrameworkDisplayName => RemoteDiscoverer.TestFrameworkDisplayName;

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            RemoteDiscoverer.SafeDispose();
            Framework.SafeDispose();
        }

        /// <summary>
        /// Starts the process of finding all xUnit.net v3 tests in an assembly.
        /// </summary>
        /// <param name="includeSourceInformation">Whether to include source file information, if possible.</param>
        /// <param name="messageSink">The message sink to report results back to.</param>
        /// <param name="discoveryOptions">The options used by the test framework during discovery.</param>
        public void Find(bool includeSourceInformation, IMessageSink messageSink, ITestFrameworkDiscoveryOptions discoveryOptions)
            => RemoteDiscoverer.Find(includeSourceInformation, messageSink, discoveryOptions);

        /// <summary>
        /// Starts the process of finding all xUnit.net v3 tests in a class.
        /// </summary>
        /// <param name="typeName">The fully qualified type name to find tests in.</param>
        /// <param name="includeSourceInformation">Whether to include source file information, if possible.</param>
        /// <param name="messageSink">The message sink to report results back to.</param>
        /// <param name="discoveryOptions">The options used by the test framework during discovery.</param>
        public void Find(string typeName, bool includeSourceInformation, IMessageSink messageSink, ITestFrameworkDiscoveryOptions discoveryOptions)
            => RemoteDiscoverer.Find(typeName, includeSourceInformation, messageSink, discoveryOptions);

        static string GetExecutionAssemblyFileName()
        {
            try
            {
                var assemblyName = $"xunit.v3.execution";
                Assembly.Load(new AssemblyName { Name = assemblyName });
                return assemblyName + ".dll";
            }
            catch { }

            throw new InvalidOperationException("Could not find or load: xunit.v3.execution.dll");
        }

        /// <inheritdoc/>
        public List<TestCaseDescriptor> GetTestCaseDescriptors(List<ITestCase> testCases, bool includeSerialization)
        {
            if (defaultTestCaseDescriptorProvider == null)
                defaultTestCaseDescriptorProvider = new DefaultTestCaseDescriptorProvider(RemoteDiscoverer);

            return defaultTestCaseDescriptorProvider.GetTestCaseDescriptors(testCases, includeSerialization);
        }

        static AssemblyName GetTestFrameworkAssemblyName(string xunitExecutionAssemblyPath)
            => Assembly.Load(new AssemblyName { Name = Path.GetFileNameWithoutExtension(xunitExecutionAssemblyPath), Version = new Version(0, 0, 0, 0) }).GetName();

        /// <inheritdoc/>
        public string Serialize(ITestCase testCase)
            => RemoteDiscoverer.Serialize(testCase);
    }
}
