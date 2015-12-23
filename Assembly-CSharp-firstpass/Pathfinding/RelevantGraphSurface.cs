// Decompiled with JetBrains decompiler
// Type: Pathfinding.RelevantGraphSurface
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/Navmesh/RelevantGraphSurface")]
  public class RelevantGraphSurface : MonoBehaviour
  {
    public float maxRange = 1f;
    private static RelevantGraphSurface root;
    private RelevantGraphSurface prev;
    private RelevantGraphSurface next;
    private Vector3 position;

    public Vector3 Position
    {
      get
      {
        return this.position;
      }
    }

    public RelevantGraphSurface Next
    {
      get
      {
        return this.next;
      }
    }

    public RelevantGraphSurface Prev
    {
      get
      {
        return this.prev;
      }
    }

    public static RelevantGraphSurface Root
    {
      get
      {
        return RelevantGraphSurface.root;
      }
    }

    public void UpdatePosition()
    {
      this.position = this.transform.position;
    }

    private void OnEnable()
    {
      this.UpdatePosition();
      if ((Object) RelevantGraphSurface.root == (Object) null)
      {
        RelevantGraphSurface.root = this;
      }
      else
      {
        this.next = RelevantGraphSurface.root;
        RelevantGraphSurface.root.prev = this;
        RelevantGraphSurface.root = this;
      }
    }

    private void OnDisable()
    {
      if ((Object) RelevantGraphSurface.root == (Object) this)
      {
        RelevantGraphSurface.root = this.next;
        if ((Object) RelevantGraphSurface.root != (Object) null)
          RelevantGraphSurface.root.prev = (RelevantGraphSurface) null;
      }
      else
      {
        if ((Object) this.prev != (Object) null)
          this.prev.next = this.next;
        if ((Object) this.next != (Object) null)
          this.next.prev = this.prev;
      }
      this.prev = (RelevantGraphSurface) null;
      this.next = (RelevantGraphSurface) null;
    }

    public static void UpdateAllPositions()
    {
      for (RelevantGraphSurface relevantGraphSurface = RelevantGraphSurface.root; (Object) relevantGraphSurface != (Object) null; relevantGraphSurface = relevantGraphSurface.Next)
        relevantGraphSurface.UpdatePosition();
    }

    public static void FindAllGraphSurfaces()
    {
      RelevantGraphSurface[] relevantGraphSurfaceArray = Object.FindObjectsOfType(typeof (RelevantGraphSurface)) as RelevantGraphSurface[];
      for (int index = 0; index < relevantGraphSurfaceArray.Length; ++index)
      {
        relevantGraphSurfaceArray[index].OnDisable();
        relevantGraphSurfaceArray[index].OnEnable();
      }
    }

    public void OnDrawGizmos()
    {
      Gizmos.color = new Color(0.2235294f, 0.827451f, 0.1803922f, 0.4f);
      Gizmos.DrawLine(this.transform.position - Vector3.up * this.maxRange, this.transform.position + Vector3.up * this.maxRange);
    }

    public void OnDrawGizmosSelected()
    {
      Gizmos.color = new Color(0.2235294f, 0.827451f, 0.1803922f);
      Gizmos.DrawLine(this.transform.position - Vector3.up * this.maxRange, this.transform.position + Vector3.up * this.maxRange);
    }
  }
}
