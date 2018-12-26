namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration
{
  using System.Collections.Generic;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer.Attributes;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using Sitecore.MemoryDiagnostics.SourceFactories;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ConnectionDetails;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// Extracts <see cref="ClrRuntime" /> from <see cref="MemoryDumpConnectionDetailsWithName" />, and enumerates objects from <see cref="IObjectEnumerationFacade"/>.
  /// </summary>
  /// <para>Implements <seealso cref="IObjectEnumerationFacade" /></para>
  public interface IEnumeratiorConnection : IObjectEnumerationFacade
  {
    /// <summary>
    /// Gets the runtime factory.
    /// </summary>
    /// <value>
    /// The runtime factory.
    /// </value>
    IClrRuntimeFactory RuntimeFactory { get; }

    /// <summary>
    /// Extracts enumeration of objects from given connection details.
    /// </summary>
    /// <param name="connectionDetailsWithName">The connection details.</param>
    /// <returns>An enumeration of objects from <see cref="MemoryDumpConnectionDetails"/>.</returns>
    IEnumerable<ClrObject> ExtractFromConnectionDetails([NotNull] MemoryDumpConnectionDetails connectionDetailsWithName);
  }
}