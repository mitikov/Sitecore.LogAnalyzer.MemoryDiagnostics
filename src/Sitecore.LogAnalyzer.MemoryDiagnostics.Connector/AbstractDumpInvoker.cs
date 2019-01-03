namespace Sitecore.LogAnalyzer.MemoryDiagnostics.Connector
{
  using System;
  using System.Drawing;
  using System.Windows.Forms;
  using Sitecore.DumpModule.Common.UI;
  using Sitecore.LogAnalyzer;
  using Sitecore.LogAnalyzer.Presentation.Invokers;
  using Sitecore.LogAnalyzer.Settings;

  /// <summary>
  /// Base invoker with image and <see cref="PickDumpDetails"/> form to pick MD file.
  /// <para>Derived classes must implement <see cref="Name"/> property.</para>
  /// <para>Modules are registered in Sitecore.config and picked by <see cref="Name"/>, so it is important that each custom module has matching name here and in config.</para>
  /// </summary>
  /// <seealso cref="IVisibleSourceInvoker" />
  public abstract class AbstractDumpInvoker : IVisibleSourceInvoker
  {
    /// <summary>
    /// The pick dump details form.
    /// <para>Constructs <see cref="MDFileConnection"/> - carries path to snapshot and mscord.</para>
    /// <para>In other words - all the data you need to load ClrRuntime.</para>
    /// </summary>
    protected PickDumpDetails PickDumpDetailsForm = new PickDumpDetails();

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractDumpInvoker"/> class.
    /// </summary>
    protected AbstractDumpInvoker() => Image = Resources.Image;

    /// <summary>
    /// Occurs when [cancel].
    /// </summary>
    public event EventHandler Cancel;

    public event EventHandler<EventArgs<Tuple<DateTime, DateTime>>> DateFilterChanged;

    public event EventHandler<EventArgs<IConnectionSettings>> Done;

    public event EventHandler Load;

    /// <summary>
    /// Gets the image to be shown in module picker form.
    /// </summary>
    /// <value>
    /// The image.
    /// </value>
    public virtual Image Image { get; }

    /// <summary>
    /// Gets the name of the represented module.
    /// <para>Binds this module code with configuration.</para>
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public abstract string Name { get; }

    /// <summary>
    /// Invokes this module.
    /// <para>Shows <see cref="PickDumpDetailsForm"/>, and takes connection specified there.</para>
    /// <para>Sets lower, and upper <see cref="DateTime"/> with 1 year range from now.</para>
    /// </summary>
    public virtual void Invoke()
    {
      if (PickDumpDetailsForm.ShowDialog() == DialogResult.OK)
      {
        var connection = PickDumpDetailsForm.FileConnection;

        var lower = DateTime.UtcNow.AddYears(-1);
        var upper = DateTime.UtcNow.AddYears(+1);
        var lowerUpperDateTimePair = new Tuple<DateTime, DateTime>(lower, upper);

        OnDateFilterChanged(new EventArgs<Tuple<DateTime, DateTime>>(lowerUpperDateTimePair));

        OnDone(new EventArgs<IConnectionSettings>(connection));
      }
      else
      {
        OnCancel();
      }
    }

    /// <summary>
    /// Called when module form was shown, but either nothing was selected, or pick cancelled.
    /// </summary>
    protected virtual void OnCancel()
    {
      Cancel?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Raises the <see cref="E:DateFilterChanged"/> event to indicate lower-upper <see cref="DateTime"/> boundry changed.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs{Tuple{DateTime, DateTime}}"/> instance containing the event data.</param>
    protected virtual void OnDateFilterChanged(EventArgs<Tuple<DateTime, DateTime>> e)
      => DateFilterChanged?.Invoke(this, e);

    /// <summary>
    /// Raises the <see cref="E:Done"/> event.
    /// <para>Occurs when <see cref="IConnectionSettings"/> are picked by module.</para>
    /// </summary>
    /// <param name="e">The <see cref="EventArgs{IConnectionSettings}"/> instance containing the event data.</param>
    protected virtual void OnDone(EventArgs<IConnectionSettings> e)
    {
      var handler = Done;
      handler?.Invoke(this, e);
    }
  }
}