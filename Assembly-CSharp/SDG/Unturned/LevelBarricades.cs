// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelBarricades
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class LevelBarricades
  {
    private static Transform _models;

    public static Transform models
    {
      get
      {
        return LevelBarricades._models;
      }
    }

    public static void load()
    {
      LevelBarricades._models = new GameObject().transform;
      LevelBarricades.models.name = "Barricades";
      LevelBarricades.models.parent = Level.spawns;
      LevelBarricades.models.tag = "Logic";
      LevelBarricades.models.gameObject.layer = LayerMasks.LOGIC;
    }
  }
}
