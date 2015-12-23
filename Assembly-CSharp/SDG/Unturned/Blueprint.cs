// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Blueprint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class Blueprint
  {
    private ushort _source;
    private byte _id;
    private EBlueprintType _type;
    private BlueprintSupply[] _supplies;
    private ushort _tool;
    private ushort _product;
    private ushort _build;
    private byte _level;
    private EBlueprintSkill _skill;
    public bool hasSupplies;
    public bool hasTool;
    public bool hasItem;
    public ushort tools;
    public ushort products;
    public ushort items;

    public ushort source
    {
      get
      {
        return this._source;
      }
    }

    public byte id
    {
      get
      {
        return this._id;
      }
    }

    public EBlueprintType type
    {
      get
      {
        return this._type;
      }
    }

    public BlueprintSupply[] supplies
    {
      get
      {
        return this._supplies;
      }
    }

    public ushort tool
    {
      get
      {
        return this._tool;
      }
    }

    public ushort product
    {
      get
      {
        return this._product;
      }
    }

    public ushort build
    {
      get
      {
        return this._build;
      }
    }

    public byte level
    {
      get
      {
        return this._level;
      }
    }

    public EBlueprintSkill skill
    {
      get
      {
        return this._skill;
      }
    }

    public Blueprint(ushort newSource, byte newID, EBlueprintType newType, BlueprintSupply[] newSupplies, ushort newTool, ushort newProduct, ushort newProducts, ushort newBuild, byte newLevel, EBlueprintSkill newSkill)
    {
      this._source = newSource;
      this._id = newID;
      this._type = newType;
      this._supplies = newSupplies;
      this._tool = newTool;
      this._product = newProduct;
      this.products = newProducts;
      this._build = newBuild;
      this._level = newLevel;
      this._skill = newSkill;
      this.hasSupplies = false;
      this.hasTool = false;
      this.tools = (ushort) 0;
    }

    public override string ToString()
    {
      return (string) (object) this.type + (object) ": " + (string) (object) this.supplies.Length + " -> " + (string) (object) this.product;
    }
  }
}
