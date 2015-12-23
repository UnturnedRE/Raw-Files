// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.DrivingPlayerInputPacket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class DrivingPlayerInputPacket : PlayerInputPacket
  {
    public Vector3 position;
    public byte angle_x;
    public byte angle_y;
    public byte angle_z;
    public byte speed;
    public byte turn;

    public override void read(SteamChannel channel)
    {
      base.read(channel);
      this.position = (Vector3) channel.read(Types.VECTOR3_TYPE);
      this.angle_x = (byte) channel.read(Types.BYTE_TYPE);
      this.angle_y = (byte) channel.read(Types.BYTE_TYPE);
      this.angle_z = (byte) channel.read(Types.BYTE_TYPE);
      this.speed = (byte) channel.read(Types.BYTE_TYPE);
      this.turn = (byte) channel.read(Types.BYTE_TYPE);
    }

    public override void write(SteamChannel channel)
    {
      base.write(channel);
      channel.write((object) this.position);
      channel.write((object) this.angle_x);
      channel.write((object) this.angle_y);
      channel.write((object) this.angle_z);
      channel.write((object) this.speed);
      channel.write((object) this.turn);
    }
  }
}
