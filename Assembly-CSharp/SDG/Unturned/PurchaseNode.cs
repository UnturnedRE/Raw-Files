// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PurchaseNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class PurchaseNode : Node
  {
    public static readonly float MIN_SIZE = 2f;
    public static readonly float MAX_SIZE = 16f;
    private float _radius;
    public ushort id;
    public uint cost;

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
        float num = PurchaseNode.MIN_SIZE + this.radius * (PurchaseNode.MAX_SIZE - PurchaseNode.MIN_SIZE);
        this.model.transform.localScale = new Vector3(num, num, num);
      }
    }

    public PurchaseNode(Vector3 newPoint)
      : this(newPoint, 0.0f, (ushort) 0, 0U)
    {
    }

    public PurchaseNode(Vector3 newPoint, float newRadius, ushort newID, uint newCost)
    {
      this._point = newPoint;
      if (Level.isEditor)
      {
        this._model = ((GameObject) Object.Instantiate(Resources.Load("Edit/Purchase"))).transform;
        this.model.name = "Node";
        this.model.position = this.point;
        this.model.parent = LevelNodes.models;
      }
      this.radius = Level.isEditor ? newRadius : Mathf.Pow((float) (((double) PurchaseNode.MIN_SIZE + (double) newRadius * ((double) PurchaseNode.MAX_SIZE - (double) PurchaseNode.MIN_SIZE)) / 2.0), 2f);
      this.id = newID;
      this.cost = newCost;
      this._type = ENodeType.PURCHASE;
    }
  }
}
