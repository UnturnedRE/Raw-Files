// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.VehicleSpawnpoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class VehicleSpawnpoint
  {
    public byte type;
    private Vector3 _point;
    private float _angle;
    private Transform _node;

    public Vector3 point
    {
      get
      {
        return this._point;
      }
    }

    public float angle
    {
      get
      {
        return this._angle;
      }
    }

    public Transform node
    {
      get
      {
        return this._node;
      }
    }

    public VehicleSpawnpoint(byte newType, Vector3 newPoint, float newAngle)
    {
      this.type = newType;
      this._point = newPoint;
      this._angle = newAngle;
      if (!Level.isEditor)
        return;
      this._node = ((GameObject) Object.Instantiate(Resources.Load("Edit/Vehicle"))).transform;
      this.node.name = this.type.ToString();
      this.node.position = this.point;
      this.node.rotation = Quaternion.Euler(0.0f, this.angle, 0.0f);
      this.node.parent = LevelVehicles.models;
      this.node.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int) this.type].color;
      this.node.FindChild("Arrow").GetComponent<Renderer>().material.color = LevelVehicles.tables[(int) this.type].color;
    }

    public void setEnabled(bool isEnabled)
    {
      this.node.transform.gameObject.SetActive(isEnabled);
    }
  }
}
