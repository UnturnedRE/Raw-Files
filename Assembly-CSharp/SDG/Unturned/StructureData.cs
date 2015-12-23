// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.StructureData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class StructureData
  {
    private Structure _structure;
    private Vector3 _point;
    private byte _angle;
    private ulong _owner;
    private ulong _group;

    public Structure structure
    {
      get
      {
        return this._structure;
      }
    }

    public Vector3 point
    {
      get
      {
        return this._point;
      }
    }

    public byte angle
    {
      get
      {
        return this._angle;
      }
    }

    public ulong owner
    {
      get
      {
        return this._owner;
      }
    }

    public ulong group
    {
      get
      {
        return this._group;
      }
    }

    public StructureData(Structure newStructure, Vector3 newPoint, byte newAngle, ulong newOwner, ulong newGroup)
    {
      this._structure = newStructure;
      this._point = newPoint;
      this._angle = newAngle;
      this._owner = newOwner;
      this._group = newGroup;
    }
  }
}
