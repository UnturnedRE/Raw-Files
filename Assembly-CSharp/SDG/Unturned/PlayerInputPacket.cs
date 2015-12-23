// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerInputPacket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class PlayerInputPacket
  {
    public int sequence;

    public virtual void read(SteamChannel channel)
    {
      this.sequence = (int) channel.read(Types.INT32_TYPE);
    }

    public virtual void write(SteamChannel channel)
    {
      channel.write((object) this.sequence);
    }
  }
}
