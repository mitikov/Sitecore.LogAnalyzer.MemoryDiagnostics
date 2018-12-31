namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ModelViewer
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Forms;

  using Sitecore.DumpModule.Common.UI;
  using Sitecore.MemoryDiagnostics.Attributes;
  using Sitecore.MemoryDiagnostics.Models.BaseMappingModel;
  using Sitecore.MemoryDiagnostics.Utils;
  using Sitecore.LogAnalyzer.MemoryDiagnostics.ModelViewer.ConnectionDetails;

  public partial class PickObjectToPrint : PickDumpDetails
  {
    public PickObjectToPrint()
    {
      InitializeComponent();
    }

    private void PickObjectToPrint_Load(object sender, EventArgs e)
    {
      AssemblyUtils.EnsureBinAssembliesLoaded();
      FillTreeWithTypes();
    }

    private void FillTreeWithTypes()
    {
      var types = FindTypes();
      var namespaces = Namespace.FromStrings(types);

      AddNamespaces(typeTreeView.Nodes, namespaces);
    }

    private List<Type> FindTypes()
    {
      var types = new List<Type>(100 * 10);
      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.IsDefined(typeof(CarriesMemoryDumpAnalysisLogic), false)))
      {
        try
        {
          var candidates = from type in assembly.GetExportedTypes()
                           where !type.IsInterface
                           where !type.IsAbstract
                           where typeof(ClrObjectMappingModel).IsAssignableFrom(type)
                           where Attribute.IsDefined(type, typeof(ModelMappingAttribute), inherit: true)
                           select type;
          types.AddRange(candidates);
        }
        catch (Exception ex)
        {
          //Context.LogError("Failed to load types", ex);
        }
      }
      return types;
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
