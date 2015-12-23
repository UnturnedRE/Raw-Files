// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ZombieSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace SDG.Unturned
{
  public class ZombieSlot
  {
    private List<ZombieCloth> _table;
    public float chance;

    public List<ZombieCloth> table
    {
      get
      {
        return this._table;
      }
    }

    public ZombieSlot(float newChance, List<ZombieCloth> newTable)
    {
      this._table = newTable;
      this.chance = newChance;
    }

    public void addCloth(ushort id)
    {
      if (this.table.Count == (int) byte.MaxValue)
        return;
      for (byte index = (byte) 0; (int) index < this.table.Count; ++index)
      {
        if ((int) this.table[(int) index].item == (int) id)
          return;
      }
      this.table.Add(new ZombieCloth(id));
    }

    public void removeCloth(byte index)
    {
      this.table.RemoveAt((int) index);
    }
  }
}
