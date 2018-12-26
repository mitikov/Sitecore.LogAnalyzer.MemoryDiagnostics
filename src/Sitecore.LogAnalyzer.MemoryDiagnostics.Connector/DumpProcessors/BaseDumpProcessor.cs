using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.DumpProcessors
{
  using Sitecore.MemoryDiagnostics.SourceFactories;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.Exceptions;
  using Sitecore.LogAnalyzer;
  using Sitecore.LogAnalyzer.Attributes;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.LogAnalyzer.Parsing;
  using Sitecore.LogAnalyzer.Settings;
  using Sitecore.LogAnalyzer.States;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ConnectionDetails;

  public abstract class BaseDumpProcessor : ILogProcessor
  {
    #region ILogProcessor implementation
    /// <summary>
    ///     Reads data from <see cref="ProcessContext.Settings" />, and outputs <see cref="ProcessContext.Result" />.
    /// <para>Performs an operation in dedicated thread on case processing context is Async.</para>
    /// </summary>
    /// <param name="processingContext">The processing context.</param>
    void ILogProcessor.StartAnalyzing([NotNull] ProcessContext processingContext)
    {
      this.Initialize();
      Sitecore.Diagnostics.Assert.ArgumentNotNull(processingContext, "processingContext");

      if (processingContext.Async)
      {
        Task.Factory.StartNew(() => this.ProcessWrapped(processingContext));
      }
      else
      {
        this.ProcessWrapped(processingContext);
      }
    }
    #endregion

    /// <summary>
    /// Initializes this instance before actual job is done.
    /// <para>Placeholder for initialization.</para>
    /// </summary>
    protected virtual void Initialize()
    {
    }

    /// <summary>
    ///   Builds <see cref="ClrRuntime" /> from provided in <paramref name="context" /> connection details, gets
    ///   <see cref="ParsingResult" />, reorganizes it by <see cref="LogLevel" />, extracts captions and returns resulting
    ///   <see cref="GeneralContext" />
    ///   <para>Code flow</para>
    ///   <para>
    ///     1. Extracts <see cref="ClrRuntime" /> via <see cref="BuildRuntime" /> method and
    ///     <see cref="ProcessContext.Settings" /> connection string.
    ///   </para>
    ///   <para>
    ///     2. Invokes <see cref="BuildParsingResult" /> with <see cref="ClrRuntime" />, and
    ///     <see cref="ProcessContext.Settings" />
    ///   </para>
    ///   <para>
    ///     3. <see cref="ReorganizeParsingResult" /> reorganizes results by <see cref="LogLevel" />,
    ///     <see cref="LogEntry.LogDateTime" /> and so on.
    ///   </para>
    ///   <para>4. <see cref="BuildCaptions" /> using reorganized results</para>
    ///   <para>
    ///     5. <see cref="BuildGeneralContext" /> using <see cref="LogGroupsCollection" /> and <see cref="ParsingResult" />
    ///   </para>
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    ///   The <see cref="GeneralContext" />.
    /// </returns>
    protected virtual GeneralContext DoProcessing(ProcessContext context)
    {
      Sitecore.Diagnostics.Assert.IsNotNull(context.Settings, "processingContext.Settings");
      Sitecore.Diagnostics.Assert.IsNotNull(context.Settings.ConnectionSettings, "Connection");

      var connection = context.Settings.ConnectionSettings as MemoryDumpConnectionDetails;      

      var clrRuntime = this.BuildRuntime(connection);

      this.OnPreBuildParsingResult(clrRuntime, context);

      var parsingResult = this.BuildParsingResult(clrRuntime, context.Settings);

      this.ReorganizeParsingResult(parsingResult);

      var captions = this.BuildCaptions(parsingResult);

      var result = this.BuildGeneralContext(parsingResult, captions);

      return result;
    }

    /// <summary>
    /// Called before <see cref="BuildParsingResult"/>.
    /// </summary>
    /// <param name="clrRuntime">The runtime.</param>
    /// <param name="context">The context.</param>
    protected virtual void OnPreBuildParsingResult(ClrRuntime clrRuntime, ProcessContext context)
    {
    }

    /// <summary>
    ///   Builds the <see cref="ClrRuntime" /> from provided <see cref="MemoryDumpConnectionDetails" /> connection details.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <returns>Initialized <see cref="ClrRuntime"/> instance from provided connection details.</returns>
    /// <exception cref="RequiredObjectIsNullException">
    ///   in case <see cref="IClrRuntimeFactory" /> attempted to return null
    ///   <see cref="ClrRuntime" />.
    /// </exception>
    protected abstract ClrRuntime BuildRuntime([NotNull] MemoryDumpConnectionDetails connection);

    /// <summary>
    ///   Builds the parsing result wrapped in try-catch, so must raise exceptions.
    /// </summary>
    /// <param name="clrRuntime">The data source.</param>
    /// <param name="processContext">The process context.</param>
    /// <returns></returns>    
    protected abstract ParsingResult BuildParsingResult([NotNull] ClrRuntime clrRuntime, [NotNull] LogProcessorSettings processContext);

    /// <summary>
    ///   Reorganizes the parsing result by
    ///   <para>Sorting all entries by <see cref="LogEntry.LogDateTime" /></para>
    ///   <para>
    ///     Reorganizes <see cref="ParsingResult.All" /> by appropriate <see cref="LogLevel" /> into
    ///     <see cref="ParsingResult.Infos" />, <see cref="ParsingResult.Warns" />, and so on.
    ///   </para>
    ///   <para>indexes <see cref="ParsingResult.All" /> entries by giving sequential numbers.</para>
    /// </summary>
    /// <param name="parsingResult">The parsing result.</param>
    protected abstract void ReorganizeParsingResult([NotNull] ParsingResult parsingResult);

    /// <summary>
    ///   Builds the captions to be shown in UI (wrapped in try-catch).
    /// </summary>
    /// <param name="parsingResult">The parsing result.</param>
    /// <returns>A collection of groups per LogLevel.</returns>
    [NotNull]
    protected abstract LogGroupsCollection BuildCaptions([NotNull] ParsingResult parsingResult);

    protected abstract GeneralContext BuildGeneralContext(ParsingResult parsingResult, LogGroupsCollection captions);

    /// <summary>
    ///   <para>
    ///     Wraps Processing logic into try-catch block, if an exception has happened
    ///     <see cref="ProcessContext.SetError" /> is called.
    ///   </para>
    ///   <para>
    ///     1. Calls <see cref="ProcessContext.SetProcessingStarted" /> and logs a message via
    ///     <see cref="Context.Message(string,object[])" /> indicating processing has started.
    ///   </para>
    ///   <para>2. MAIN LOGIC: Executes <see cref="DoProcessing" /> operation inside try-catch block. </para>
    ///   <para>3. After processing is finished, <see cref="ProcessContext.SetResult" /> is called.</para>
    /// </summary>
    /// <param name="processingContext">The processing context.</param>
    private void ProcessWrapped([NotNull] ProcessContext processingContext)
    {
      Sitecore.Diagnostics.Assert.ArgumentNotNull(processingContext, "processingContext");
      try
      {
        Context.Message("Processing has started. Please wait");
        processingContext.SetProcessingStarted();

        var resultingGeneralContext = this.DoProcessing(processingContext);
        Context.Message("Processing has finished.");
        processingContext.SetResult(resultingGeneralContext);
      }
      catch (Exception ex)
      {
        processingContext.SetError("Error during processing", ex);
        Context.Error("Error during processing", ex);
      }
    }
  }
}
