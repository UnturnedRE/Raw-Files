// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Hash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SDG.Unturned
{
  public class Hash
  {
    private static SHA1CryptoServiceProvider service = new SHA1CryptoServiceProvider();

    public static byte[] SHA1(byte[] bytes)
    {
      return Hash.service.ComputeHash(bytes);
    }

    public static byte[] SHA1(Stream stream)
    {
      return Hash.service.ComputeHash(stream);
    }

    public static byte[] SHA1(string text)
    {
      return Hash.SHA1(Encoding.UTF8.GetBytes(text));
    }

    public static byte[] SHA1(CSteamID steamID)
    {
      return Hash.SHA1(BitConverter.GetBytes(steamID.m_SteamID));
    }

    public static bool verifyHash(byte[] hash_0, byte[] hash_1)
    {
      if (hash_0.Length != 20 || hash_1.Length != 20)
        return false;
      for (int index = 0; index < hash_0.Length; ++index)
      {
        if ((int) hash_0[index] != (int) hash_1[index])
          return false;
      }
      return true;
    }

    public static byte[] combine(params byte[][] hashes)
    {
      byte[] bytes = new byte[hashes.Length * 20];
      for (int index = 0; index < hashes.Length; ++index)
        hashes[index].CopyTo((Array) bytes, index * 20);
      return Hash.SHA1(bytes);
    }
  }
}
