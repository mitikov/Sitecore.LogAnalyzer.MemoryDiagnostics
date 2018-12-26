namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.MemoryDumpSource
{
  using System;
  using System.Collections.Generic;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore;

  /// <summary>
  ///   Provides an enumeration of dump-related information, not <see cref="IEnumerable{T}" /> <see cref="string" />.
  /// </summary>
  [Obsolete("Do not use, please", error: true)]
  public interface IMemoryDumpSource
  {
    /// <summary>
    /// Gets or sets the runtime to get data from.
    /// </summary>
    /// <value>
    /// The runtime.
    /// </value>
    [NotNull]
    ClrRuntime Runtime { get; set; }
  }
}