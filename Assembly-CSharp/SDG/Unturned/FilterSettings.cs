// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.FilterSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class FilterSettings
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 6;
    public static string filterMap;
    public static EPassword filterPassword;
    public static EWorkshop filterWorkshop;
    public static EAttendance filterAttendance;
    public static EProtection filterProtection;
    public static ECombat filterCombat;
    public static EGameMode filterMode;
    public static ECameraMode filterCamera;

    public static void load()
    {
      if (ReadWrite.fileExists("/Filters.dat", true))
      {
        Block block = ReadWrite.readBlock("/Filters.dat", true, (byte) 0);
        if (block != null)
        {
          byte num = block.readByte();
          if ((int) num > 2)
          {
            FilterSettings.filterMap = block.readString();
            if ((int) num > 5)
            {
              FilterSettings.filterPassword = (EPassword) block.readByte();
              FilterSettings.filterWorkshop = (EWorkshop) block.readByte();
            }
            else
            {
              block.readBoolean();
              block.readBoolean();
              FilterSettings.filterPassword = EPassword.NO;
              FilterSettings.filterWorkshop = EWorkshop.NO;
            }
            FilterSettings.filterAttendance = (EAttendance) block.readByte();
            FilterSettings.filterProtection = (EProtection) block.readByte();
            FilterSettings.filterCombat = (ECombat) block.readByte();
            FilterSettings.filterMode = (EGameMode) block.readByte();
            if ((int) num > 3)
            {
              FilterSettings.filterCamera = (ECameraMode) block.readByte();
              return;
            }
            FilterSettings.filterCamera = ECameraMode.ANY;
            return;
          }
        }
      }
      FilterSettings.filterMap = string.Empty;
      FilterSettings.filterPassword = EPassword.NO;
      FilterSettings.filterWorkshop = EWorkshop.NO;
      FilterSettings.filterAttendance = EAttendance.SPACE;
      FilterSettings.filterProtection = EProtection.SECURE;
      FilterSettings.filterCombat = ECombat.ANY;
      FilterSettings.filterMode = EGameMode.NORMAL;
      FilterSettings.filterCamera = ECameraMode.ANY;
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(FilterSettings.SAVEDATA_VERSION);
      block.writeString(FilterSettings.filterMap);
      block.writeByte((byte) FilterSettings.filterPassword);
      block.writeByte((byte) FilterSettings.filterWorkshop);
      block.writeByte((byte) FilterSettings.filterAttendance);
      block.writeByte((byte) FilterSettings.filterProtection);
      block.writeByte((byte) FilterSettings.filterCombat);
      block.writeByte((byte) FilterSettings.filterMode);
      block.writeByte((byte) FilterSettings.filterCamera);
      ReadWrite.writeBlock("/Filters.dat", true, block);
    }
  }
}
