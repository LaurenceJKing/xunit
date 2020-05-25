﻿#if NETSTANDARD

using System;
using Xunit.Abstractions;

namespace Xunit
{
    /// <summary>
    /// This class provides assistance with assembly resolution for missing assemblies.
    /// </summary>
    static class AssemblyHelper
    {
        /// <summary>
        /// Subscribes to the appropriate assembly resolution event, to provide automatic assembly resolution for
        /// an assembly and any of its dependencies.
        /// </summary>
        /// <returns>An object which, when disposed, un-subscribes.</returns>
        public static IDisposable SubscribeResolveForAssembly(string assemblyFileName, IMessageSink internalDiagnosticsMessageSink = null)
            => null;

        /// <summary>
        /// Subscribes to the appropriate assembly resolution event, to provide automatic assembly resolution for
        /// an assembly and any of its dependencies.
        /// </summary>
        /// <returns>An object which, when disposed, un-subscribes.</returns>
        public static IDisposable SubscribeResolveForAssembly(Type typeInAssembly, IMessageSink internalDiagnosticsMessageSink = null)
            => null;
    }
}

#endif