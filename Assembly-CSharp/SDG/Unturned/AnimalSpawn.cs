// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.AnimalSpawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class AnimalSpawn
  {
    private ushort _animal;

    public ushort animal
    {
      get
      {
        return this._animal;
      }
    }

    public AnimalSpawn(ushort newAnimal)
    {
      this._animal = newAnimal;
    }
  }
}
