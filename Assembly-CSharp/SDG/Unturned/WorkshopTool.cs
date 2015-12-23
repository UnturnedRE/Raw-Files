// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.WorkshopTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class WorkshopTool
  {
    public static bool checkMapMeta(string path, bool usePath)
    {
      return ReadWrite.fileExists(path + "/Map.meta", false, usePath);
    }

    public static bool checkMapValid(string path, bool usePath)
    {
      return ReadWrite.getFolders(path, usePath).Length == 1;
    }

    public static bool checkLocalizationMeta(string path, bool usePath)
    {
      return ReadWrite.fileExists(path + "/Localization.meta", false, usePath);
    }

    public static bool checkLocalizationValid(string path, bool usePath)
    {
      return ReadWrite.getFolders(path, usePath).Length == 4;
    }

    public static bool checkObjectMeta(string path, bool usePath)
    {
      return ReadWrite.fileExists(path + "/Object.meta", false, usePath);
    }

    public static bool checkItemMeta(string path, bool usePath)
    {
      return ReadWrite.fileExists(path + "/Item.meta", false, usePath);
    }

    public static bool checkVehicleMeta(string path, bool usePath)
    {
      return ReadWrite.fileExists(path + "/Vehicle.meta", false, usePath);
    }

    public static bool checkSkinMeta(string path, bool usePath)
    {
      return ReadWrite.fileExists(path + "/Skin.meta", false, usePath);
    }

    public static bool checkBundleValid(string path, bool usePath)
    {
      return ReadWrite.getFolders(path, usePath).Length > 0;
    }
  }
}
