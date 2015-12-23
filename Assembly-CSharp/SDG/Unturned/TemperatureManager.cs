// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.TemperatureManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class TemperatureManager : MonoBehaviour
  {
    private static List<TemperatureBubble> bubbles;
    private static TemperatureManager manager;

    public static EPlayerTemperature checkPointTemperature(Vector3 point)
    {
      EPlayerTemperature eplayerTemperature = EPlayerTemperature.NONE;
      for (int index = 0; index < TemperatureManager.bubbles.Count; ++index)
      {
        TemperatureBubble temperatureBubble = TemperatureManager.bubbles[index];
        if ((double) (temperatureBubble.origin - point).sqrMagnitude < (double) temperatureBubble.sqrRadius)
        {
          if (temperatureBubble.temperature == EPlayerTemperature.BURNING)
            return temperatureBubble.temperature;
          eplayerTemperature = temperatureBubble.temperature;
        }
      }
      return eplayerTemperature;
    }

    public static TemperatureBubble registerBubble(Vector3 origin, float radius, EPlayerTemperature temperature)
    {
      TemperatureBubble temperatureBubble = new TemperatureBubble(origin, radius * radius, temperature);
      TemperatureManager.bubbles.Add(temperatureBubble);
      return temperatureBubble;
    }

    public static void deregisterBubble(TemperatureBubble bubble)
    {
      TemperatureManager.bubbles.Remove(bubble);
    }

    private void onLevelLoaded(int level)
    {
      TemperatureManager.bubbles = new List<TemperatureBubble>();
    }

    private void Start()
    {
      TemperatureManager.manager = this;
      Level.onPrePreLevelLoaded += new PrePreLevelLoaded(this.onLevelLoaded);
    }
  }
}
