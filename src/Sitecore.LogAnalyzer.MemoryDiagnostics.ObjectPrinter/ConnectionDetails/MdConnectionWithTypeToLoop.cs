using System;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ModelViewer.ConnectionDetails
{
  using Sitecore.LogAnalyzer;
  using Sitecore.LogAnalyzer.Attributes;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ConnectionDetails;

  /// <summary>
  /// Extends <see cref="MemoryDumpConnectionDetails"/> with <see cref="Type"/> property.
  /// <para>Sample usage: locate only models of given type in memory.</para>
  /// </summary>
  public class MemoryDumpConnectionWithType : MemoryDumpConnectionDetails
  {
    public readonly Type ModelsType;

    public MemoryDumpConnectionWithType(string pathToDump, string pathToMscord, [NotNull] Type modelMappingType) : base(pathToDump, pathToMscord)
    {
      Assert.ArgumentNotNull(modelMappingType, nameof(modelMappingType));
      ModelsType = modelMappingType;
    }
  }
}
