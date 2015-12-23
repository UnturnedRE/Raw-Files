// Decompiled with JetBrains decompiler
// Type: Pathfinding.NavGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using Pathfinding.Serialization.JsonFx;
using System;
using UnityEngine;

namespace Pathfinding
{
  public abstract class NavGraph
  {
    [JsonMember]
    public bool drawGizmos = true;
    public byte[] _sguid;
    public AstarPath active;
    [JsonMember]
    public uint initialPenalty;
    [JsonMember]
    public bool open;
    public uint graphIndex;
    [JsonMember]
    public string name;
    [JsonMember]
    public bool infoScreenOpen;
    [JsonMember]
    public Matrix4x4 matrix;
    public Matrix4x4 inverseMatrix;

    [JsonMember]
    public Pathfinding.Util.Guid guid
    {
      get
      {
        if (this._sguid == null || this._sguid.Length != 16)
          this._sguid = Pathfinding.Util.Guid.NewGuid().ToByteArray();
        return new Pathfinding.Util.Guid(this._sguid);
      }
      set
      {
        this._sguid = value.ToByteArray();
      }
    }

    public virtual int CountNodes()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NavGraph.\u003CCountNodes\u003Ec__AnonStorey26 nodesCAnonStorey26 = new NavGraph.\u003CCountNodes\u003Ec__AnonStorey26();
      // ISSUE: reference to a compiler-generated field
      nodesCAnonStorey26.count = 0;
      // ISSUE: reference to a compiler-generated method
      this.GetNodes(new GraphNodeDelegateCancelable(nodesCAnonStorey26.\u003C\u003Em__1A));
      // ISSUE: reference to a compiler-generated field
      return nodesCAnonStorey26.count;
    }

    public abstract void GetNodes(GraphNodeDelegateCancelable del);

    public void SetMatrix(Matrix4x4 m)
    {
      this.matrix = m;
      this.inverseMatrix = m.inverse;
    }

    public virtual void CreateNodes(int number)
    {
      throw new NotSupportedException();
    }

    public virtual void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NavGraph.\u003CRelocateNodes\u003Ec__AnonStorey27 nodesCAnonStorey27 = new NavGraph.\u003CRelocateNodes\u003Ec__AnonStorey27();
      Matrix4x4 inverse = oldMatrix.inverse;
      // ISSUE: reference to a compiler-generated field
      nodesCAnonStorey27.m = inverse * newMatrix;
      // ISSUE: reference to a compiler-generated method
      this.GetNodes(new GraphNodeDelegateCancelable(nodesCAnonStorey27.\u003C\u003Em__1B));
      this.SetMatrix(newMatrix);
    }

    public NNInfo GetNearest(Vector3 position)
    {
      return this.GetNearest(position, NNConstraint.None);
    }

    public NNInfo GetNearest(Vector3 position, NNConstraint constraint)
    {
      return this.GetNearest(position, constraint, (GraphNode) null);
    }

    public virtual NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NavGraph.\u003CGetNearest\u003Ec__AnonStorey28 nearestCAnonStorey28 = new NavGraph.\u003CGetNearest\u003Ec__AnonStorey28();
      // ISSUE: reference to a compiler-generated field
      nearestCAnonStorey28.position = position;
      // ISSUE: reference to a compiler-generated field
      nearestCAnonStorey28.constraint = constraint;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      nearestCAnonStorey28.maxDistSqr = !nearestCAnonStorey28.constraint.constrainDistance ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistanceSqr;
      // ISSUE: reference to a compiler-generated field
      nearestCAnonStorey28.minDist = float.PositiveInfinity;
      // ISSUE: reference to a compiler-generated field
      nearestCAnonStorey28.minNode = (GraphNode) null;
      // ISSUE: reference to a compiler-generated field
      nearestCAnonStorey28.minConstDist = float.PositiveInfinity;
      // ISSUE: reference to a compiler-generated field
      nearestCAnonStorey28.minConstNode = (GraphNode) null;
      // ISSUE: reference to a compiler-generated method
      this.GetNodes(new GraphNodeDelegateCancelable(nearestCAnonStorey28.\u003C\u003Em__1C));
      // ISSUE: reference to a compiler-generated field
      NNInfo nnInfo = new NNInfo(nearestCAnonStorey28.minNode);
      // ISSUE: reference to a compiler-generated field
      nnInfo.constrainedNode = nearestCAnonStorey28.minConstNode;
      // ISSUE: reference to a compiler-generated field
      if (nearestCAnonStorey28.minConstNode != null)
      {
        // ISSUE: reference to a compiler-generated field
        nnInfo.constClampedPosition = (Vector3) nearestCAnonStorey28.minConstNode.position;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (nearestCAnonStorey28.minNode != null)
        {
          // ISSUE: reference to a compiler-generated field
          nnInfo.constrainedNode = nearestCAnonStorey28.minNode;
          // ISSUE: reference to a compiler-generated field
          nnInfo.constClampedPosition = (Vector3) nearestCAnonStorey28.minNode.position;
        }
      }
      return nnInfo;
    }

    public virtual NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
    {
      return this.GetNearest(position, constraint);
    }

    public virtual void Awake()
    {
    }

    public void SafeOnDestroy()
    {
      AstarPath.RegisterSafeUpdate(new OnVoidDelegate(this.OnDestroy));
    }

    public virtual void OnDestroy()
    {
      this.GetNodes((GraphNodeDelegateCancelable) (node =>
      {
        node.Destroy();
        return true;
      }));
    }

    public void ScanGraph()
    {
      if (AstarPath.OnPreScan != null)
        AstarPath.OnPreScan(AstarPath.active);
      if (AstarPath.OnGraphPreScan != null)
        AstarPath.OnGraphPreScan(this);
      this.ScanInternal();
      if (AstarPath.OnGraphPostScan != null)
        AstarPath.OnGraphPostScan(this);
      if (AstarPath.OnPostScan == null)
        return;
      AstarPath.OnPostScan(AstarPath.active);
    }

    [Obsolete("Please use AstarPath.active.Scan or if you really want this.ScanInternal which has the same functionality as this method had")]
    public void Scan()
    {
      throw new Exception("This method is deprecated. Please use AstarPath.active.Scan or if you really want this.ScanInternal which has the same functionality as this method had.");
    }

    public void ScanInternal()
    {
      this.ScanInternal((OnScanStatus) null);
    }

    public abstract void ScanInternal(OnScanStatus statusCallback);

    public virtual Color NodeColor(GraphNode node, PathHandler data)
    {
      Color color = AstarColor.NodeConnection;
      bool flag = false;
      if (node == null)
        return AstarColor.NodeConnection;
      GraphDebugMode graphDebugMode = AstarPath.active.debugMode;
      switch (graphDebugMode)
      {
        case GraphDebugMode.Penalty:
          color = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (float) node.Penalty / AstarPath.active.debugRoof);
          flag = true;
          break;
        case GraphDebugMode.Tags:
          color = AstarMath.IntToColor((int) node.Tag, 0.5f);
          flag = true;
          break;
        default:
          if (graphDebugMode == GraphDebugMode.Areas)
          {
            color = AstarColor.GetAreaColor(node.Area);
            flag = true;
            break;
          }
          break;
      }
      if (!flag)
      {
        if (data == null)
          return AstarColor.NodeConnection;
        PathNode pathNode = data.GetPathNode(node);
        switch (AstarPath.active.debugMode)
        {
          case GraphDebugMode.G:
            color = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (float) pathNode.G / AstarPath.active.debugRoof);
            break;
          case GraphDebugMode.H:
            color = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (float) pathNode.H / AstarPath.active.debugRoof);
            break;
          case GraphDebugMode.F:
            color = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (float) pathNode.F / AstarPath.active.debugRoof);
            break;
        }
      }
      color.a *= 0.5f;
      return color;
    }

    public virtual void SerializeExtraInfo(GraphSerializationContext ctx)
    {
    }

    public virtual void DeserializeExtraInfo(GraphSerializationContext ctx)
    {
    }

    public virtual void PostDeserialization()
    {
    }

    public bool InSearchTree(GraphNode node, Path path)
    {
      if (path == null || path.pathHandler == null)
        return true;
      return (int) path.pathHandler.GetPathNode(node).pathID == (int) path.pathID;
    }

    public virtual void OnDrawGizmos(bool drawNodes)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NavGraph.\u003COnDrawGizmos\u003Ec__AnonStorey29 gizmosCAnonStorey29 = new NavGraph.\u003COnDrawGizmos\u003Ec__AnonStorey29();
      // ISSUE: reference to a compiler-generated field
      gizmosCAnonStorey29.\u003C\u003Ef__this = this;
      if (!drawNodes)
        return;
      // ISSUE: reference to a compiler-generated field
      gizmosCAnonStorey29.data = AstarPath.active.debugPathData;
      // ISSUE: reference to a compiler-generated field
      gizmosCAnonStorey29.node = (GraphNode) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      gizmosCAnonStorey29.del = new GraphNodeDelegate(gizmosCAnonStorey29.\u003C\u003Em__1E);
      // ISSUE: reference to a compiler-generated method
      this.GetNodes(new GraphNodeDelegateCancelable(gizmosCAnonStorey29.\u003C\u003Em__1F));
    }
  }
}
