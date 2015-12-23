// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.BarricadeData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class BarricadeData
  {
    private Barricade _barricade;
    private Vector3 _point;
    private byte _angle_x;
    private byte _angle_y;
    private byte _angle_z;
    private ulong _owner;
    private ulong _group;

    public Barricade barricade
    {
      get
      {
        return this._barricade;
      }
    }

    public Vector3 point
    {
      get
      {
        return this._point;
      }
    }

    public byte angle_x
    {
      get
      {
        return this._angle_x;
      }
    }

    public byte angle_y
    {
      get
      {
        return this._angle_y;
      }
    }

    public byte angle_z
    {
      get
      {
        return this._angle_z;
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

    public BarricadeData(Barricade newBarricade, Vector3 newPoint, byte newAngle_X, byte newAngle_Y, byte newAngle_Z, ulong newOwner, ulong newGroup)
    {
      this._barricade = newBarricade;
      this._point = newPoint;
      this._angle_x = newAngle_X;
      this._angle_y = newAngle_Y;
      this._angle_z = newAngle_Z;
      this._owner = newOwner;
      this._group = newGroup;
    }
  }
}
