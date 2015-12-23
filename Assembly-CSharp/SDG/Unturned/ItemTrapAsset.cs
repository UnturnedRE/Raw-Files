// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemTrapAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemTrapAsset : ItemBarricadeAsset
  {
    protected float _playerDamage;
    protected float _zombieDamage;
    protected float _animalDamage;
    protected float _barricadeDamage;
    protected float _structureDamage;
    protected float _vehicleDamage;
    protected float _resourceDamage;
    protected bool _isBroken;
    protected bool _isExplosive;

    public float playerDamage
    {
      get
      {
        return this._playerDamage;
      }
    }

    public float zombieDamage
    {
      get
      {
        return this._zombieDamage;
      }
    }

    public float animalDamage
    {
      get
      {
        return this._animalDamage;
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

    public bool isBroken
    {
      get
      {
        return this._isBroken;
      }
    }

    public bool isExplosive
    {
      get
      {
        return this._isExplosive;
      }
    }

    public ItemTrapAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._playerDamage = data.readSingle("Player_Damage");
      this._zombieDamage = data.readSingle("Zombie_Damage");
      this._animalDamage = data.readSingle("Animal_Damage");
      this._barricadeDamage = data.readSingle("Barricade_Damage");
      this._structureDamage = data.readSingle("Structure_Damage");
      this._vehicleDamage = data.readSingle("Vehicle_Damage");
      this._resourceDamage = data.readSingle("Resource_Damage");
      this._isBroken = data.has("Broken");
      this._isExplosive = data.has("Explosive");
    }
  }
}
