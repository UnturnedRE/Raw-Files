// Decompiled with JetBrains decompiler
// Type: Pathfinding.ModifierConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  public class ModifierConverter
  {
    public static bool AllBits(ModifierData a, ModifierData b)
    {
      return (a & b) == b;
    }

    public static bool AnyBits(ModifierData a, ModifierData b)
    {
      return (a & b) != ModifierData.None;
    }

    public static ModifierData Convert(Path p, ModifierData input, ModifierData output)
    {
      if (!ModifierConverter.CanConvert(input, output))
      {
        Debug.LogError((object) string.Concat(new object[4]
        {
          (object) "Can't convert ",
          (object) input,
          (object) " to ",
          (object) output
        }));
        return ModifierData.None;
      }
      if (ModifierConverter.AnyBits(input, output))
        return input;
      if (ModifierConverter.AnyBits(input, ModifierData.Nodes) && ModifierConverter.AnyBits(output, ModifierData.Vector))
      {
        p.vectorPath.Clear();
        for (int index = 0; index < p.vectorPath.Count; ++index)
          p.vectorPath.Add((Vector3) p.path[index].position);
        return (ModifierData) (8 | (!ModifierConverter.AnyBits(input, ModifierData.StrictNodePath) ? 0 : 4));
      }
      Debug.LogError((object) ("This part should not be reached - Error in ModifierConverted\nInput: " + (object) input + " (" + (string) (object) input + ")\nOutput: " + (string) (object) output + " (" + (string) (object) output + ")"));
      return ModifierData.None;
    }

    public static bool CanConvert(ModifierData input, ModifierData output)
    {
      ModifierData b = ModifierConverter.CanConvertTo(input);
      return ModifierConverter.AnyBits(output, b);
    }

    public static ModifierData CanConvertTo(ModifierData a)
    {
      if (a == ModifierData.All)
        return ModifierData.All;
      ModifierData modifierData = a;
      if (ModifierConverter.AnyBits(a, ModifierData.Nodes))
        modifierData |= ModifierData.VectorPath;
      if (ModifierConverter.AnyBits(a, ModifierData.StrictNodePath))
        modifierData |= ModifierData.StrictVectorPath;
      if (ModifierConverter.AnyBits(a, ModifierData.StrictVectorPath))
        modifierData |= ModifierData.VectorPath;
      return modifierData;
    }
  }
}
