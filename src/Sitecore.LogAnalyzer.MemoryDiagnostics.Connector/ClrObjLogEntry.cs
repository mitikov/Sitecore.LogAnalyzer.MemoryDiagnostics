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
  using SitecoreMemoryInspectionKit.Core.AppHelpers;
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

    #region properties
    /// <summary>
    /// Gets the address.
    /// </summary>
    /// <value>
    /// The address.
    /// </value>
    public ulong Address => this.Model?.Obj.Address ?? ulong.MinValue;

    /// <summary>
    /// Indicates if 
    /// </summary>
    public bool HasMappingModel => this.Model != null;

    public ClrObject? ClrObject => this.Model?.Obj;   

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
      this.Model = model;
      this.Parent = parent;
      if (this.Address != default(ulong))
      {
        this.Level = LogLevel.INFO;
      }
    }
    #endregion

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
        if (this.Model == null)
        {
          this.Caption = "[NoModel]";
          this.Text = "[Emtpy Model]";
          this.LinesCount = 1;
          this.EventSource = "[No Source]";
          this.Level = LogLevel.FATAL;
          this.LogDateTime = DateTime.UtcNow;
          return;
        }

        if (this.Model is IDateTimeHolder)
        {
          this.LogDateTime = (this.Model as IDateTimeHolder).Datetime;
        }
        else
        {
          this.LogDateTime = DateTime.UtcNow;
        }

        using (new MemoryFailPoint(sizeInMegabytes: 500))
        {
          using (new MemoryUsageWatcher("{0} obj {1} model".FormatWith(this.Model.Obj.HexAddress, Model.ModelOfTypeName)))
          {
            var modelText = this.Model.ToString();
            this.Text = string.IsNullOrEmpty(modelText) ? "[Model yielded No Text]" : modelText.Trim();
          }

          if (this.Model is ICaptionHolder)
          {
            this.Caption = (this.Model as ICaptionHolder).Caption;
          }
          else
          {
            this.Caption = this.Text;
          }
        }
      }
      catch (InsufficientMemoryException ex)
      {
        if (Debugger.IsAttached)
        {
          Debugger.Break();
        }

        throw new Exception("{0} not enough memory".FormatWith(this.Model.Obj.HexAddress), ex);
      }
      catch (OutOfMemoryException ex)
      {
        if (Debugger.IsAttached)
        {
          Debugger.Break();
        }

        throw new Exception("{0} not enough memory".FormatWith(this.Model.Obj.HexAddress), ex);
      }

      //if (Model is NoConverterForType)
      //this.Level =LogLevel.DEBUG;

      this.EventSource = this.Model.Obj.Address.ToString("X") + " [" + this.Model.GetType().Name + "]";

      this.LinesCount = StringUtil.LinesCount(this.Text);
    }

    #endregion

    #region ToString Override

    public override string ToString()
    {
      return this.Caption + Environment.NewLine + this.Text;
    }

    #endregion

    #region Equals and Hashcode override.
    public bool Equals(ClrObjLogEntry other)
    {
      if (other == null)
      {
        return false;
      }

      return other.Address == this.Address;
    }

    bool IEquatable<ClrObjLogEntry>.Equals(ClrObjLogEntry other)
    {
      return this.Equals(other);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
      {
        return false;
      }
      var casted = obj as ClrObjLogEntry;
      return casted == null ? base.Equals(obj) : this.Equals(casted);
    }

    public override int GetHashCode()
    {
      return this.Address.GetHashCode();
    }
    #endregion

    #region Operators
    public static bool operator ==(ClrObjLogEntry a, ClrObjLogEntry b)
    {
      // If both are null, or both are same instance, return true.
      if (object.ReferenceEquals(a, b))
      {
        return true;
      }

      if (((object)a == null) || ((object)b == null))
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
