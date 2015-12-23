// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuConfigurationOptions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuConfigurationOptions : MonoBehaviour
  {
    private static bool hasPlayed;
    private static AudioSource music;

    public static void apply()
    {
      if (MenuConfigurationOptions.hasPlayed && OptionsSettings.music)
        return;
      MenuConfigurationOptions.hasPlayed = true;
      MenuConfigurationOptions.music.enabled = OptionsSettings.music;
    }

    private void Start()
    {
      MenuConfigurationOptions.apply();
    }

    private void Awake()
    {
      MenuConfigurationOptions.music = this.GetComponent<AudioSource>();
    }
  }
}
