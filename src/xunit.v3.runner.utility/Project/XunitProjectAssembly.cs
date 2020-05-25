﻿using Xunit.Abstractions;

namespace Xunit
{
    /// <summary>
    /// Represents an assembly in an <see cref="XunitProject"/>.
    /// </summary>
    public class XunitProjectAssembly : IXunitProjectAssembly
    {
        TestAssemblyConfiguration configuration;

        /// <inheritdoc />
        public string AssemblyFilename { get; set; }

        /// <inheritdoc />
        public string ConfigFilename { get; set; }

        /// <inheritdoc />
        public TestAssemblyConfiguration Configuration
        {
            get
            {
                if (configuration == null)
                    configuration = ConfigReader.Load(AssemblyFilename, ConfigFilename);

                return configuration;
            }
        }

        ITestAssemblyConfiguration IXunitProjectAssembly.Configuration => Configuration;
    }
}
