// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorSelection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorSelection
  {
    private Transform _transform;
    private Transform _parent;
    public Vector3 fromPosition;
    public Quaternion fromRotation;

    public Transform transform
    {
      get
      {
        return this._transform;
      }
    }

    public Transform parent
    {
      get
      {
        return this._parent;
      }
    }

    public EditorSelection(Transform newTransform, Transform newParent, Vector3 newFromPosition, Quaternion newFromRotation)
    {
      this._transform = newTransform;
      this._parent = newParent;
      this.fromPosition = newFromPosition;
      this.fromRotation = newFromRotation;
    }
  }
}
