﻿// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemSpawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemSpawn
  {
    private ushort _item;

    public ushort item
    {
      get
      {
        return this._item;
      }
    }

    public ItemSpawn(ushort newItem)
    {
      this._item = newItem;
    }
  }
}
