namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector
{
  using System;
  using Assert = Sitecore.LogAnalyzer.Assert;

  public sealed class MemoryUsageWatcher : IDisposable
  {
    #region fields
    private Action<string> messageOutput;

    private readonly long _startRAM;

    private bool disposed;

    private readonly string _message;

    private readonly int _threshHoldMegabytes;
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="LongRunningOperationWatcher"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="threshHoldMB">The thresh hold ms.</param>
    /// <param name="printer">The printer.</param>
    public MemoryUsageWatcher(string message, int threshHoldMB = 15, Action<string> printer = null)
    {
      Assert.ArgumentNotNullOrEmpty(message, nameof(message));

      messageOutput = printer ?? Context.Message;

      _startRAM = GC.GetTotalMemory(forceFullCollection: false);
      _threshHoldMegabytes = threshHoldMB;
      _message = message;

    }

    void IDisposable.Dispose()
    {
      if (disposed)
      {
        return;
      }

      disposed = true;

      var megabytes = (GC.GetTotalMemory(false) - _startRAM) / (1024 * 1024);
      if (megabytes >= _threshHoldMegabytes)
        messageOutput($"{_message} used {megabytes} MB");
    }
  }
}
