namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector
{
  using System;
  using System.Diagnostics;
  using System.Runtime;
  using System.Security;
  using Sitecore.LogAnalyzer.Models;
  using Sitecore.MemoryDiagnostics;
  using Sitecore.MemoryDiagnostics.ModelMetadataInterfaces;
  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
  using SitecoreMemoryInspectionKit.Core.ClrHelpers;

  /// <summary>
  /// Extended <see cref="LogEntry"/> with <see cref="IClrObjMappingModel"/> and <see cref="Parent"/>.
  /// <para>A bridge between Log Analyzer UI, and Memory Dump data models.</para>
  /// </summary>
  [DebuggerDisplay("{Caption}")]
  public class ClrObjLogEntry : LogEntry, IEquatable<ClrObjLogEntry>
  {
    #region Instance fields
    /// <summary>
    /// The model represented by LogEntry
    /// </summary>
    public readonly IClrObjMappingModel Model;

    /// <summary>
    /// TODO: Use during grouping
    /// </summary>
    public ClrObjLogEntry Parent;
    #endregion
    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="ClrObjLogEntry" /> class.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="storage">The storage.</param>
    /// <param name="parent">The parent.</param>
    public ClrObjLogEntry(IClrObjMappingModel model = null, TextStorage storage = null, ClrObjLogEntry parent = null)
      : base(storage)
    {
      Model = model;
      Parent = parent;
      if (Address != default(ulong))
      {
        Level = LogLevel.INFO;
      }
    }
    #endregion

    /// <summary>
    /// Gets the address.
    /// </summary>
    /// <value>
    /// The address.
    /// </value>
    public ulong Address => Model?.Obj.Address ?? ulong.MinValue;

    /// <summary>
    /// Indicates if 
    /// </summary>
    public bool HasMappingModel => Model != null;

    public ClrObject? ClrObject => Model?.Obj;

    #region Public API

    #region Static methods

    public static bool IsNullOrEmptyModel(ClrObjLogEntry entry)
    {
      return (entry == null) || (!entry.HasMappingModel);
    }

    public static bool ModelOfType<T>(ClrObjLogEntry entry)
    {
      if (IsNullOrEmptyModel(entry))
      {
        return false;
      }

      return entry is T;
    }
    #endregion

    /// <summary>
    /// Called after the <see cref="ClrObjLogEntry"/> is created to initiate instance fields from model.
    /// </summary>
    [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute()]
    [SecurityCritical]
    public virtual void InitFldsFromModel()
    {
      try
      {
        if (Model == null)
        {
          Caption = "[NoModel]";
          Text = "[Emtpy Model]";
          LinesCount = 1;
          EventSource = "[No Source]";
          Level = LogLevel.FATAL;
          LogDateTime = DateTime.UtcNow;
          return;
        }

        if (Model is IDateTimeHolder)
        {
          LogDateTime = (Model as IDateTimeHolder).Datetime;
        }
        else
        {
          LogDateTime = DateTime.UtcNow;
        }

        using (new MemoryFailPoint(sizeInMegabytes: 500))
        {
          using (new MemoryUsageWatcher($"{Model.Obj.HexAddress} obj {Model.ModelOfTypeName} model"))
          {
            var modelText = Model.ToString();
            Text = string.IsNullOrEmpty(modelText) ? "[Model yielded No Text]" : modelText.Trim();
          }

          if (Model is ICaptionHolder)
          {
            Caption = (Model as ICaptionHolder).Caption;
          }
          else
          {
            Caption = Text;
          }
        }
      }
      catch (InsufficientMemoryException ex)
      {
        if (Debugger.IsAttached)
        {
          Debugger.Break();
        }

        throw new Exception($"{Model.Obj.HexAddress} not enough memory", ex);
      }
      catch (OutOfMemoryException ex)
      {
        if (Debugger.IsAttached)
        {
          Debugger.Break();
        }

        throw new Exception($"{Model.Obj.HexAddress} not enough memory", ex);
      }

      //if (Model is NoConverterForType)
      //Level =LogLevel.DEBUG;

      EventSource = $"{Model.Obj.HexAddress} [{Model.GetType().Name}]";

      LinesCount = StringUtil.LinesCount(Text);
    }

    #endregion

    #region ToString Override

    public override string ToString() => $"{Caption}{Environment.NewLine}{Text}";

    #endregion

    #region Equals and Hashcode override.
    public bool Equals(ClrObjLogEntry other)
    {
      if (other == null)
      {
        return false;
      }

      return other.Address == Address;
    }

    bool IEquatable<ClrObjLogEntry>.Equals(ClrObjLogEntry other) => Equals(other);

    public override bool Equals(object obj)
    {
      if (obj == null)
      {
        return false;
      }
      var casted = obj as ClrObjLogEntry;
      return casted == null ? base.Equals(obj) : Equals(casted);
    }

    public override int GetHashCode()
    {
      return Address.GetHashCode();
    }
    #endregion

    #region Operators
    public static bool operator ==(ClrObjLogEntry a, ClrObjLogEntry b)
    {
      // If both are null, or both are same instance, return true.
      if (ReferenceEquals(a, b))
      {
        return true;
      }

      if ((a is null) || (b is null))
      {
        return false;
      }

      return a.Address == b.Address;
    }

    public static bool operator !=(ClrObjLogEntry one, ClrObjLogEntry two)
    {
      return !(one == two);
    }

    #endregion
  }
}
