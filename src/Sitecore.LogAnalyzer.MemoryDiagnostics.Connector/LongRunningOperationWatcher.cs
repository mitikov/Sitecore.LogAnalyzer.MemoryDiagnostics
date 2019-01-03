using System;
using Sitecore.Diagnostics;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector
{
  /// <summary>
  /// Outputs message in case operation took more than specified threshhold.
  /// </summary>
  public class LongRunningOperationWatcher : IDisposable
  {
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="LongRunningOperationWatcher"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="threshHoldMs">The thresh hold ms.</param>
    /// <param name="printer">The printer funtion to output message.</param>
    public LongRunningOperationWatcher(string message, int threshHoldMs = 40, Action<string> printer = null)
    {
      Assert.ArgumentNotNullOrEmpty(message, nameof(message));
      MessagePrinter = printer ?? Context.Message;

      StartTicks = HighResTimer.GetTick();
      ThreshHoldMs = threshHoldMs;
      Message = message;
    }

    protected Action<string> MessagePrinter { get; private set; }

    protected long StartTicks { get; private set; }

    protected string Message { get; private set; }

    protected int ThreshHoldMs { get; private set; }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    void IDisposable.Dispose()
    {
      if (_disposed)
      {
        return;
      }
      _disposed = true;

      var msSpend = (int)Math.Floor(HighResTimer.GetMillisecondsSince(StartTicks));

      if (msSpend >= ThreshHoldMs)
      {
        MessagePrinter($"{Message} in {msSpend} ms.");
      }
    }
  }
}
