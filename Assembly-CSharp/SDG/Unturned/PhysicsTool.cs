// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PhysicsTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class PhysicsTool
  {
    public static EPhysicsMaterial checkMaterial(Vector3 point)
    {
      GroundMaterial material = LevelGround.getMaterial(point);
      if (material.isGrassy_0 || material.isGrassy_1 || material.isFlowery)
        return EPhysicsMaterial.FOLIAGE_STATIC;
      if (material.isRocky)
        return EPhysicsMaterial.GRAVEL_STATIC;
      return material.isSnowy ? EPhysicsMaterial.SNOW_STATIC : EPhysicsMaterial.CONCRETE_STATIC;
    }

    public static bool isMaterialDynamic(EPhysicsMaterial material)
    {
      switch (material)
      {
        case EPhysicsMaterial.CLOTH_DYNAMIC:
          return true;
        case EPhysicsMaterial.TILE_DYNAMIC:
          return true;
        case EPhysicsMaterial.CONCRETE_DYNAMIC:
          return true;
        case EPhysicsMaterial.FLESH_DYNAMIC:
          return true;
        case EPhysicsMaterial.GRAVEL_DYNAMIC:
          return true;
        case EPhysicsMaterial.METAL_DYNAMIC:
          return true;
        case EPhysicsMaterial.WOOD_DYNAMIC:
          return true;
        default:
          return false;
      }
    }

    public static EPhysicsMaterial checkMaterial(Collider collider)
    {
      string key = collider.material.name.ToString();
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (PhysicsTool.\u003C\u003Ef__switch\u0024map7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PhysicsTool.\u003C\u003Ef__switch\u0024map7 = new Dictionary<string, int>(33)
          {
            {
              "Cloth (Instance)",
              0
            },
            {
              "Cloth_Dynamic (Instance)",
              1
            },
            {
              "Cloth_Static (Instance)",
              2
            },
            {
              "Tile (Instance)",
              3
            },
            {
              "Tile_Dynamic (Instance)",
              4
            },
            {
              "Tile_Static (Instance)",
              5
            },
            {
              "Concrete (Instance)",
              6
            },
            {
              "Concrete_Dynamic (Instance)",
              7
            },
            {
              "Concrete_Static (Instance)",
              8
            },
            {
              "Flesh (Instance)",
              9
            },
            {
              "Flesh_Dynamic (Instance)",
              10
            },
            {
              "Flesh_Static (Instance)",
              11
            },
            {
              "Gravel (Instance)",
              12
            },
            {
              "Gravel_Dynamic (Instance)",
              13
            },
            {
              "Gravel_Static (Instance)",
              14
            },
            {
              "Metal (Instance)",
              15
            },
            {
              "Metal_Dynamic (Instance)",
              16
            },
            {
              "Metal_Static (Instance)",
              17
            },
            {
              "Wood (Instance)",
              18
            },
            {
              "Wood_Dynamic (Instance)",
              19
            },
            {
              "Wood_Static (Instance)",
              20
            },
            {
              "Foliage (Instance)",
              21
            },
            {
              "Foliage_Dynamic (Instance)",
              22
            },
            {
              "Foliage_Static (Instance)",
              23
            },
            {
              "Water (Instance)",
              24
            },
            {
              "Water_Dynamic (Instance)",
              25
            },
            {
              "Water_Static (Instance)",
              26
            },
            {
              "Snow (Instance)",
              27
            },
            {
              "Snow_Dynamic (Instance)",
              28
            },
            {
              "Snow_Static (Instance)",
              29
            },
            {
              "Ice (Instance)",
              30
            },
            {
              "Ice_Dynamic (Instance)",
              31
            },
            {
              "Ice_Static (Instance)",
              32
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (PhysicsTool.\u003C\u003Ef__switch\u0024map7.TryGetValue(key, out num))
        {
          switch (num)
          {
            case 0:
              return EPhysicsMaterial.CLOTH_STATIC;
            case 1:
              return EPhysicsMaterial.CLOTH_DYNAMIC;
            case 2:
              return EPhysicsMaterial.CLOTH_STATIC;
            case 3:
              return EPhysicsMaterial.TILE_STATIC;
            case 4:
              return EPhysicsMaterial.TILE_DYNAMIC;
            case 5:
              return EPhysicsMaterial.TILE_STATIC;
            case 6:
              return EPhysicsMaterial.CONCRETE_STATIC;
            case 7:
              return EPhysicsMaterial.CONCRETE_DYNAMIC;
            case 8:
              return EPhysicsMaterial.CONCRETE_STATIC;
            case 9:
              return EPhysicsMaterial.FLESH_DYNAMIC;
            case 10:
              return EPhysicsMaterial.FLESH_DYNAMIC;
            case 11:
              return EPhysicsMaterial.FLESH_DYNAMIC;
            case 12:
              return EPhysicsMaterial.GRAVEL_STATIC;
            case 13:
              return EPhysicsMaterial.GRAVEL_DYNAMIC;
            case 14:
              return EPhysicsMaterial.GRAVEL_STATIC;
            case 15:
              return EPhysicsMaterial.METAL_STATIC;
            case 16:
              return EPhysicsMaterial.METAL_DYNAMIC;
            case 17:
              return EPhysicsMaterial.METAL_STATIC;
            case 18:
              return EPhysicsMaterial.WOOD_STATIC;
            case 19:
              return EPhysicsMaterial.WOOD_DYNAMIC;
            case 20:
              return EPhysicsMaterial.WOOD_STATIC;
            case 21:
              return EPhysicsMaterial.FOLIAGE_STATIC;
            case 22:
              return EPhysicsMaterial.FOLIAGE_STATIC;
            case 23:
              return EPhysicsMaterial.FOLIAGE_STATIC;
            case 24:
              return EPhysicsMaterial.WATER_STATIC;
            case 25:
              return EPhysicsMaterial.WATER_STATIC;
            case 26:
              return EPhysicsMaterial.WATER_STATIC;
            case 27:
              return EPhysicsMaterial.SNOW_STATIC;
            case 28:
              return EPhysicsMaterial.SNOW_STATIC;
            case 29:
              return EPhysicsMaterial.SNOW_STATIC;
            case 30:
              return EPhysicsMaterial.ICE_STATIC;
            case 31:
              return EPhysicsMaterial.ICE_STATIC;
            case 32:
              return EPhysicsMaterial.ICE_STATIC;
          }
        }
      }
      return EPhysicsMaterial.NONE;
    }
  }
}
