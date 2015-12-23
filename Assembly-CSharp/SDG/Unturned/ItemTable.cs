// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class ItemTable
  {
    private List<ItemTier> _tiers;
    private Color _color;
    private string _name;

    public List<ItemTier> tiers
    {
      get
      {
        return this._tiers;
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
            for (ushort index3 = (ushort) 0; (int) index3 < LevelItems.spawns[(int) index1, (int) index2].Count; ++index3)
            {
              ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int) index1, (int) index2][(int) index3];
              if ((int) itemSpawnpoint.type == (int) EditorSpawns.selectedItem)
                itemSpawnpoint.node.GetComponent<Renderer>().material.color = this.color;
            }
            EditorSpawns.itemSpawn.GetComponent<Renderer>().material.color = this.color;
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

    public ItemTable(string newName)
    {
      this._tiers = new List<ItemTier>();
      this._color = Color.white;
      this._name = newName;
    }

    public ItemTable(List<ItemTier> newTiers, Color newColor, string newName)
    {
      this._tiers = newTiers;
      this._color = newColor;
      this._name = newName;
    }

    public void addTier(string name)
    {
      if (this.tiers.Count == (int) byte.MaxValue)
        return;
      for (int index = 0; index < this.tiers.Count; ++index)
      {
        if (this.tiers[index].name == name)
          return;
      }
      if (this.tiers.Count == 0)
        this.tiers.Add(new ItemTier(new List<ItemSpawn>(), name, 1f));
      else
        this.tiers.Add(new ItemTier(new List<ItemSpawn>(), name, 0.0f));
    }

    public void removeTier(int tierIndex)
    {
      this.updateChance(tierIndex, 0.0f);
      this.tiers.RemoveAt(tierIndex);
    }

    public void addItem(byte tierIndex, ushort id)
    {
      this.tiers[(int) tierIndex].addItem(id);
    }

    public void removeItem(byte tierIndex, byte itemIndex)
    {
      this.tiers[(int) tierIndex].removeItem(itemIndex);
    }

    public ushort getItem()
    {
      float num = Random.value;
      if (this.tiers.Count == 0)
        return (ushort) 0;
      for (int index = 0; index < this.tiers.Count; ++index)
      {
        if ((double) num < (double) this.tiers[index].chance)
        {
          ItemTier itemTier = this.tiers[index];
          if (itemTier.table.Count > 0)
            return itemTier.table[Random.Range(0, itemTier.table.Count)].item;
          return (ushort) 0;
        }
      }
      ItemTier itemTier1 = this.tiers[Random.Range(0, this.tiers.Count)];
      if (itemTier1.table.Count > 0)
        return itemTier1.table[Random.Range(0, itemTier1.table.Count)].item;
      return (ushort) 0;
    }

    public void buildTable()
    {
      List<ItemTier> list = new List<ItemTier>();
      for (int index1 = 0; index1 < this.tiers.Count; ++index1)
      {
        if (list.Count == 0)
        {
          list.Add(this.tiers[index1]);
        }
        else
        {
          bool flag = false;
          for (int index2 = 0; index2 < list.Count; ++index2)
          {
            if ((double) this.tiers[index1].chance < (double) list[index2].chance)
            {
              flag = true;
              list.Insert(index2, this.tiers[index1]);
              break;
            }
          }
          if (!flag)
            list.Add(this.tiers[index1]);
        }
      }
      float num = 0.0f;
      for (int index = 0; index < list.Count; ++index)
      {
        num += list[index].chance;
        list[index].chance = num;
      }
      this._tiers = list;
    }

    public void updateChance(int tierIndex, float chance)
    {
      float f = chance - this.tiers[tierIndex].chance;
      this.tiers[tierIndex].chance = chance;
      float num1 = Mathf.Abs(f);
      while ((double) num1 > 1.0 / 1000.0)
      {
        int num2 = 0;
        for (int index = 0; index < this.tiers.Count; ++index)
        {
          if (((double) f < 0.0 && (double) this.tiers[index].chance < 1.0 || (double) f > 0.0 && (double) this.tiers[index].chance > 0.0) && index != tierIndex)
            ++num2;
        }
        if (num2 != 0)
        {
          float num3 = num1 / (float) num2;
          for (int index = 0; index < this.tiers.Count; ++index)
          {
            if (((double) f < 0.0 && (double) this.tiers[index].chance < 1.0 || (double) f > 0.0 && (double) this.tiers[index].chance > 0.0) && index != tierIndex)
            {
              if ((double) f > 0.0)
              {
                if ((double) this.tiers[index].chance >= (double) num3)
                {
                  num1 -= num3;
                  this.tiers[index].chance -= num3;
                }
                else
                {
                  num1 -= this.tiers[index].chance;
                  this.tiers[index].chance = 0.0f;
                }
              }
              else if ((double) this.tiers[index].chance <= 1.0 - (double) num3)
              {
                num1 -= num3;
                this.tiers[index].chance += num3;
              }
              else
              {
                num1 -= 1f - this.tiers[index].chance;
                this.tiers[index].chance = 1f;
              }
            }
          }
        }
        else
          break;
      }
      float num4 = 0.0f;
      for (int index = 0; index < this.tiers.Count; ++index)
        num4 += this.tiers[index].chance;
      for (int index = 0; index < this.tiers.Count; ++index)
        this.tiers[index].chance /= num4;
    }
  }
}
