// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class SteamGroup
  {
    private CSteamID _steamID;
    private string _name;
    private Texture2D _icon;

    public CSteamID steamID
    {
      get
      {
        return this._steamID;
      }
    }

    public string name
    {
      get
      {
        return this._name;
      }
    }

    public Texture2D icon
    {
      get
      {
        return this._icon;
      }
    }

    public SteamGroup(CSteamID newSteamID, string newName, Texture2D newIcon)
    {
      this._steamID = newSteamID;
      this._name = newName;
      this._icon = newIcon;
    }
  }
}
