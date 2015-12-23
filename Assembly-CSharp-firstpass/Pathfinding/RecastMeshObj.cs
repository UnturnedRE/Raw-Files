// Decompiled with JetBrains decompiler
// Type: Pathfinding.RecastMeshObj
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/Navmesh/RecastMeshObj")]
  public class RecastMeshObj : MonoBehaviour
  {
    protected static RecastBBTree tree = new RecastBBTree();
    protected static List<RecastMeshObj> dynamicMeshObjs = new List<RecastMeshObj>();
    [HideInInspector]
    public Bounds bounds;
    public bool dynamic;
    public int area;
    private bool _dynamic;
    private bool registered;

    public static void GetAllInBounds(List<RecastMeshObj> buffer, Bounds bounds)
    {
      if (!Application.isPlaying)
      {
        RecastMeshObj[] recastMeshObjArray = UnityEngine.Object.FindObjectsOfType(typeof (RecastMeshObj)) as RecastMeshObj[];
        for (int index = 0; index < recastMeshObjArray.Length; ++index)
        {
          recastMeshObjArray[index].RecalculateBounds();
          if (recastMeshObjArray[index].GetBounds().Intersects(bounds))
            buffer.Add(recastMeshObjArray[index]);
        }
      }
      else
      {
        if ((double) Time.timeSinceLevelLoad == 0.0)
        {
          foreach (RecastMeshObj recastMeshObj in UnityEngine.Object.FindObjectsOfType(typeof (RecastMeshObj)) as RecastMeshObj[])
            recastMeshObj.Register();
        }
        for (int index = 0; index < RecastMeshObj.dynamicMeshObjs.Count; ++index)
        {
          if (RecastMeshObj.dynamicMeshObjs[index].GetBounds().Intersects(bounds))
            buffer.Add(RecastMeshObj.dynamicMeshObjs[index]);
        }
        Rect bounds1 = Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
        RecastMeshObj.tree.QueryInBounds(bounds1, buffer);
      }
    }

    private void OnEnable()
    {
      this.Register();
    }

    private void Register()
    {
      if (this.registered)
        return;
      this.registered = true;
      this.area = Mathf.Clamp(this.area, -1, 33554432);
      Renderer component1 = this.GetComponent<Renderer>();
      Collider component2 = this.GetComponent<Collider>();
      if ((UnityEngine.Object) component1 == (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null)
        throw new Exception("A renderer or a collider should be attached to the GameObject");
      MeshFilter component3 = this.GetComponent<MeshFilter>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component3 == (UnityEngine.Object) null)
        throw new Exception("A renderer was attached but no mesh filter");
      this.bounds = !((UnityEngine.Object) component1 != (UnityEngine.Object) null) ? component2.bounds : component1.bounds;
      this._dynamic = this.dynamic;
      if (this._dynamic)
        RecastMeshObj.dynamicMeshObjs.Add(this);
      else
        RecastMeshObj.tree.Insert(this);
    }

    private void RecalculateBounds()
    {
      Renderer component1 = this.GetComponent<Renderer>();
      Collider collider = this.GetCollider();
      if ((UnityEngine.Object) component1 == (UnityEngine.Object) null && (UnityEngine.Object) collider == (UnityEngine.Object) null)
        throw new Exception("A renderer or a collider should be attached to the GameObject");
      MeshFilter component2 = this.GetComponent<MeshFilter>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null)
        throw new Exception("A renderer was attached but no mesh filter");
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        this.bounds = component1.bounds;
      else
        this.bounds = collider.bounds;
    }

    public Bounds GetBounds()
    {
      if (this._dynamic)
        this.RecalculateBounds();
      return this.bounds;
    }

    public MeshFilter GetMeshFilter()
    {
      return this.GetComponent<MeshFilter>();
    }

    public Collider GetCollider()
    {
      return this.GetComponent<Collider>();
    }

    private void OnDisable()
    {
      this.registered = false;
      if (this._dynamic)
        RecastMeshObj.dynamicMeshObjs.Remove(this);
      else if (!RecastMeshObj.tree.Remove(this))
        throw new Exception("Could not remove RecastMeshObj from tree even though it should exist in it. Has the object moved without being marked as dynamic?");
      this._dynamic = this.dynamic;
    }
  }
}
