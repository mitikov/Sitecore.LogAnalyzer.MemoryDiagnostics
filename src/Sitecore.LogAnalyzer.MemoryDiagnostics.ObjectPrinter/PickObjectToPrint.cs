namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ObjectPrinter
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using System.Windows.Forms;

  using Sitecore.DumpModule.Common.UI;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.ObjectPrinter.ConnectionDetails;
  using Sitecore.MemoryDiagnostics.Attributes;
  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
  using Sitecore.MemoryDiagnostics.Utils;

  /// <summary>
  /// Setups <see cref="MemoryDumpConnectionWithType"/> based on user type to inspect selection.
  /// <para>All assemblies loaded into domain that are decorated with <see cref="CarriesMemoryDumpAnalysisLogic"/> inspected for defined models.</para>
  /// <para>Models in turn, contain target type they represent - via <see cref="ModelMappingAttribute"/> - these types are provided for user to pick from.</para>
  /// </summary>
  public partial class PickObjectToPrint : PickDumpDetails
  {
    public PickObjectToPrint() => InitializeComponent();

    private void PickObjectToPrint_Load(object sender, EventArgs e)
    {
      AssemblyUtils.EnsureBinAssembliesLoaded();
      FillTreeWithTypes();
    }

    private void FillTreeWithTypes()
    {
      var types = LoadModelMappingTypes(AppDomain.CurrentDomain.GetAssemblies(), assemblyAttributeFilter: typeof(CarriesMemoryDumpAnalysisLogic), modelMappingAttributeFilter: typeof(ModelMappingAttribute));
      var namespaces = Namespace.FromStrings(types);

      AddNamespaces(typeTreeView.Nodes, namespaces);
    }

    /// <summary>
    /// Finds model types in loaded assemblies.
    /// <para>Processes only assemblies that are decorated with <paramref name="assemblyAttributeFilter"/>.</para>
    /// </summary>
    /// <param name="assemblyAttributeFilter"></param>
    /// <param name="modelMappingAttributeFilter"></param>
    /// <returns></returns>
    private IEnumerable<Type> LoadModelMappingTypes(IEnumerable<Assembly> assemblies, Type assemblyAttributeFilter, Type modelMappingAttributeFilter)
    {
      return from assembly in assemblies
             where assembly.IsDefined(assemblyAttributeFilter, inherit: false)
             from exportedType in assembly.GetExportedTypes()
             where !exportedType.IsAbstract && !exportedType.IsInterface
             where typeof(ClrObjectMappingModel).IsAssignableFrom(exportedType)
             where Attribute.IsDefined(exportedType, modelMappingAttributeFilter, inherit: true)
             select exportedType;
    }

    protected virtual void AddNamespaces(TreeNodeCollection nodeCollection, IEnumerable<Namespace> namespaces)
    {
      foreach (var aNamespace in namespaces)
      {
        var node = new TreeNode(aNamespace.NameOnLevel)
        {
          Tag = aNamespace
        };
        nodeCollection.Add(node);
        if (aNamespace.Subnamespaces.Count > 0)
        {
          AddNamespaces(node.Nodes, aNamespace.Subnamespaces);
        }

        node.Expand();
      }
    }

    protected override void okBtn_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Type selectedConvertorType = GetSelectedType();
      FileConnection = new MemoryDumpConnectionWithType(dumpPathTxt.Text, mscordPathTxt.Text, selectedConvertorType);
      prevConnection = FileConnection;
      Close();
    }

    protected override bool SelectedDataExists()
    {
      return base.SelectedDataExists() && (typeTreeView.SelectedNode != null);
    }

    private Type GetSelectedType()
    {
      var selected = typeTreeView.SelectedNode;

      var casted = selected?.Tag as Namespace;

      return casted?.ConstructedType;

      //TODO: override;
    }

    private void typeTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
      var casted = e.Node.Tag as Namespace;
      if ((casted == null) || (casted.Subnamespaces.Count > 0))
      {
        e.Node.Checked = false;
      }

      EnsureSelectedDataExists(sender, e);
    }
  }
}
