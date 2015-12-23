﻿// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ZombieSpawnpoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ZombieSpawnpoint
  {
    public byte type;
    private Vector3 _point;
    private Transform _node;

    public Vector3 point
    {
      get
      {
        return this._point;
      }
    }

    public Transform node
    {
      get
      {
        return this._node;
      }
    }

    public ZombieSpawnpoint(byte newType, Vector3 newPoint)
    {
      this.type = newType;
      this._point = newPoint;
      if (!Level.isEditor)
        return;
      this._node = ((GameObject) Object.Instantiate(Resources.Load("Edit/Zombie"))).transform;
      this.node.name = this.type.ToString();
      this.node.position = this.point + Vector3.up;
      this.node.parent = LevelItems.models;
      this.node.GetComponent<Renderer>().material.color = LevelZombies.tables[(int) this.type].color;
    }

    public void setEnabled(bool isEnabled)
    {
      this.node.transform.gameObject.SetActive(isEnabled);
    }
  }
}
