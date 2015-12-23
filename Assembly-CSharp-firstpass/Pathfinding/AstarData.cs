// Decompiled with JetBrains decompiler
// Type: Pathfinding.AstarData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace Pathfinding
{
  [Serializable]
  public class AstarData
  {
    [NonSerialized]
    public NavGraph[] graphs = new NavGraph[0];
    [NonSerialized]
    public UserConnection[] userConnections = new UserConnection[0];
    [NonSerialized]
    public NavMeshGraph navmesh;
    [NonSerialized]
    public GridGraph gridGraph;
    [NonSerialized]
    public PointGraph pointGraph;
    [NonSerialized]
    public RecastGraph recastGraph;
    public System.Type[] graphTypes;
    public bool hasBeenReverted;
    [SerializeField]
    private byte[] data;
    public uint dataChecksum;
    public byte[] data_backup;
    public TextAsset file_cachedStartup;
    public byte[] data_cachedStartup;
    [SerializeField]
    public bool cacheStartup;

    public AstarPath active
    {
      get
      {
        return AstarPath.active;
      }
    }

    public byte[] GetData()
    {
      return this.data;
    }

    public void SetData(byte[] data, uint checksum)
    {
      this.data = data;
      this.dataChecksum = checksum;
    }

    public void Awake()
    {
      this.userConnections = new UserConnection[0];
      this.graphs = new NavGraph[0];
      if (this.cacheStartup && (UnityEngine.Object) this.file_cachedStartup != (UnityEngine.Object) null)
        this.LoadFromCache();
      else
        this.DeserializeGraphs();
    }

    public void UpdateShortcuts()
    {
      this.navmesh = (NavMeshGraph) this.FindGraphOfType(typeof (NavMeshGraph));
      this.gridGraph = (GridGraph) this.FindGraphOfType(typeof (GridGraph));
      this.pointGraph = (PointGraph) this.FindGraphOfType(typeof (PointGraph));
      this.recastGraph = (RecastGraph) this.FindGraphOfType(typeof (RecastGraph));
    }

    public void LoadFromCache()
    {
      AstarPath.active.BlockUntilPathQueueBlocked();
      if ((UnityEngine.Object) this.file_cachedStartup != (UnityEngine.Object) null)
      {
        this.DeserializeGraphs(this.file_cachedStartup.bytes);
        GraphModifier.TriggerEvent(GraphModifier.EventType.PostCacheLoad);
      }
      else
        UnityEngine.Debug.LogError((object) "Can't load from cache since the cache is empty");
    }

    public byte[] SerializeGraphs()
    {
      return this.SerializeGraphs(SerializeSettings.Settings);
    }

    public byte[] SerializeGraphs(SerializeSettings settings)
    {
      uint checksum;
      return this.SerializeGraphs(settings, out checksum);
    }

    public byte[] SerializeGraphs(SerializeSettings settings, out uint checksum)
    {
      AstarPath.active.BlockUntilPathQueueBlocked();
      AstarSerializer sr = new AstarSerializer(this, settings);
      sr.OpenSerialize();
      this.SerializeGraphsPart(sr);
      byte[] numArray = sr.CloseSerialize();
      checksum = sr.GetChecksum();
      return numArray;
    }

    public void SerializeGraphsPart(AstarSerializer sr)
    {
      sr.SerializeGraphs(this.graphs);
      sr.SerializeUserConnections(this.userConnections);
      sr.SerializeNodes();
      sr.SerializeExtraInfo();
    }

    public void DeserializeGraphs()
    {
      if (this.data == null)
        return;
      this.DeserializeGraphs(this.data);
    }

    private void ClearGraphs()
    {
      if (this.graphs == null)
        return;
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null)
          this.graphs[index].OnDestroy();
      }
      this.graphs = (NavGraph[]) null;
      this.UpdateShortcuts();
    }

    public void OnDestroy()
    {
      this.ClearGraphs();
    }

    public void DeserializeGraphs(byte[] bytes)
    {
      AstarPath.active.BlockUntilPathQueueBlocked();
      try
      {
        if (bytes == null)
          throw new ArgumentNullException("Bytes should not be null when passed to DeserializeGraphs");
        AstarSerializer sr = new AstarSerializer(this);
        if (sr.OpenDeserialize(bytes))
        {
          this.DeserializeGraphsPart(sr);
          sr.CloseDeserialize();
          this.UpdateShortcuts();
        }
        else
          UnityEngine.Debug.Log((object) "Invalid data file (cannot read zip).\nThe data is either corrupt or it was saved using a 3.0.x or earlier version of the system");
        this.active.VerifyIntegrity();
      }
      catch (Exception ex)
      {
        UnityEngine.Debug.LogWarning((object) ("Caught exception while deserializing data.\n" + (object) ex));
        this.data_backup = bytes;
      }
    }

    public void DeserializeGraphsAdditive(byte[] bytes)
    {
      AstarPath.active.BlockUntilPathQueueBlocked();
      try
      {
        if (bytes == null)
          throw new ArgumentNullException("Bytes should not be null when passed to DeserializeGraphs");
        AstarSerializer sr = new AstarSerializer(this);
        if (sr.OpenDeserialize(bytes))
        {
          this.DeserializeGraphsPartAdditive(sr);
          sr.CloseDeserialize();
        }
        else
          UnityEngine.Debug.Log((object) "Invalid data file (cannot read zip).");
        this.active.VerifyIntegrity();
      }
      catch (Exception ex)
      {
        UnityEngine.Debug.LogWarning((object) ("Caught exception while deserializing data.\n" + (object) ex));
      }
    }

    public void DeserializeGraphsPart(AstarSerializer sr)
    {
      this.ClearGraphs();
      this.graphs = sr.DeserializeGraphs();
      if (this.graphs != null)
      {
        for (int index = 0; index < this.graphs.Length; ++index)
        {
          if (this.graphs[index] != null)
            this.graphs[index].graphIndex = (uint) index;
        }
      }
      this.userConnections = sr.DeserializeUserConnections();
      sr.DeserializeExtraInfo();
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AstarData.\u003CDeserializeGraphsPart\u003Ec__AnonStorey12 partCAnonStorey12 = new AstarData.\u003CDeserializeGraphsPart\u003Ec__AnonStorey12();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (partCAnonStorey12.i = 0; partCAnonStorey12.i < this.graphs.Length; partCAnonStorey12.i = partCAnonStorey12.i + 1)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.graphs[partCAnonStorey12.i] != null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.graphs[partCAnonStorey12.i].GetNodes(new GraphNodeDelegateCancelable(partCAnonStorey12.\u003C\u003Em__1));
        }
      }
      sr.PostDeserialization();
    }

    public void DeserializeGraphsPartAdditive(AstarSerializer sr)
    {
      if (this.graphs == null)
        this.graphs = new NavGraph[0];
      if (this.userConnections == null)
        this.userConnections = new UserConnection[0];
      List<NavGraph> list1 = new List<NavGraph>((IEnumerable<NavGraph>) this.graphs);
      list1.AddRange((IEnumerable<NavGraph>) sr.DeserializeGraphs());
      this.graphs = list1.ToArray();
      if (this.graphs != null)
      {
        for (int index = 0; index < this.graphs.Length; ++index)
        {
          if (this.graphs[index] != null)
            this.graphs[index].graphIndex = (uint) index;
        }
      }
      List<UserConnection> list2 = new List<UserConnection>((IEnumerable<UserConnection>) this.userConnections);
      list2.AddRange((IEnumerable<UserConnection>) sr.DeserializeUserConnections());
      this.userConnections = list2.ToArray();
      sr.DeserializeNodes();
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AstarData.\u003CDeserializeGraphsPartAdditive\u003Ec__AnonStorey13 additiveCAnonStorey13 = new AstarData.\u003CDeserializeGraphsPartAdditive\u003Ec__AnonStorey13();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (additiveCAnonStorey13.i = 0; additiveCAnonStorey13.i < this.graphs.Length; additiveCAnonStorey13.i = additiveCAnonStorey13.i + 1)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.graphs[additiveCAnonStorey13.i] != null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.graphs[additiveCAnonStorey13.i].GetNodes(new GraphNodeDelegateCancelable(additiveCAnonStorey13.\u003C\u003Em__2));
        }
      }
      sr.DeserializeExtraInfo();
      sr.PostDeserialization();
      for (int index1 = 0; index1 < this.graphs.Length; ++index1)
      {
        for (int index2 = index1 + 1; index2 < this.graphs.Length; ++index2)
        {
          if (this.graphs[index1] != null && this.graphs[index2] != null && this.graphs[index1].guid == this.graphs[index2].guid)
          {
            UnityEngine.Debug.LogWarning((object) "Guid Conflict when importing graphs additively. Imported graph will get a new Guid.\nThis message is (relatively) harmless.");
            this.graphs[index1].guid = Pathfinding.Util.Guid.NewGuid();
            break;
          }
        }
      }
    }

    public void FindGraphTypes()
    {
      System.Type[] types = Assembly.GetAssembly(typeof (AstarPath)).GetTypes();
      List<System.Type> list = new List<System.Type>();
      foreach (System.Type type in types)
      {
        for (System.Type baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
        {
          if (object.Equals((object) baseType, (object) typeof (NavGraph)))
          {
            list.Add(type);
            break;
          }
        }
      }
      this.graphTypes = list.ToArray();
    }

    public System.Type GetGraphType(string type)
    {
      for (int index = 0; index < this.graphTypes.Length; ++index)
      {
        if (this.graphTypes[index].Name == type)
          return this.graphTypes[index];
      }
      return (System.Type) null;
    }

    public NavGraph CreateGraph(string type)
    {
      UnityEngine.Debug.Log((object) ("Creating Graph of type '" + type + "'"));
      for (int index = 0; index < this.graphTypes.Length; ++index)
      {
        if (this.graphTypes[index].Name == type)
          return this.CreateGraph(this.graphTypes[index]);
      }
      UnityEngine.Debug.LogError((object) ("Graph type (" + type + ") wasn't found"));
      return (NavGraph) null;
    }

    public NavGraph CreateGraph(System.Type type)
    {
      NavGraph navGraph = Activator.CreateInstance(type) as NavGraph;
      navGraph.active = this.active;
      return navGraph;
    }

    public NavGraph AddGraph(string type)
    {
      NavGraph graph = (NavGraph) null;
      for (int index = 0; index < this.graphTypes.Length; ++index)
      {
        if (this.graphTypes[index].Name == type)
          graph = this.CreateGraph(this.graphTypes[index]);
      }
      if (graph == null)
      {
        UnityEngine.Debug.LogError((object) ("No NavGraph of type '" + type + "' could be found"));
        return (NavGraph) null;
      }
      this.AddGraph(graph);
      return graph;
    }

    public NavGraph AddGraph(System.Type type)
    {
      NavGraph graph = (NavGraph) null;
      for (int index = 0; index < this.graphTypes.Length; ++index)
      {
        if (object.Equals((object) this.graphTypes[index], (object) type))
          graph = this.CreateGraph(this.graphTypes[index]);
      }
      if (graph == null)
      {
        UnityEngine.Debug.LogError((object) ("No NavGraph of type '" + (object) type + "' could be found, " + (string) (object) this.graphTypes.Length + " graph types are avaliable"));
        return (NavGraph) null;
      }
      this.AddGraph(graph);
      return graph;
    }

    public void AddGraph(NavGraph graph)
    {
      AstarPath.active.BlockUntilPathQueueBlocked();
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] == null)
        {
          this.graphs[index] = graph;
          graph.active = this.active;
          graph.Awake();
          graph.graphIndex = (uint) index;
          this.UpdateShortcuts();
          return;
        }
      }
      if (this.graphs != null && (long) this.graphs.Length >= (long) byte.MaxValue)
        throw new Exception("Graph Count Limit Reached. You cannot have more than " + (object) (uint) byte.MaxValue + " graphs. Some compiler directives can change this limit, e.g ASTAR_MORE_AREAS, look under the 'Optimizations' tab in the A* Inspector");
      this.graphs = new List<NavGraph>((IEnumerable<NavGraph>) this.graphs)
      {
        graph
      }.ToArray();
      this.UpdateShortcuts();
      graph.active = this.active;
      graph.Awake();
      graph.graphIndex = (uint) (this.graphs.Length - 1);
    }

    public bool RemoveGraph(NavGraph graph)
    {
      graph.SafeOnDestroy();
      int index = 0;
      while (index < this.graphs.Length && this.graphs[index] != graph)
        ++index;
      if (index == this.graphs.Length)
        return false;
      this.graphs[index] = (NavGraph) null;
      this.UpdateShortcuts();
      return true;
    }

    public static NavGraph GetGraph(GraphNode node)
    {
      if (node == null)
        return (NavGraph) null;
      AstarPath astarPath = AstarPath.active;
      if ((UnityEngine.Object) astarPath == (UnityEngine.Object) null)
        return (NavGraph) null;
      AstarData astarData = astarPath.astarData;
      if (astarData == null)
        return (NavGraph) null;
      if (astarData.graphs == null)
        return (NavGraph) null;
      uint graphIndex = node.GraphIndex;
      if ((long) graphIndex >= (long) astarData.graphs.Length)
        return (NavGraph) null;
      return astarData.graphs[(int) graphIndex];
    }

    public GraphNode GetNode(int graphIndex, int nodeIndex)
    {
      return this.GetNode(graphIndex, nodeIndex, this.graphs);
    }

    public GraphNode GetNode(int graphIndex, int nodeIndex, NavGraph[] graphs)
    {
      throw new NotImplementedException();
    }

    public NavGraph FindGraphOfType(System.Type type)
    {
      if (this.graphs != null)
      {
        for (int index = 0; index < this.graphs.Length; ++index)
        {
          if (this.graphs[index] != null && object.Equals((object) this.graphs[index].GetType(), (object) type))
            return this.graphs[index];
        }
      }
      return (NavGraph) null;
    }

    [DebuggerHidden]
    public IEnumerable FindGraphsOfType(System.Type type)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AstarData.\u003CFindGraphsOfType\u003Ec__Iterator3 ofTypeCIterator3 = new AstarData.\u003CFindGraphsOfType\u003Ec__Iterator3()
      {
        type = type,
        \u003C\u0024\u003Etype = type,
        \u003C\u003Ef__this = this
      };
      // ISSUE: reference to a compiler-generated field
      ofTypeCIterator3.\u0024PC = -2;
      return (IEnumerable) ofTypeCIterator3;
    }

    [DebuggerHidden]
    public IEnumerable GetUpdateableGraphs()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AstarData.\u003CGetUpdateableGraphs\u003Ec__Iterator4 graphsCIterator4 = new AstarData.\u003CGetUpdateableGraphs\u003Ec__Iterator4()
      {
        \u003C\u003Ef__this = this
      };
      // ISSUE: reference to a compiler-generated field
      graphsCIterator4.\u0024PC = -2;
      return (IEnumerable) graphsCIterator4;
    }

    [DebuggerHidden]
    public IEnumerable GetRaycastableGraphs()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AstarData.\u003CGetRaycastableGraphs\u003Ec__Iterator5 graphsCIterator5 = new AstarData.\u003CGetRaycastableGraphs\u003Ec__Iterator5()
      {
        \u003C\u003Ef__this = this
      };
      // ISSUE: reference to a compiler-generated field
      graphsCIterator5.\u0024PC = -2;
      return (IEnumerable) graphsCIterator5;
    }

    public int GetGraphIndex(NavGraph graph)
    {
      if (graph == null)
        throw new ArgumentNullException("graph");
      if (this.graphs != null)
      {
        for (int index = 0; index < this.graphs.Length; ++index)
        {
          if (graph == this.graphs[index])
            return index;
        }
      }
      return -1;
    }

    public int GuidToIndex(Pathfinding.Util.Guid guid)
    {
      if (this.graphs == null)
        return -1;
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null && this.graphs[index].guid == guid)
          return index;
      }
      return -1;
    }

    public NavGraph GuidToGraph(Pathfinding.Util.Guid guid)
    {
      if (this.graphs == null)
        return (NavGraph) null;
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null && this.graphs[index].guid == guid)
          return this.graphs[index];
      }
      return (NavGraph) null;
    }
  }
}
