// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamPacker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;

namespace SDG.Unturned
{
  public class SteamPacker
  {
    private static Block block = new Block();

    public static object read(Type type)
    {
      return SteamPacker.block.read(type);
    }

    public static object[] read(params Type[] types)
    {
      return SteamPacker.block.read(types);
    }

    public static void openRead(int prefix, byte[] bytes)
    {
      SteamPacker.block.reset(prefix, bytes);
    }

    public static void closeRead()
    {
    }

    public static void write(object objects)
    {
      SteamPacker.block.write(objects);
    }

    public static void write(params object[] objects)
    {
      SteamPacker.block.write(objects);
    }

    public static void openWrite(int prefix)
    {
      SteamPacker.block.reset(prefix);
    }

    public static byte[] closeWrite(out int size)
    {
      return SteamPacker.block.getBytes(out size);
    }

    public static byte[] getBytes(int prefix, out int size, params object[] objects)
    {
      SteamPacker.block.reset(prefix);
      SteamPacker.block.write(objects);
      return SteamPacker.block.getBytes(out size);
    }

    public static object[] getObjects(CSteamID steamID, int offset, int prefix, byte[] bytes, params Type[] types)
    {
      SteamPacker.block.reset(offset + prefix, bytes);
      if (types[0] != Types.STEAM_ID_TYPE)
        return SteamPacker.block.read(types);
      object[] objArray = SteamPacker.block.read(1, types);
      objArray[0] = (object) steamID;
      return objArray;
    }
  }
}
