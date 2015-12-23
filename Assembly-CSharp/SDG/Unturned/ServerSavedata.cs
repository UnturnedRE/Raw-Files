// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ServerSavedata
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ServerSavedata
  {
    public static string directory
    {
      get
      {
        return Dedicator.isDedicated ? "/Servers" : "/Worlds";
      }
    }

    public static void writeData(string path, Data data)
    {
      ReadWrite.writeData(ServerSavedata.directory + "/" + Provider.serverID + path, false, data);
    }

    public static Data readData(string path)
    {
      return ReadWrite.readData(ServerSavedata.directory + "/" + Provider.serverID + path, false);
    }

    public static void writeBlock(string path, Block block)
    {
      ReadWrite.writeBlock(ServerSavedata.directory + "/" + Provider.serverID + path, false, block);
    }

    public static Block readBlock(string path, byte prefix)
    {
      return ReadWrite.readBlock(ServerSavedata.directory + "/" + Provider.serverID + path, false, prefix);
    }

    public static River openRiver(string path, bool isReading)
    {
      return new River(ServerSavedata.directory + "/" + Provider.serverID + path, true, false, isReading);
    }

    public static void deleteFile(string path)
    {
      ReadWrite.deleteFile(ServerSavedata.directory + "/" + Provider.serverID + path, false);
    }

    public static bool fileExists(string path)
    {
      return ReadWrite.fileExists(ServerSavedata.directory + "/" + Provider.serverID + path, false);
    }

    public static void createFolder(string path)
    {
      ReadWrite.createFolder(ServerSavedata.directory + "/" + Provider.serverID + path);
    }

    public static void deleteFolder(string path)
    {
      ReadWrite.deleteFolder(ServerSavedata.directory + "/" + Provider.serverID + path);
    }

    public static bool folderExists(string path)
    {
      return ReadWrite.folderExists(ServerSavedata.directory + "/" + Provider.serverID + path);
    }
  }
}
