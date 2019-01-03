namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.HeapStat
{
  using System;
  using Sitecore.MemoryDiagnostics;
  using Sitecore.MemoryDiagnostics.ModelMetadataInterfaces;
  using Sitecore;
  using Sitecore.Diagnostics;

  /// <summary>
  /// Represents per-type statistics of object instances in heap.
  /// </summary>
  public class HeapStatEntry : ICaptionHolder, IDateTimeHolder
  {                                
    #region Fields
    /// <summary>
    /// The name of the type carried by this object instance.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// The count of objects of the type met.
    /// </summary>
    public int Count;

    /// <summary>
    /// The total size occupied by the type.
    /// </summary>
    public ulong Size;

    private readonly int hash;

    #endregion

    #region Constructors
    public HeapStatEntry([NotNull] string typeName)
    {
      Assert.ArgumentNotNullOrEmpty(typeName, "typeName");
      Count = 0;
      Size = 0;
      Name = typeName;
      hash = Name.GetHashCode();
    }

    #endregion
    #region Properties
    /// <summary>
    /// A brief instance description
    /// </summary>
    public virtual string Caption => $"{Sitecore.MemoryDiagnostics.StringUtil.StripNamespaceFromTypeFullName(Name)} hits {Count}";

    /// <summary>
    /// Gets the <see cref="DateTime"/> ( for sorting purposes ).
    /// </summary>
    /// <value>
    /// The datetime.
    /// </value>
    public virtual DateTime Datetime => DateTime.Today.AddSeconds(Count);
    #endregion

    #region Object overrided methods
    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public override int GetHashCode() => hash;

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString() => $"{Sitecore.MemoryDiagnostics.StringUtil.GetSizeString(Size),4:n0} {Count,4:0} {Name}";   
    #endregion
  }
}