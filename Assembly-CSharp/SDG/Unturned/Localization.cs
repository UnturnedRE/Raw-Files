// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Localization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Localization
  {
    public static Local tryRead(string path)
    {
      return Localization.tryRead(path, true);
    }

    public static Local tryRead(string path, bool usePath)
    {
      if (ReadWrite.fileExists(path + "/" + Provider.language + ".dat", false, usePath))
        return new Local(ReadWrite.readData(path + "/" + Provider.language + ".dat", false, usePath));
      if (ReadWrite.fileExists(path + "/English.dat", false, usePath))
        return new Local(ReadWrite.readData(path + "/English.dat", false, usePath));
      Debug.LogError((object) ("Failed to find localization at: " + path));
      return new Local();
    }

    public static Local read(string path)
    {
      if (ReadWrite.fileExists(Provider.path + Provider.language + path, false, false))
        return new Local(ReadWrite.readData(Provider.path + Provider.language + path, false, false));
      Debug.LogError((object) ("Failed to find localization at: " + path));
      return new Local();
    }
  }
}
