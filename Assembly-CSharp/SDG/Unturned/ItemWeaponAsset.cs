// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemWeaponAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemWeaponAsset : ItemAsset
  {
    private float _range;
    private PlayerDamageMultiplier _playerDamageMultiplier;
    private ZombieDamageMultiplier _zombieDamageMultiplier;
    private AnimalDamageMultiplier _animalDamageMultiplier;
    private float _barricadeDamage;
    private float _structureDamage;
    private float _vehicleDamage;
    private float _resourceDamage;
    private float _durability;
    protected Texture2D _albedoBase;
    protected Texture2D _metallicBase;
    protected Texture2D _emissionBase;

    public float range
    {
      get
      {
        return this._range;
      }
    }

    public PlayerDamageMultiplier playerDamageMultiplier
    {
      get
      {
        return this._playerDamageMultiplier;
      }
    }

    public ZombieDamageMultiplier zombieDamageMultiplier
    {
      get
      {
        return this._zombieDamageMultiplier;
      }
    }

    public AnimalDamageMultiplier animalDamageMultiplier
    {
      get
      {
        return this._animalDamageMultiplier;
      }
    }

    public float barricadeDamage
    {
      get
      {
        return this._barricadeDamage;
      }
    }

    public float structureDamage
    {
      get
      {
        return this._structureDamage;
      }
    }

    public float vehicleDamage
    {
      get
      {
        return this._vehicleDamage;
      }
    }

    public float resourceDamage
    {
      get
      {
        return this._resourceDamage;
      }
    }

    public float durability
    {
      get
      {
        return this._durability;
      }
    }

    public Texture albedoBase
    {
      get
      {
        return (Texture) this._albedoBase;
      }
    }

    public Texture metallicBase
    {
      get
      {
        return (Texture) this._metallicBase;
      }
    }

    public Texture emissionBase
    {
      get
      {
        return (Texture) this._emissionBase;
      }
    }

    public ItemWeaponAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._range = data.readSingle("Range");
      this._playerDamageMultiplier = new PlayerDamageMultiplier(data.readSingle("Player_Damage"), data.readSingle("Player_Leg_Multiplier"), data.readSingle("Player_Arm_Multiplier"), data.readSingle("Player_Spine_Multiplier"), data.readSingle("Player_Skull_Multiplier"));
      this._zombieDamageMultiplier = new ZombieDamageMultiplier(data.readSingle("Zombie_Damage"), data.readSingle("Zombie_Leg_Multiplier"), data.readSingle("Zombie_Arm_Multiplier"), data.readSingle("Zombie_Spine_Multiplier"), data.readSingle("Zombie_Skull_Multiplier"));
      this._animalDamageMultiplier = new AnimalDamageMultiplier(data.readSingle("Animal_Damage"), data.readSingle("Animal_Leg_Multiplier"), data.readSingle("Animal_Spine_Multiplier"), data.readSingle("Animal_Skull_Multiplier"));
      this._barricadeDamage = data.readSingle("Barricade_Damage");
      this._structureDamage = data.readSingle("Structure_Damage");
      this._vehicleDamage = data.readSingle("Vehicle_Damage");
      this._resourceDamage = data.readSingle("Resource_Damage");
      this._durability = data.readSingle("Durability");
      if (Dedicator.isDedicated)
        return;
      this._albedoBase = (Texture2D) bundle.load("Albedo_Base");
      this._metallicBase = (Texture2D) bundle.load("Metallic_Base");
      this._emissionBase = (Texture2D) bundle.load("Emission_Base");
    }
  }
}
