namespace Xunit.Abstractions
{
    /// <summary>
    /// Represents a test assembly and its associated configuration.
    /// </summary>
    public interface IXunitProjectAssembly
    {
        /// <summary>
        /// Gets the assembly filename.
        /// </summary>
        string AssemblyFilename { get; }

        /// <summary>
        /// Gets the config filename.
        /// </summary>
        string ConfigFilename { get; }

        /// <summary>
        /// Gets the configuration values read from the test assembly configuration file.
        /// </summary>
        ITestAssemblyConfiguration Configuration { get; }
    }
}
