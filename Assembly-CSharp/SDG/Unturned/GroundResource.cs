// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.GroundResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class GroundResource
  {
    private ushort _id;
    public float density;
    public float chance;
    public bool isTree_0;
    public bool isTree_1;
    public bool isFlower;
    public bool isRock;
    public bool isSnow;

    public ushort id
    {
      get
      {
        return this._id;
      }
    }

    public GroundResource(ushort newID)
    {
      this._id = newID;
      this.density = 0.0f;
      this.chance = 0.0f;
      this.isTree_0 = true;
      this.isTree_1 = false;
      this.isFlower = false;
      this.isRock = false;
      this.isSnow = false;
    }
  }
}
