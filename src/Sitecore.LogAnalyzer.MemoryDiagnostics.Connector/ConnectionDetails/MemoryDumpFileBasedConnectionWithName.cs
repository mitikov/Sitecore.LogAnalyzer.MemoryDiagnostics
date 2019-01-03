namespace Sitecore.MemoryDiagnostics.ConnectionDetails
{
  using Interfaces;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.Connector.ConnectionDetails;

  /// <summary>
  /// File-base memory dump connection with additional <see cref="string"/>.
  /// </summary>
  /// <seealso cref="INameProvider" />  
  public class MemoryDumpFileBasedConnectionWithName : MemoryDumpConnectionDetails, INameProvider
  {
    #region fields

    private string name;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryDumpFileBasedConnectionWithName"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="pathToDump">The path to dump.</param>
    /// <param name="pathToMsCorDacwks">The path to mscord.</param>
    public MemoryDumpFileBasedConnectionWithName(string name, string pathToDump, string pathToMsCorDacwks) : base(pathToDump, pathToMsCorDacwks)
    {
      name = name;
    }

    /// <summary>
    /// Gets the connection details string to be shown in SCLA.
    /// </summary>
    /// <value>
    /// The connection details string.
    /// </value>
    public override string ConnectionDetailsString => $"{Name}|{base.ConnectionDetailsString}";

    /// <summary>
    /// Gets or sets the name carried by the instance.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    [NotNull]
    public virtual string Name
    {
      get
      {
        return name;
      }

      set
      {
        name = value;
      }
    }
  }
}