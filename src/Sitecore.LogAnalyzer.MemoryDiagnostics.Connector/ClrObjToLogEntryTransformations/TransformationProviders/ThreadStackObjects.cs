namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ClrObjToLogEntryTransformations.TransformationProviders
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime;

  using Interfaces;
  using Microsoft.Diagnostics.Runtime;
  using Sitecore.LogAnalyzer;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.MemoryDiagnostics.ClrThreadTransformators;
  using Sitecore.MemoryDiagnostics.Interfaces;
  using Sitecore.MemoryDiagnostics.ModelFactory.Abstracts;
  using Sitecore.MemoryDiagnostics.ModelFilters;
  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
  using Sitecore.MemoryDiagnostics.Models.FallBack;
  using Sitecore.MemoryDiagnostics.Models.InternalProcessing;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;
  using SitecoreMemoryInspectionKit.Core.Switchers;

  /// <summary>
  /// Expects <see cref="ClrObject"/> address same as thead address via <see cref="ClrRuntime.Threads"/> candidate, and would get thread stack objects as nested.
  /// <para>Would init <see cref="ThreadMappingModel"/> via hands, so use <see cref="IInitLogEntryFields"/> to apply custom formatting</para>
  /// <para> Gets the thread stack objects by treating  as <see cref="ThreadMappingModel"/> 
  /// </para>
  /// </summary>
  public class ExtractThreadStaticObjects : ClrObjectTranformProvider, IClrThreadTransformator
  {
    #region Constructors
    public ExtractThreadStaticObjects(IModelMapperFactory modelMapperFactory, 
      IInitLogEntryFields logEntryFieldsInitializer, IModelMappingFilter filter, IFilter<ClrObject> stackObjectFilter) : base(modelMapperFactory, logEntryFieldsInitializer, filter)
    {
      Assert.ArgumentNotNull(stackObjectFilter, nameof(stackObjectFilter));

      StackObjectFilter = stackObjectFilter;
    }

    #endregion

    #region Properties
    protected IFilter<ClrObject> StackObjectFilter { get; private set; }
    #endregion

    public virtual LogEntry BuildCandidate([NotNull] ClrRuntime clrRuntime, ClrThread thread, out LogEntry[] nested)
    {
      
      if (thread == null)
      {
        nested = null;
        return null;
      }

      var threadMappingModel = BuildThreadModel(clrRuntime, thread);

      using (new HashtableEnumerationLimitContext(100))
      {
        ClrObjLogEntry threadLogEntryModel = BuildLogEntry(threadMappingModel);

        nested = GetNestedObjects(clrRuntime, threadMappingModel.Obj, threadMappingModel, threadLogEntryModel);

        if (nested != null)
        {
          GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
          foreach (var logEntry in nested.OfType<ClrObjLogEntry>())
          {
            LogEntryFieldsInitializer.ApplyCustomLogicOnLogEntry(logEntry);
          }
        }

        return threadLogEntryModel;
      }
    }

    public override LogEntry BuildCandidate(ClrRuntime clrRuntime, ClrObject clrTheadObject, out LogEntry[] nested)
    {
      var thread = (from matchingThread in clrRuntime.Threads
                    where matchingThread.Address.Equals(clrTheadObject.Address)
                    select matchingThread).FirstOrDefault();

      return BuildCandidate(clrRuntime, thread, out nested);
    }

    protected virtual ClrObjLogEntry BuildLogEntry(ThreadMappingModel threadMappingModel)
    {
      var thread = threadMappingModel.Thread;

      var threadLogEntryModel = new ClrObjLogEntry(threadMappingModel, Storage)
      {
        Caption = "Thread #" + thread.OSThreadId.ToString("####")
      };

      threadLogEntryModel.InitFldsFromModel();
      threadLogEntryModel.Text = threadMappingModel.StackTrace;

      LogEntryFieldsInitializer.ApplyCustomLogicOnLogEntry(threadLogEntryModel);

      return threadLogEntryModel;
    }

    /// <summary>
    /// Gets the thread stack objects by treating <paramref name="mapping"/> as <see cref="ThreadMappingModel"/>.
    /// <para>Uses <see cref="IModelMapperFactory"/> base class field to treat </para>
    /// </summary>
    /// <param name="clrRuntime">The runtime</param>
    /// <param name="clrObject">The clrObject.</param>
    /// <param name="mapping">The mapping.</param>
    /// <param name="parentEntry">The parent model.</param>
    /// <returns>Null if <paramref name="mapping"/> is not of <see cref="ThreadMappingModel"/>, or without thread.</returns>
    [CanBeNull]
    public override LogEntry[] GetNestedObjects([NotNull]ClrRuntime clrRuntime, ClrObject clrObject, IClrObjMappingModel mapping, ClrObjLogEntry parentEntry)
    {
      var casted = mapping as ThreadMappingModel;
      if ((casted == null) || (casted.Thread == null))
      {
        return null;
      }

      var nested = FetchThreadStackObjects(clrRuntime, casted.Thread);

      return ProcessNested(nested, parentEntry);
    }

    protected virtual ICollection<ClrObject> FetchThreadStackObjects(ClrRuntime clrRuntime, [NotNull] ClrThread thread)
    {
      var heap = clrRuntime.Heap;
      var nested = new HashSet<ClrObject>();      
      foreach (var stackObj in thread.EnumerateStackObjects(includePossiblyDead: false))
      {
        ulong tmpAddress = stackObj.Object;

        var tmpType = heap.GetObjectType(tmpAddress);
        if (tmpType == null)
        {
          continue;
        }

        var clrob = new ClrObject(tmpAddress, tmpType);
        if (!nested.Add(clrob))
        {
          continue;
        }
        clrob.ReEvaluateType();

        if (clrob.IsNullObj || clrob.Type.IsArray)
        {
          continue;
        }
      }
      return nested;
    }

    [CanBeNull]
    protected virtual ThreadMappingModel BuildThreadModel([NotNull]ClrRuntime clrRuntime, [CanBeNull]ClrThread thread)
    {
      if (thread == null)
      {
        return null;
      }

      var threadMappingModel = new ThreadMappingModel
      {
        Obj = new ClrObject(thread.Address, clrRuntime.Heap),
        Thread = thread,
        m_Name = thread.OSThreadId.ToString()
      };

      return threadMappingModel;
    }

    protected virtual LogEntry[] ProcessNested([NotNull]ICollection<ClrObject> nested, [NotNull]ClrObjLogEntry parentEntry)
    {
      var result = new List<LogEntry>(nested.Count);
      foreach (var candidate in nested.Where(StackObjectFilter.Matches))
      {
        IClrObjMappingModel model;
        if (candidate.Type == null)
        {
          continue;
        }

        using (new LongRunningOperationWatcher($"{candidate.HexAddress} ({candidate.Type.Name}) processed by ModelMapperFactory"))
        {
          model = ModelMapperFactory.BuildModel(candidate);
        }

        if (model is NoConverterForType)
        {
          continue;
        }

        var entry = new ClrObjLogEntry(model, Storage, parentEntry);
        try
        {
          entry.InitFldsFromModel();
          entry.Caption = entry.Caption;
          LogEntryFieldsInitializer.ApplyCustomLogicOnLogEntry(entry);
        }
        catch (Exception ex)
        {
          entry.Caption = $"En Error occurred for {model.Obj.HexAddress}";
        }

        result.Add(entry);
      }
      return result.ToArray();
    }

  }
}
