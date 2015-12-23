// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Node
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Node
  {
    protected Vector3 _point;
    protected ENodeType _type;
    protected Transform _model;

    public Vector3 point
    {
      get
      {
        return this._point;
      }
    }

    public ENodeType type
    {
      get
      {
        return this._type;
      }
    }

    public Transform model
    {
      get
      {
        return this._model;
      }
    }

    public void move(Vector3 newPoint)
    {
      this._point = newPoint;
      this.model.position = this.point;
    }

    public void setEnabled(bool isEnabled)
    {
      this.model.gameObject.SetActive(isEnabled);
    }

    public void remove()
    {
      Object.Destroy((Object) this.model.gameObject);
    }
  }
}
