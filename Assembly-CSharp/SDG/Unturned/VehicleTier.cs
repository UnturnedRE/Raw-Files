// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.VehicleTier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace SDG.Unturned
{
  public class VehicleTier
  {
    private List<VehicleSpawn> _table;
    private string _name;
    public float chance;

    public List<VehicleSpawn> table
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

    public VehicleTier(List<VehicleSpawn> newTable, string newName, float newChance)
    {
      this._table = newTable;
      this._name = newName;
      this.chance = newChance;
    }

    public void addVehicle(ushort id)
    {
      if (this.table.Count == (int) byte.MaxValue)
        return;
      for (byte index = (byte) 0; (int) index < this.table.Count; ++index)
      {
        if ((int) this.table[(int) index].vehicle == (int) id)
          return;
      }
      this.table.Add(new VehicleSpawn(id));
    }

    public void removeVehicle(byte index)
    {
      this.table.RemoveAt((int) index);
    }
  }
}
