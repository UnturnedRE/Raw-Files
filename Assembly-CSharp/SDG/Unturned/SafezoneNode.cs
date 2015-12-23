// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SafezoneNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SafezoneNode : Node
  {
    public static readonly float MIN_SIZE = 32f;
    public static readonly float MAX_SIZE = 1024f;
    private float _radius;

    public float radius
    {
      get
      {
        return this._radius;
      }
      set
      {
        this._radius = value;
        if (!((Object) this.model != (Object) null))
          return;
        float num = SafezoneNode.MIN_SIZE + this.radius * (SafezoneNode.MAX_SIZE - SafezoneNode.MIN_SIZE);
        this.model.transform.localScale = new Vector3(num, num, num);
      }
    }

    public SafezoneNode(Vector3 newPoint)
      : this(newPoint, 0.0f)
    {
    }

    public SafezoneNode(Vector3 newPoint, float newRadius)
    {
      this._point = newPoint;
      if (Level.isEditor)
      {
        this._model = ((GameObject) Object.Instantiate(Resources.Load("Edit/Safezone"))).transform;
        this.model.name = "Node";
        this.model.position = this.point;
        this.model.parent = LevelNodes.models;
      }
      this.radius = Level.isEditor ? newRadius : Mathf.Pow((float) (((double) SafezoneNode.MIN_SIZE + (double) newRadius * ((double) SafezoneNode.MAX_SIZE - (double) SafezoneNode.MIN_SIZE)) / 2.0), 2f);
      this._type = ENodeType.SAFEZONE;
    }
  }
}
