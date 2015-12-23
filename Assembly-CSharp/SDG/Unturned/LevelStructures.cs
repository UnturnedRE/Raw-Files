// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelStructures
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class LevelStructures
  {
    private static Transform _models;

    public static Transform models
    {
      get
      {
        return LevelStructures._models;
      }
    }

    public static void load()
    {
      LevelStructures._models = new GameObject().transform;
      LevelStructures.models.name = "Structures";
      LevelStructures.models.parent = Level.spawns;
      LevelStructures.models.tag = "Logic";
      LevelStructures.models.gameObject.layer = LayerMasks.LOGIC;
    }
  }
}
