namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration
{
  using System.Collections.Generic;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.MemoryDiagnostics.ClrObjectEnumerators;
  using Sitecore.MemoryDiagnostics.ClrObjectsProviders.Base;
  using Sitecore.MemoryDiagnostics.Facades.ObjectEnumeration;
  using Sitecore.MemoryDiagnostics.SourceFactories;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ConnectionDetails;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// Uses <see cref="IClrRuntimeFactory"/> and <see cref="IObjectEnumerationFacade"/> to build runtime, and enumerate objects.
  /// </summary>
  /// <para>Implements <seealso cref="IEnumeratiorConnection" /></para>
  public class BaseObjectEnumerationConnectionDetails : IEnumeratiorConnection
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseObjectEnumerationConnectionDetails"/> class.
    /// </summary>
    /// <param name="runtimeFactory">The runtime factory.</param>
    /// <param name="objectEnumeration">The object enumeration.</param>
    public BaseObjectEnumerationConnectionDetails(IClrRuntimeFactory runtimeFactory, IObjectEnumerationFacade objectEnumeration)
    {
      RuntimeFactory = runtimeFactory;
      ClrObjectEnumerator = objectEnumeration.ClrObjectEnumerator;
      FilteredObjectsProvider = objectEnumeration.FilteredObjectsProvider;
    }

    public virtual IEnumerateClrObjectsFromClrRuntime ClrObjectEnumerator { get; }

    public virtual IFilteredObjectsProvider FilteredObjectsProvider { get; }

    public virtual IClrRuntimeFactory RuntimeFactory { get; }

    public IEnumerable<ClrObject> ExtractFromRuntime(ClrRuntime ds)
    {
      return FilteredObjectsProvider.ExtractFromRuntime(ds, ClrObjectEnumerator);
    }

    /// <summary>
    /// Extracts from connection details.
    /// </summary>
    /// <param name="connectionDetailsWithName">The connection details.</param>
    /// <returns>Filtered <see cref="ClrObject"/>s from connection details.</returns>
    public IEnumerable<ClrObject> ExtractFromConnectionDetails(MemoryDumpConnectionDetails connectionDetailsWithName)
    {
      var runtime = RuntimeFactory.BuildClrRuntime(connectionDetailsWithName);
      return ExtractFromRuntime(runtime);
    }
  }
}