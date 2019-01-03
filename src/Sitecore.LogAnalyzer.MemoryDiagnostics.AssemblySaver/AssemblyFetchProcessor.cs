namespace Sitecore.LogAnalyzer.MemoryDiagnostics.AssemblySaver
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using ManagedDumpAssembliesFetcher;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer;
  using Sitecore.LogAnalyzer.Attributes;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.MemoryDiagnostics;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.Facades.ObjectEnumeration;
  

  public class AssemblyFetchProcessor : DiDumpProcessor
  {
    public AssemblyFetchProcessor(ILogAnalyzerFacade logAnalyzerFacade) : base(logAnalyzerFacade, new MdConnectionNoEnumerator(), new EmptyClrObjectTransformator())
    {
    }

    /// <summary>
    /// Builds the parsing result wrapped in try-catch, so must raise exceptions.
    /// </summary>
    /// <param name="clrRuntime">The data source.</param>
    /// <param name="processContext">The process context.</param>
    /// <returns></returns>
    [NotNull]
    protected override ParsingResult BuildParsingResult([NotNull] ClrRuntime clrRuntime, [NotNull] LogProcessorSettings processContext)
    {
      Assert.ArgumentCorrectType(processContext.ConnectionSettings, typeof(AssemblyFetchConnection), "connection details");

      var connection = (AssemblyFetchConnection)processContext.ConnectionSettings;
      var outputPath = connection.FolderToSave;

      var includeDotNet = connection.IncludeDotNet;

      Directory.CreateDirectory(outputPath);

      var correctModels = new List<LogEntry>();

      foreach (var clrModule in FilterFileBased(clrRuntime.Modules))
      {
        var fileName = Path.GetFileName(clrModule.FileName);
        if (!includeDotNet && (IsDotNetFrameworkAssembly(clrModule)))
        {
          continue;
        }
        LogEntry entry = BuildLogEntryFromModule(clrModule, fileName);

        if (!processContext.ReaderSettings.Matches(entry))
        {
          continue;
        }

        try
        {
          var fetcher = new ModuleFetcher(clrModule);

          fetcher.FetchToFile(outputPath);

          Context.Message($"{clrModule.AssemblyName} fetched.");
        }
        catch (Exception ex)
        {
          // TODO: Create exception handling.
          entry.Level = LogLevel.ERROR;
          Sitecore.LogAnalyzer.Context.Error($"{clrModule.AssemblyName} failed.", ex);
        }

        correctModels.Add(entry);
      }

      var parsingResult = new ParsingResult();

      parsingResult.AddToGroupAll(correctModels);

      return parsingResult;
    }

    protected virtual LogEntry BuildLogEntryFromModule([NotNull]ClrModule clrModule, [NotNull] string fileName)
    {
      return new LogEntry
      {
        Text = $"{clrModule.AssemblyName}{Environment.NewLine}Size: {StringUtil.GetSizeString(clrModule.Size)} ",
        Caption = fileName,
        LinesCount = 3,
        EventSource = clrModule.MetadataAddress.ToString("x"),
        LogDateTime = DateTime.Today - TimeSpan.FromMilliseconds(clrModule.Size),
        Level = LogLevel.INFO
      };
    }

    protected virtual bool IsDotNetFrameworkAssembly([NotNull]ClrModule clrModule)
    {
      return clrModule.Name.IndexOf(@"\Windows\Microsoft.Net\assembly\gac", StringComparison.OrdinalIgnoreCase) > 0;      
    }

    /// <summary>
    /// Filters only file-based modules, that are valid to be saved.
    /// </summary>
    /// <param name="allModules">A stream of all modules to be filtered.</param>
    /// <returns></returns>
    protected virtual IEnumerable<ClrModule> FilterFileBased([NotNull]IEnumerable<ClrModule> allModules)
    {
      return allModules.Where(module => module.IsFile && !module.IsDynamic);
    }
  }
}
