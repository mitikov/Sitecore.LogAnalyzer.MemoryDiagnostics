using System;
using System.Threading;
using Sitecore.Diagnostics;
using Sitecore.LogAnalyzer;
using Assert = Sitecore.LogAnalyzer.Assert;

namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector
{
  /// <summary>
  /// Outputs message in case operation took more than specified threshhold.
  /// </summary>
  public class LongRunningOperationWatcher:IDisposable
  {
    protected Action<string> messageOutput;
    protected long _startTicks;
    protected int disposeFlag;
    protected string Message;
    protected int ThreshHoldMs;

    /// <summary>
    /// Initializes a new instance of the <see cref="LongRunningOperationWatcher"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="threshHoldMs">The thresh hold ms.</param>
    /// <param name="printer">The printer.</param>
    public LongRunningOperationWatcher(string message,int threshHoldMs = 40, Action<string> printer = null)
    {
      Assert.ArgumentNotNullOrEmpty(message, "No message provided");
      this.messageOutput = printer ?? LogAnalyzer.Context.Message;

      this._startTicks = HighResTimer.GetTick();
      this.ThreshHoldMs = threshHoldMs;
      this.Message = message;

      Thread.VolatileWrite(ref this.disposeFlag, 1);
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="LongRunningOperationWatcher"/> class.
    /// </summary>
    ~LongRunningOperationWatcher()
    {
      this.Dispose(true);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {      
      this.Dispose(false);     
    }

    /// <summary>
    /// Disposes the specified is finalize call.
    /// </summary>
    /// <param name="isFinalizeCall">if set to <c>true</c> [is finalize call].</param>
    private void Dispose(bool isFinalizeCall)
    {
      if (Interlocked.CompareExchange(ref this.disposeFlag, 0, 1) == 0)
      {
        return;
      }

      var msSpend = (int)Math.Floor(HighResTimer.GetMillisecondsSince(this._startTicks));

      if (msSpend >= this.ThreshHoldMs)
      {
        this.messageOutput($"{this.Message} in {msSpend} ms.");
      }

      if (!isFinalizeCall)
      {
        GC.SuppressFinalize(this);
      }

      this.messageOutput = null;
    }
  }
}
