// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerSavedata
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class PlayerSavedata
  {
    public static void writeData(SteamPlayerID playerID, string path, Data data)
    {
      ServerSavedata.writeData("/Players/" + (object) playerID.steamID + "_" + (string) (object) playerID.characterID + "/" + Level.info.name + path, data);
    }

    public static Data readData(SteamPlayerID playerID, string path)
    {
      return ServerSavedata.readData("/Players/" + (object) playerID.steamID + "_" + (string) (object) playerID.characterID + "/" + Level.info.name + path);
    }

    public static void writeBlock(SteamPlayerID playerID, string path, Block block)
    {
      ServerSavedata.writeBlock("/Players/" + (object) playerID.steamID + "_" + (string) (object) playerID.characterID + "/" + Level.info.name + path, block);
    }

    public static Block readBlock(SteamPlayerID playerID, string path, byte prefix)
    {
      return ServerSavedata.readBlock("/Players/" + (object) playerID.steamID + "_" + (string) (object) playerID.characterID + "/" + Level.info.name + path, prefix);
    }

    public static River openRiver(SteamPlayerID playerID, string path, bool isReading)
    {
      return ServerSavedata.openRiver("/Players/" + (object) playerID.steamID + "_" + (string) (object) playerID.characterID + "/" + Level.info.name + path, isReading);
    }

    public static void deleteFile(SteamPlayerID playerID, string path)
    {
      ServerSavedata.deleteFile("/Players/" + (object) playerID.steamID + "_" + (string) (object) playerID.characterID + "/" + Level.info.name + path);
    }

    public static bool fileExists(SteamPlayerID playerID, string path)
    {
      return ServerSavedata.fileExists("/Players/" + (object) playerID.steamID + "_" + (string) (object) playerID.characterID + "/" + Level.info.name + path);
    }

    public static void deleteFolder(SteamPlayerID playerID)
    {
      for (byte index = (byte) 0; (int) index < (int) Customization.FREE_CHARACTERS + (int) Customization.PRO_CHARACTERS; ++index)
      {
        if (ServerSavedata.folderExists(string.Concat(new object[4]
        {
          (object) "/Players/",
          (object) playerID.steamID,
          (object) "_",
          (object) playerID.characterID
        })))
          ServerSavedata.deleteFolder(string.Concat(new object[4]
          {
            (object) "/Players/",
            (object) playerID.steamID,
            (object) "_",
            (object) playerID.characterID
          }));
      }
    }
  }
}
