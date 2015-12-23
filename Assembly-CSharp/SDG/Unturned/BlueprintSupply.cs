// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.BlueprintSupply
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class BlueprintSupply
  {
    private ushort _id;
    public ushort amount;
    public ushort hasAmount;

    public ushort id
    {
      get
      {
        return this._id;
      }
    }

    public BlueprintSupply(ushort newID, byte newAmount)
    {
      this._id = newID;
      this.amount = (ushort) newAmount;
      this.hasAmount = (ushort) 0;
    }
  }
}
