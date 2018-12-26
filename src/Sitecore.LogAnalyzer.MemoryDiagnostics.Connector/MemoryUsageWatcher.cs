namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector
{
  using System;
  using System.Threading;
  using Assert = Sitecore.LogAnalyzer.Assert;

  public sealed class MemoryUsageWatcher : IDisposable
  {
    #region fields
    protected Action<string> messageOutput;

    protected long _startRAM;

    protected int flag;

    protected string Message;

    protected int ThreshHoldMb;
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="LongRunningOperationWatcher"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="threshHoldMB">The thresh hold ms.</param>
    /// <param name="printer">The printer.</param>
    public MemoryUsageWatcher(string message, int threshHoldMB = 15, Action<string> printer = null)
    {
      Assert.ArgumentNotNullOrEmpty(message, "No message provided");
      this.messageOutput = printer ?? LogAnalyzer.Context.Message;

      this._startRAM = GC.GetTotalMemory(false);
      this.ThreshHoldMb = threshHoldMB;
      this.Message = message;
      Thread.VolatileWrite(ref this.flag, 1);
    }

    public void Dispose()
    {
      this._Dispose(false);
    }

    ~MemoryUsageWatcher()
    {
      this._Dispose(true);
    }

    private void _Dispose(bool isFinalizeCall)
    {
      if (Interlocked.CompareExchange(ref flag, 0, 1) == 0)
      {
        return;
      }
      var msSpend = (GC.GetTotalMemory(false) - _startRAM) / (1024 * 1024);
      if (msSpend >= ThreshHoldMb)
        messageOutput(Message + " used " + msSpend + " MB");

      if (!isFinalizeCall)
        GC.SuppressFinalize(this);

      messageOutput = null;
    }
  }
}
