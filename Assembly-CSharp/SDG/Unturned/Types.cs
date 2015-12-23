// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Types
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class Types
  {
    public static readonly System.Type STRING_TYPE = typeof (string);
    public static readonly System.Type BOOLEAN_TYPE = typeof (bool);
    public static readonly System.Type BOOLEAN_ARRAY_TYPE = typeof (bool[]);
    public static readonly System.Type BYTE_ARRAY_TYPE = typeof (byte[]);
    public static readonly System.Type BYTE_TYPE = typeof (byte);
    public static readonly System.Type INT16_TYPE = typeof (short);
    public static readonly System.Type UINT16_TYPE = typeof (ushort);
    public static readonly System.Type INT32_ARRAY_TYPE = typeof (int[]);
    public static readonly System.Type INT32_TYPE = typeof (int);
    public static readonly System.Type UINT32_TYPE = typeof (uint);
    public static readonly System.Type SINGLE_TYPE = typeof (float);
    public static readonly System.Type INT64_TYPE = typeof (long);
    public static readonly System.Type UINT64_ARRAY_TYPE = typeof (ulong[]);
    public static readonly System.Type UINT64_TYPE = typeof (ulong);
    public static readonly System.Type STEAM_ID_TYPE = typeof (CSteamID);
    public static readonly System.Type VECTOR3_TYPE = typeof (Vector3);
    public static readonly System.Type COLOR_TYPE = typeof (Color);
    public static readonly byte[] SHIFTS = new byte[8]
    {
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 8,
      (byte) 16,
      (byte) 32,
      (byte) 64,
      (byte) sbyte.MinValue
    };
  }
}
