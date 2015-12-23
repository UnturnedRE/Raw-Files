// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LocationNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class LocationNode : Node
  {
    public string name;

    public LocationNode(Vector3 newPoint)
      : this(newPoint, string.Empty)
    {
    }

    public LocationNode(Vector3 newPoint, string newName)
    {
      this._point = newPoint;
      if (Level.isEditor)
      {
        this._model = ((GameObject) Object.Instantiate(Resources.Load("Edit/Location"))).transform;
        this.model.name = "Node";
        this.model.position = this.point;
        this.model.parent = LevelNodes.models;
      }
      this.name = newName;
      this._type = ENodeType.LOCATION;
    }
  }
}
