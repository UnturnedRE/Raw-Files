// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class MenuSettings
  {
    private static bool hasLoaded;

    public static void load()
    {
      FilterSettings.load();
      PlaySettings.load();
      GraphicsSettings.load();
      ControlsSettings.load();
      OptionsSettings.load();
      MenuSettings.hasLoaded = true;
    }

    public static void save()
    {
      if (!MenuSettings.hasLoaded)
        return;
      FilterSettings.save();
      PlaySettings.save();
      GraphicsSettings.save();
      ControlsSettings.save();
      OptionsSettings.save();
    }
  }
}
