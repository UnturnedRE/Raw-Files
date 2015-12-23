// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamControllerState_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct SteamControllerState_t
  {
    public uint unPacketNum;
    public ulong ulButtons;
    public short sLeftPadX;
    public short sLeftPadY;
    public short sRightPadX;
    public short sRightPadY;
  }
}
