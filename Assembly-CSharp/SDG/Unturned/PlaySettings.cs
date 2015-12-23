// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlaySettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class PlaySettings
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 3;
    public static string connectIP;
    public static ushort connectPort;
    public static string connectPassword;
    public static string serversPassword;
    public static EGameMode singleplayerMode;
    public static string singleplayerMap;
    public static string editorMap;

    public static void load()
    {
      if (ReadWrite.fileExists("/Play.dat", true))
      {
        Block block = ReadWrite.readBlock("/Play.dat", true, (byte) 0);
        if (block != null)
        {
          byte num = block.readByte();
          if ((int) num > 1)
          {
            PlaySettings.connectIP = block.readString();
            PlaySettings.connectPort = block.readUInt16();
            PlaySettings.connectPassword = block.readString();
            PlaySettings.serversPassword = block.readString();
            PlaySettings.singleplayerMode = (EGameMode) block.readByte();
            if ((int) num > 2)
            {
              PlaySettings.singleplayerMap = block.readString();
              PlaySettings.editorMap = block.readString();
              return;
            }
            PlaySettings.singleplayerMap = string.Empty;
            PlaySettings.editorMap = string.Empty;
            return;
          }
        }
      }
      PlaySettings.connectIP = "127.0.0.1";
      PlaySettings.connectPort = (ushort) 27015;
      PlaySettings.connectPassword = string.Empty;
      PlaySettings.serversPassword = string.Empty;
      PlaySettings.singleplayerMode = !Provider.isPro ? EGameMode.NORMAL : EGameMode.PRO;
      PlaySettings.singleplayerMap = string.Empty;
      PlaySettings.editorMap = string.Empty;
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(PlaySettings.SAVEDATA_VERSION);
      block.writeString(PlaySettings.connectIP);
      block.writeUInt16(PlaySettings.connectPort);
      block.writeString(PlaySettings.connectPassword);
      block.writeString(PlaySettings.serversPassword);
      block.writeByte((byte) PlaySettings.singleplayerMode);
      block.writeString(PlaySettings.singleplayerMap);
      block.writeString(PlaySettings.editorMap);
      ReadWrite.writeBlock("/Play.dat", true, block);
    }
  }
}
