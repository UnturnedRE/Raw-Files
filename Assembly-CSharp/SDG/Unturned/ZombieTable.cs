// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ZombieTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class ZombieTable
  {
    private ZombieSlot[] _slots;
    private Color _color;
    private string _name;
    public bool isMega;
    public ushort health;
    public byte damage;
    public byte loot;

    public ZombieSlot[] slots
    {
      get
      {
        return this._slots;
      }
    }

    public Color color
    {
      get
      {
        return this._color;
      }
      set
      {
        this._color = value;
        for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
        {
          for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          {
            for (ushort index3 = (ushort) 0; (int) index3 < LevelZombies.spawns[(int) index1, (int) index2].Count; ++index3)
            {
              ZombieSpawnpoint zombieSpawnpoint = LevelZombies.spawns[(int) index1, (int) index2][(int) index3];
              if ((int) zombieSpawnpoint.type == (int) EditorSpawns.selectedZombie)
                zombieSpawnpoint.node.GetComponent<Renderer>().material.color = this.color;
            }
            EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = this.color;
          }
        }
      }
    }

    public string name
    {
      get
      {
        return this._name;
      }
    }

    public ZombieTable(string newName)
    {
      this._slots = new ZombieSlot[4];
      for (int index = 0; index < this.slots.Length; ++index)
        this.slots[index] = new ZombieSlot(1f, new List<ZombieCloth>());
      this._color = Color.white;
      this._name = newName;
      this.isMega = false;
      this.health = (ushort) 100;
      this.damage = (byte) 15;
      this.loot = (byte) 0;
    }

    public ZombieTable(ZombieSlot[] newSlots, Color newColor, string newName, bool newMega, ushort newHealth, byte newDamage, byte newLoot)
    {
      this._slots = newSlots;
      this._color = newColor;
      this._name = newName;
      this.isMega = newMega;
      this.health = newHealth;
      this.damage = newDamage;
      this.loot = newLoot;
    }

    public void addCloth(byte slotIndex, ushort id)
    {
      this.slots[(int) slotIndex].addCloth(id);
    }

    public void removeCloth(byte slotIndex, byte clothIndex)
    {
      this.slots[(int) slotIndex].removeCloth(clothIndex);
    }
  }
}
