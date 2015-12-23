// Decompiled with JetBrains decompiler
// Type: Pathfinding.RichPath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class RichPath
  {
    private List<RichPathPart> parts = new List<RichPathPart>();
    private int currentPart;
    public Seeker seeker;

    public void Initialize(Seeker s, Path p, bool mergePartEndpoints, RichFunnel.FunnelSimplification simplificationMode)
    {
      if (p.error)
        throw new ArgumentException("Path has an error");
      List<GraphNode> nodes = p.path;
      if (nodes.Count == 0)
        throw new ArgumentException("Path traverses no nodes");
      this.seeker = s;
      for (int index = 0; index < this.parts.Count; ++index)
      {
        if (this.parts[index] is RichFunnel)
          ObjectPool<RichFunnel>.Release(this.parts[index] as RichFunnel);
        else if (this.parts[index] is RichSpecial)
          ObjectPool<RichSpecial>.Release(this.parts[index] as RichSpecial);
      }
      this.parts.Clear();
      this.currentPart = 0;
      for (int end = 0; end < nodes.Count; ++end)
      {
        if (nodes[end] is TriangleMeshNode)
        {
          RichFunnel richFunnel = ObjectPool<RichFunnel>.Claim().Initialize(this, AstarData.GetGraph(nodes[end]) as IFunnelGraph);
          richFunnel.funnelSimplificationMode = simplificationMode;
          int start = end;
          uint graphIndex = nodes[start].GraphIndex;
          while (end < nodes.Count && ((int) nodes[end].GraphIndex == (int) graphIndex || nodes[end] is NodeLink3Node))
            ++end;
          --end;
          richFunnel.exactStart = start != 0 ? (!mergePartEndpoints ? (Vector3) nodes[start].position : (Vector3) nodes[start - 1].position) : p.vectorPath[0];
          richFunnel.exactEnd = end != nodes.Count - 1 ? (!mergePartEndpoints ? (Vector3) nodes[end].position : (Vector3) nodes[end + 1].position) : p.vectorPath[p.vectorPath.Count - 1];
          richFunnel.BuildFunnelCorridor(nodes, start, end);
          this.parts.Add((RichPathPart) richFunnel);
        }
        else if (nodes[end] is GraphNode && (UnityEngine.Object) NodeLink2.GetNodeLink(nodes[end]) != (UnityEngine.Object) null)
        {
          NodeLink2 nodeLink = NodeLink2.GetNodeLink(nodes[end]);
          int index1 = end;
          uint graphIndex = nodes[index1].GraphIndex;
          int index2 = end + 1;
          while (index2 < nodes.Count && (int) nodes[index2].GraphIndex == (int) graphIndex)
            ++index2;
          end = index2 - 1;
          if (end - index1 > 1)
            throw new Exception("NodeLink2 path length greater than two (2) nodes. " + (object) (end - index1));
          if (end - index1 != 0)
            this.parts.Add((RichPathPart) ObjectPool<RichSpecial>.Claim().Initialize(nodeLink, nodes[index1]));
        }
      }
    }

    public bool PartsLeft()
    {
      return this.currentPart < this.parts.Count;
    }

    public void NextPart()
    {
      ++this.currentPart;
      if (this.currentPart < this.parts.Count)
        return;
      this.currentPart = this.parts.Count;
    }

    public RichPathPart GetCurrentPart()
    {
      if (this.currentPart >= this.parts.Count)
        return (RichPathPart) null;
      return this.parts[this.currentPart];
    }
  }
}
