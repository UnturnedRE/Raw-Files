// Decompiled with JetBrains decompiler
// Type: Pathfinding.NodeLink3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/Link3")]
  public class NodeLink3 : GraphModifier
  {
    protected static Dictionary<GraphNode, NodeLink3> reference = new Dictionary<GraphNode, NodeLink3>();
    private static readonly Color GizmosColor = new Color(0.8078431f, 0.5333334f, 0.1882353f, 0.5f);
    private static readonly Color GizmosColorSelected = new Color(0.9215686f, 0.4823529f, 0.1254902f, 1f);
    public float costFactor = 1f;
    public Transform end;
    public bool oneWay;
    private NodeLink3Node startNode;
    private NodeLink3Node endNode;
    private MeshNode connectedNode1;
    private MeshNode connectedNode2;
    private Vector3 clamped1;
    private Vector3 clamped2;
    private bool postScanCalled;

    public Transform StartTransform
    {
      get
      {
        return this.transform;
      }
    }

    public Transform EndTransform
    {
      get
      {
        return this.end;
      }
    }

    public GraphNode StartNode
    {
      get
      {
        return (GraphNode) this.startNode;
      }
    }

    public GraphNode EndNode
    {
      get
      {
        return (GraphNode) this.endNode;
      }
    }

    public static NodeLink3 GetNodeLink(GraphNode node)
    {
      NodeLink3 nodeLink3;
      NodeLink3.reference.TryGetValue(node, out nodeLink3);
      return nodeLink3;
    }

    public override void OnPostScan()
    {
      if (AstarPath.active.isScanning)
        this.InternalOnPostScan();
      else
        AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((Func<bool, bool>) (force =>
        {
          this.InternalOnPostScan();
          return true;
        })));
    }

    public void InternalOnPostScan()
    {
      if (AstarPath.active.astarData.pointGraph == null)
        AstarPath.active.astarData.AddGraph((NavGraph) new PointGraph());
      this.startNode = AstarPath.active.astarData.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3) this.StartTransform.position);
      this.startNode.link = this;
      this.endNode = AstarPath.active.astarData.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3) this.EndTransform.position);
      this.endNode.link = this;
      this.connectedNode1 = (MeshNode) null;
      this.connectedNode2 = (MeshNode) null;
      if (this.startNode == null || this.endNode == null)
      {
        this.startNode = (NodeLink3Node) null;
        this.endNode = (NodeLink3Node) null;
      }
      else
      {
        this.postScanCalled = true;
        NodeLink3.reference[(GraphNode) this.startNode] = this;
        NodeLink3.reference[(GraphNode) this.endNode] = this;
        this.Apply(true);
      }
    }

    public override void OnGraphsPostUpdate()
    {
      if (AstarPath.active.isScanning)
        return;
      if (this.connectedNode1 != null && this.connectedNode1.Destroyed)
        this.connectedNode1 = (MeshNode) null;
      if (this.connectedNode2 != null && this.connectedNode2.Destroyed)
        this.connectedNode2 = (MeshNode) null;
      if (!this.postScanCalled)
        this.OnPostScan();
      else
        this.Apply(false);
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      if (!((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null) || AstarPath.active.astarData == null || AstarPath.active.astarData.pointGraph == null)
        return;
      this.OnGraphsPostUpdate();
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this.postScanCalled = false;
      if (this.startNode != null)
        NodeLink3.reference.Remove((GraphNode) this.startNode);
      if (this.endNode != null)
        NodeLink3.reference.Remove((GraphNode) this.endNode);
      if (this.startNode == null || this.endNode == null)
        return;
      this.startNode.RemoveConnection((GraphNode) this.endNode);
      this.endNode.RemoveConnection((GraphNode) this.startNode);
      if (this.connectedNode1 == null || this.connectedNode2 == null)
        return;
      this.startNode.RemoveConnection((GraphNode) this.connectedNode1);
      this.connectedNode1.RemoveConnection((GraphNode) this.startNode);
      this.endNode.RemoveConnection((GraphNode) this.connectedNode2);
      this.connectedNode2.RemoveConnection((GraphNode) this.endNode);
    }

    private void RemoveConnections(GraphNode node)
    {
      node.ClearConnections(true);
    }

    [ContextMenu("Recalculate neighbours")]
    private void ContextApplyForce()
    {
      if (!Application.isPlaying)
        return;
      this.Apply(true);
      if (!((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null))
        return;
      AstarPath.active.FloodFill();
    }

    public void Apply(bool forceNewCheck)
    {
      NNConstraint none = NNConstraint.None;
      none.distanceXZ = true;
      int num1 = (int) this.startNode.GraphIndex;
      none.graphMask = ~(1 << num1);
      bool flag1 = true;
      NNInfo nearest1 = AstarPath.active.GetNearest(this.StartTransform.position, none);
      bool flag2 = ((flag1 ? 1 : 0) & (nearest1.node != this.connectedNode1 ? 0 : (nearest1.node != null ? 1 : 0))) != 0;
      this.connectedNode1 = nearest1.node as MeshNode;
      this.clamped1 = nearest1.clampedPosition;
      if (this.connectedNode1 != null)
        Debug.DrawRay((Vector3) this.connectedNode1.position, Vector3.up * 5f, Color.red);
      NNInfo nearest2 = AstarPath.active.GetNearest(this.EndTransform.position, none);
      bool flag3 = ((flag2 ? 1 : 0) & (nearest2.node != this.connectedNode2 ? 0 : (nearest2.node != null ? 1 : 0))) != 0;
      this.connectedNode2 = nearest2.node as MeshNode;
      this.clamped2 = nearest2.clampedPosition;
      if (this.connectedNode2 != null)
        Debug.DrawRay((Vector3) this.connectedNode2.position, Vector3.up * 5f, Color.cyan);
      if (this.connectedNode2 == null || this.connectedNode1 == null)
        return;
      this.startNode.SetPosition((Int3) this.StartTransform.position);
      this.endNode.SetPosition((Int3) this.EndTransform.position);
      if (flag3 && !forceNewCheck)
        return;
      this.RemoveConnections((GraphNode) this.startNode);
      this.RemoveConnections((GraphNode) this.endNode);
      uint cost = (uint) Mathf.RoundToInt((float) (Int3) (this.StartTransform.position - this.EndTransform.position).costMagnitude * this.costFactor);
      this.startNode.AddConnection((GraphNode) this.endNode, cost);
      this.endNode.AddConnection((GraphNode) this.startNode, cost);
      Int3 rhs = this.connectedNode2.position - this.connectedNode1.position;
      for (int i1 = 0; i1 < this.connectedNode1.GetVertexCount(); ++i1)
      {
        Int3 vertex1 = this.connectedNode1.GetVertex(i1);
        Int3 vertex2 = this.connectedNode1.GetVertex((i1 + 1) % this.connectedNode1.GetVertexCount());
        if (Int3.DotLong((vertex2 - vertex1).Normal2D(), rhs) <= 0L)
        {
          for (int i2 = 0; i2 < this.connectedNode2.GetVertexCount(); ++i2)
          {
            Int3 vertex3 = this.connectedNode2.GetVertex(i2);
            Int3 vertex4 = this.connectedNode2.GetVertex((i2 + 1) % this.connectedNode2.GetVertexCount());
            if (Int3.DotLong((vertex4 - vertex3).Normal2D(), rhs) >= 0L && (double) Int3.Angle(vertex4 - vertex3, vertex2 - vertex1) > 2.96705981095632)
            {
              float val1 = 0.0f;
              float num2 = Math.Min(1f, AstarMath.NearestPointFactor(vertex1, vertex2, vertex3));
              float num3 = Math.Max(val1, AstarMath.NearestPointFactor(vertex1, vertex2, vertex4));
              if ((double) num2 < (double) num3)
              {
                Debug.LogError((object) ("Wait wut!? " + (object) num3 + " " + (string) (object) num2 + " " + (string) vertex1 + " " + (string) vertex2 + " " + (string) vertex3 + " " + (string) vertex4 + "\nTODO, fix this error"));
              }
              else
              {
                Vector3 vector3_1 = (Vector3) (vertex2 - vertex1) * num3 + (Vector3) vertex1;
                Vector3 vector3_2 = (Vector3) (vertex2 - vertex1) * num2 + (Vector3) vertex1;
                this.startNode.portalA = vector3_1;
                this.startNode.portalB = vector3_2;
                this.endNode.portalA = vector3_2;
                this.endNode.portalB = vector3_1;
                this.connectedNode1.AddConnection((GraphNode) this.startNode, (uint) Mathf.RoundToInt((float) (Int3) (this.clamped1 - this.StartTransform.position).costMagnitude * this.costFactor));
                this.connectedNode2.AddConnection((GraphNode) this.endNode, (uint) Mathf.RoundToInt((float) (Int3) (this.clamped2 - this.EndTransform.position).costMagnitude * this.costFactor));
                this.startNode.AddConnection((GraphNode) this.connectedNode1, (uint) Mathf.RoundToInt((float) (Int3) (this.clamped1 - this.StartTransform.position).costMagnitude * this.costFactor));
                this.endNode.AddConnection((GraphNode) this.connectedNode2, (uint) Mathf.RoundToInt((float) (Int3) (this.clamped2 - this.EndTransform.position).costMagnitude * this.costFactor));
                return;
              }
            }
          }
        }
      }
    }

    private void DrawCircle(Vector3 o, float r, int detail, Color col)
    {
      Vector3 from = new Vector3(Mathf.Cos(0.0f) * r, 0.0f, Mathf.Sin(0.0f) * r) + o;
      Gizmos.color = col;
      for (int index = 0; index <= detail; ++index)
      {
        float f = (float) ((double) index * 3.14159274101257 * 2.0) / (float) detail;
        Vector3 to = new Vector3(Mathf.Cos(f) * r, 0.0f, Mathf.Sin(f) * r) + o;
        Gizmos.DrawLine(from, to);
        from = to;
      }
    }

    private void DrawGizmoBezier(Vector3 p1, Vector3 p2)
    {
      Vector3 vector3_1 = p2 - p1;
      if (vector3_1 == Vector3.zero)
        return;
      Vector3 rhs = Vector3.Cross(Vector3.up, vector3_1);
      Vector3 vector3_2 = Vector3.Cross(vector3_1, rhs);
      vector3_2 = vector3_2.normalized;
      vector3_2 *= vector3_1.magnitude * 0.1f;
      Vector3 p1_1 = p1 + vector3_2;
      Vector3 p2_1 = p2 + vector3_2;
      Vector3 from = p1;
      for (int index = 1; index <= 20; ++index)
      {
        float t = (float) index / 20f;
        Vector3 to = AstarMath.CubicBezier(p1, p1_1, p2_1, p2, t);
        Gizmos.DrawLine(from, to);
        from = to;
      }
    }

    public virtual void OnDrawGizmosSelected()
    {
      this.OnDrawGizmos(true);
    }

    public void OnDrawGizmos()
    {
      this.OnDrawGizmos(false);
    }

    public void OnDrawGizmos(bool selected)
    {
      Color col = !selected ? NodeLink3.GizmosColor : NodeLink3.GizmosColorSelected;
      if ((UnityEngine.Object) this.StartTransform != (UnityEngine.Object) null)
        this.DrawCircle(this.StartTransform.position, 0.4f, 10, col);
      if ((UnityEngine.Object) this.EndTransform != (UnityEngine.Object) null)
        this.DrawCircle(this.EndTransform.position, 0.4f, 10, col);
      if (!((UnityEngine.Object) this.StartTransform != (UnityEngine.Object) null) || !((UnityEngine.Object) this.EndTransform != (UnityEngine.Object) null))
        return;
      Gizmos.color = col;
      this.DrawGizmoBezier(this.StartTransform.position, this.EndTransform.position);
      if (!selected)
        return;
      Vector3 normalized = Vector3.Cross(Vector3.up, this.EndTransform.position - this.StartTransform.position).normalized;
      this.DrawGizmoBezier(this.StartTransform.position + normalized * 0.1f, this.EndTransform.position + normalized * 0.1f);
      this.DrawGizmoBezier(this.StartTransform.position - normalized * 0.1f, this.EndTransform.position - normalized * 0.1f);
    }
  }
}
