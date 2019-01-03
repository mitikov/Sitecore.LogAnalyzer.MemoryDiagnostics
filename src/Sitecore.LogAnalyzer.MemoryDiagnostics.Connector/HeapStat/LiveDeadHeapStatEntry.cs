namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.HeapStat
{
  using System;
  using System.Text;

  /// <summary>
  ///   Carries per-type stats of dead/live object instances.
  /// </summary>
  /// <seealso cref="HeapStatEntry" />
  public class LiveDeadHeapStatEntry : HeapStatEntry
  {
    #region Fields
    /// <summary>
    ///   The total count of live instances.
    /// </summary>
    public int LiveCount;

    /// <summary>
    ///   The total size of live objects in bytes.
    /// </summary>
    public ulong LiveSize;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveDeadHeapStatEntry"/> class.
    /// </summary>
    /// <param name="entry">The entry.</param>
    public LiveDeadHeapStatEntry(HeapStatEntry entry) : base(entry.Name)
    {
      Count = entry.Count;
      Size = entry.Size;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveDeadHeapStatEntry"/> class.
    /// </summary>
    /// <param name="typeName">Name of the type.</param>
    public LiveDeadHeapStatEntry(string typeName) : base(typeName)
    {
    }

    #endregion

    #region Properties
    /// <summary>
    /// Gets the alive percent.
    /// </summary>
    /// <value>
    /// The alive percent.
    /// </value>
    public virtual float AlivePercent => Count == 0 ? 0 : (float)LiveCount / Count;

    /// <summary>
    /// Gets the alive percent text.
    /// </summary>
    /// <value>
    /// The alive percent text.
    /// </value>
    public virtual string AlivePercentText => AlivePercent.ToString("P");

    /// <summary>
    /// A brief instance description
    /// </summary>
    public override string Caption =>
      $"{Sitecore.MemoryDiagnostics.StringUtil.GetSizeString(DeadSize)} [{DeadCount}] dead {Sitecore.MemoryDiagnostics.StringUtil.StripNamespaceFromTypeFullName(Name)}";

    /// <summary>
    /// Gets the <see cref="DateTime" /> ( for sorting purposes ).
    /// </summary>
    /// <value>
    /// The DateTime.
    /// </value>
    public override DateTime Datetime => DateTime.Today.AddMilliseconds(-((long)DeadSize));

    /// <summary>
    /// Gets the number of dead objects.
    /// </summary>
    /// <value>
    /// The dead count.
    /// </value>
    public int DeadCount => Count - LiveCount;

    /// <summary>
    /// Gets the size of dead objects.
    /// </summary>
    /// <value>
    /// The size of the dead.
    /// </value>
    public ulong DeadSize => Size - LiveSize;
    #endregion

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.AppendLine($"{Name} alive: {AlivePercentText}")
        .AppendLine($"Total objects: {Count} Size: {Sitecore.MemoryDiagnostics.StringUtil.GetSizeString(Size)}")
        .AppendLine($"Live objects: {LiveCount} Size: {Sitecore.MemoryDiagnostics.StringUtil.GetSizeString(LiveSize)}")
        .AppendLine($"Dead objects: {DeadCount} Size: {Sitecore.MemoryDiagnostics.StringUtil.GetSizeString(DeadSize)}");

      return sb.ToString();
    }
  }
}