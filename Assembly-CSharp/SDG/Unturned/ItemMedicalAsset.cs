﻿// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemMedicalAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemMedicalAsset : ItemConsumeableAsset
  {
    public ItemMedicalAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      bundle.unload();
    }
  }
}
