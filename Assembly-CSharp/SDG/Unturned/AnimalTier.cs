// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.AnimalTier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace SDG.Unturned
{
  public class AnimalTier
  {
    private List<AnimalSpawn> _table;
    private string _name;
    public float chance;

    public List<AnimalSpawn> table
    {
      get
      {
        return this._table;
      }
    }

    public string name
    {
      get
      {
        return this._name;
      }
    }

    public AnimalTier(List<AnimalSpawn> newTable, string newName, float newChance)
    {
      this._table = newTable;
      this._name = newName;
      this.chance = newChance;
    }

    public void addAnimal(ushort id)
    {
      if (this.table.Count == (int) byte.MaxValue)
        return;
      for (byte index = (byte) 0; (int) index < this.table.Count; ++index)
      {
        if ((int) this.table[(int) index].animal == (int) id)
          return;
      }
      this.table.Add(new AnimalSpawn(id));
    }

    public void removeAnimal(byte index)
    {
      this.table.RemoveAt((int) index);
    }
  }
}
