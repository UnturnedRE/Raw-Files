// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelSavedata
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class LevelSavedata
  {
    public static void writeData(string path, Data data)
    {
      ServerSavedata.writeData("/Level/" + Level.info.name + path, data);
    }

    public static Data readData(string path)
    {
      return ServerSavedata.readData("/Level/" + Level.info.name + path);
    }

    public static void writeBlock(string path, Block block)
    {
      ServerSavedata.writeBlock("/Level/" + Level.info.name + path, block);
    }

    public static Block readBlock(string path, byte prefix)
    {
      return ServerSavedata.readBlock("/Level/" + Level.info.name + path, prefix);
    }

    public static River openRiver(string path, bool isReading)
    {
      return ServerSavedata.openRiver("/Level/" + Level.info.name + path, isReading);
    }

    public static void deleteFile(string path)
    {
      ServerSavedata.deleteFile("/Level/" + Level.info.name + path);
    }

    public static bool fileExists(string path)
    {
      return ServerSavedata.fileExists("/Level/" + Level.info.name + path);
    }
  }
}
