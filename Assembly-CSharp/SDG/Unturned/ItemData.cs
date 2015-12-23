// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemData
  {
    private Item _item;
    private Vector3 _point;
    private bool _isDropped;
    private float _lastDropped;

    public Item item
    {
      get
      {
        return this._item;
      }
    }

    public Vector3 point
    {
      get
      {
        return this._point;
      }
    }

    public bool isDropped
    {
      get
      {
        return this._isDropped;
      }
    }

    public float lastDropped
    {
      get
      {
        return this._lastDropped;
      }
    }

    public ItemData(Item newItem, Vector3 newPoint, bool newDropped)
    {
      this._item = newItem;
      this._point = newPoint;
      this._isDropped = newDropped;
      this._lastDropped = Time.realtimeSinceStartup;
    }
  }
}
