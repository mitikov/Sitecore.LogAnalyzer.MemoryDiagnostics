namespace Sitecore.LogAnalyzer.MemoryDiagnostics.ModelViewer
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.MemoryDiagnostics.Exceptions;
  using Sitecore.MemoryDiagnostics.Extensions;

  public class Namespace : IDictionary<string, Namespace>
  {
    #region Fields

    private IDictionary<string, Namespace> subNamespaces;

    #endregion

    #region Constructors

    private Namespace(string nameOnLevel, Namespace parent)
    {
      if (string.IsNullOrWhiteSpace(nameOnLevel))
        throw new ArgumentException(nameof(nameOnLevel));

      Parent = parent;
      NameOnLevel = nameOnLevel;
      subNamespaces = new Dictionary<string, Namespace>();

      if (Parent != null)
      {
        Parent.Add(NameOnLevel, this);
      }
    }

    private Namespace(string nameOfLevel)
      : this(nameOfLevel, null)
    {

    }

    #endregion


    #region Static

    public static IEnumerable<Namespace> FromStrings(IEnumerable<string> namespacestrings)
    {
      // Split all strings
      var splitSubNamespaces = namespacestrings
          .Select(fullNamespace =>
              fullNamespace.Split('.'));

      return FromSplitStrings(null, splitSubNamespaces);
    }
    public static IEnumerable<Namespace> FromStrings(IEnumerable<Type> namespaceStrings)
    {
      var allModelTypes = namespaceStrings.ToArray();

      // Split all strings
      var splitSubNamespaces = allModelTypes
          .Select(fullNamespace =>
              fullNamespace.FullName.Split('.'));

      var namespacegrouping = FromSplitStrings(null, splitSubNamespaces).ToArray();
      namespacegrouping.ForEach(topLevelGrouping => SetConstructedTypes(topLevelGrouping, allModelTypes));
      return namespacegrouping;
    }

    private static void SetConstructedTypes(Namespace @namespace, Type[] copy, int depth = 0)
    {
      ++depth;
      if (depth > 10)
      {
        throw new RecursionException();
      }

      if (@namespace.Count == 0)
      {
        @namespace.ConstructedType = copy.FirstOrDefault(type => type.FullName.Equals(@namespace.FullName));//No child entries, trying set constructed type
        return;
      }

      foreach (var n in @namespace.Values)
      {
        SetConstructedTypes(n, copy, depth);
      }
    }

    public static IEnumerable<Namespace> FromSplitStrings(Namespace root, IEnumerable<IEnumerable<string>> splitSubNamespaces)
    {
      if (splitSubNamespaces == null)
      {
        throw new ArgumentNullException(nameof(splitSubNamespaces));
      }

      return splitSubNamespaces
          // Remove those split sequences that have no elements
          .Where(splitSubNamespace =>
              splitSubNamespace.Any())
          // Group by the outermost namespace
          .GroupBy(splitNamespace =>
               splitNamespace.First())
          // Create Namespace for each group and prepare sequences that represent nested namespaces
          .Select(group =>
              new
              {
                Root = new Namespace(group.Key, root),
                SplitSubnamespaces = group
                    .Select(splitNamespace =>
                        splitNamespace.Skip(1))
              })
          // Select nested namespaces with recursive split call
          .Select(obj =>
              new
              {
                obj.Root,
                SubNamespaces = FromSplitStrings(obj.Root, obj.SplitSubnamespaces)
              })
          // Select only uppermost level namespaces to return
          .Select(obj =>
              obj.Root)
          // To avoid deferred execution problems when recursive function may not be able to create nested namespaces
          .ToArray();
    }

    #endregion

    #region Properties

    public readonly string NameOnLevel;

    public Type ConstructedType
    {
      get;
      protected set;
    }

    public string FullName => Parent == null ? NameOnLevel : $"{Parent.FullName}.{NameOnLevel}";

    private Namespace _Parent;

    public Namespace Parent
    {
      get
      {
        return _Parent;
      }
      private set
      {
        if (Parent != null)
          Parent.Remove(NameOnLevel);

        _Parent = value;
      }
    }

    #endregion


    #region IDictionary implementation

    public void Add(string key, Namespace value)
    {
      if (ContainsKey(key))
        throw new InvalidOperationException("Namespace already contains namespace with such name on level");

      subNamespaces.Add(key, value);
    }

    public bool ContainsKey(string key)
      => subNamespaces.ContainsKey(key);

    public ICollection<string> Keys => subNamespaces.Keys;

    public bool Remove(string key)
    {
      if (!ContainsKey(key))
        throw new KeyNotFoundException();

      this[key]._Parent = null;

      return subNamespaces.Remove(key);
    }

    public bool TryGetValue(string key, out Namespace value) => subNamespaces.TryGetValue(key, out value);

    public ICollection<Namespace> Values => subNamespaces.Values;

    public ICollection<Namespace> Subnamespaces => subNamespaces.Values;

    public Namespace this[string nameOnLevel]
    {
      get
      {
        return subNamespaces[nameOnLevel];
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof(value));


        if (TryGetValue(nameOnLevel, out Namespace toReplace))
        {
          toReplace.Parent = null;
        }

        value.Parent = this;
      }
    }

    public void Add(KeyValuePair<string, Namespace> item) => Add(item.Key, item.Value);

    public void Clear()
    {
      foreach (var subNamespace in subNamespaces.Select(kv => kv.Value))
      {
        subNamespace._Parent = null;
      }

      subNamespaces.Clear();
    }

    public bool Contains(KeyValuePair<string, Namespace> item)
      => subNamespaces.Contains(item);

    public void CopyTo(KeyValuePair<string, Namespace>[] array, int arrayIndex)
      => subNamespaces.CopyTo(array, arrayIndex);

    public int Count => subNamespaces.Count;

    public bool IsReadOnly => false;

    public bool Remove(KeyValuePair<string, Namespace> item) => subNamespaces.Remove(item);

    public IEnumerator<KeyValuePair<string, Namespace>> GetEnumerator() => subNamespaces.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region Overrides

    public override string ToString() => FullName;

    #endregion
  }
}
