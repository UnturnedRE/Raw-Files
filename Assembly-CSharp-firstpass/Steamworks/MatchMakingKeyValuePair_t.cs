// Decompiled with JetBrains decompiler
// Type: Steamworks.MatchMakingKeyValuePair_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  public struct MatchMakingKeyValuePair_t
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_szKey;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_szValue;

    private MatchMakingKeyValuePair_t(string strKey, string strValue)
    {
      this.m_szKey = strKey;
      this.m_szValue = strValue;
    }
  }
}
