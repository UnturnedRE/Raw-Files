// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelVisibility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class LevelVisibility
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 2;
    public static readonly byte OBJECT_REGIONS = (byte) 4;
    private static bool _roadsVisible;
    private static bool _navigationVisible;
    private static bool _nodesVisible;
    private static bool _itemsVisible;
    private static bool _playersVisible;
    private static bool _zombiesVisible;
    private static bool _vehiclesVisible;
    private static bool _borderVisible;
    private static bool _animalsVisible;

    public static bool roadsVisible
    {
      get
      {
        return LevelVisibility._roadsVisible;
      }
      set
      {
        LevelVisibility._roadsVisible = value;
        LevelRoads.setEnabled(LevelVisibility.roadsVisible);
      }
    }

    public static bool navigationVisible
    {
      get
      {
        return LevelVisibility._navigationVisible;
      }
      set
      {
        LevelVisibility._navigationVisible = value;
        LevelNavigation.setEnabled(LevelVisibility.navigationVisible);
      }
    }

    public static bool nodesVisible
    {
      get
      {
        return LevelVisibility._nodesVisible;
      }
      set
      {
        LevelVisibility._nodesVisible = value;
        LevelNodes.setEnabled(LevelVisibility.nodesVisible);
      }
    }

    public static bool itemsVisible
    {
      get
      {
        return LevelVisibility._itemsVisible;
      }
      set
      {
        LevelVisibility._itemsVisible = value;
        LevelItems.setEnabled(LevelVisibility.itemsVisible);
      }
    }

    public static bool playersVisible
    {
      get
      {
        return LevelVisibility._playersVisible;
      }
      set
      {
        LevelVisibility._playersVisible = value;
        LevelPlayers.setEnabled(LevelVisibility.playersVisible);
      }
    }

    public static bool zombiesVisible
    {
      get
      {
        return LevelVisibility._zombiesVisible;
      }
      set
      {
        LevelVisibility._zombiesVisible = value;
        LevelZombies.setEnabled(LevelVisibility.zombiesVisible);
      }
    }

    public static bool vehiclesVisible
    {
      get
      {
        return LevelVisibility._vehiclesVisible;
      }
      set
      {
        LevelVisibility._vehiclesVisible = value;
        LevelVehicles.setEnabled(LevelVisibility.vehiclesVisible);
      }
    }

    public static bool borderVisible
    {
      get
      {
        return LevelVisibility._borderVisible;
      }
      set
      {
        LevelVisibility._borderVisible = value;
        Level.setEnabled(LevelVisibility.borderVisible);
      }
    }

    public static bool animalsVisible
    {
      get
      {
        return LevelVisibility._animalsVisible;
      }
      set
      {
        LevelVisibility._animalsVisible = value;
        LevelAnimals.setEnabled(LevelVisibility.animalsVisible);
      }
    }

    public static void load()
    {
      if (!Level.isEditor)
        return;
      if (ReadWrite.fileExists(Level.info.path + "/Level/Visibility.dat", false, false))
      {
        River river = new River(Level.info.path + "/Level/Visibility.dat", false);
        byte num = river.readByte();
        if ((int) num <= 0)
          return;
        LevelVisibility.roadsVisible = river.readBoolean();
        LevelVisibility.navigationVisible = river.readBoolean();
        LevelVisibility.nodesVisible = river.readBoolean();
        LevelVisibility.itemsVisible = river.readBoolean();
        LevelVisibility.playersVisible = river.readBoolean();
        LevelVisibility.zombiesVisible = river.readBoolean();
        LevelVisibility.vehiclesVisible = river.readBoolean();
        LevelVisibility.borderVisible = river.readBoolean();
        if ((int) num > 1)
          LevelVisibility.animalsVisible = river.readBoolean();
        else
          LevelVisibility._animalsVisible = true;
        river.closeRiver();
      }
      else
      {
        LevelVisibility._roadsVisible = true;
        LevelVisibility._navigationVisible = true;
        LevelVisibility._nodesVisible = true;
        LevelVisibility._itemsVisible = true;
        LevelVisibility._playersVisible = true;
        LevelVisibility._zombiesVisible = true;
        LevelVisibility._vehiclesVisible = true;
        LevelVisibility._borderVisible = true;
        LevelVisibility._animalsVisible = true;
      }
    }

    public static void save()
    {
      River river = new River(Level.info.path + "/Level/Visibility.dat", false);
      river.writeByte(LevelVisibility.SAVEDATA_VERSION);
      river.writeBoolean(LevelVisibility.roadsVisible);
      river.writeBoolean(LevelVisibility.navigationVisible);
      river.writeBoolean(LevelVisibility.nodesVisible);
      river.writeBoolean(LevelVisibility.itemsVisible);
      river.writeBoolean(LevelVisibility.playersVisible);
      river.writeBoolean(LevelVisibility.zombiesVisible);
      river.writeBoolean(LevelVisibility.vehiclesVisible);
      river.writeBoolean(LevelVisibility.borderVisible);
      river.writeBoolean(LevelVisibility.animalsVisible);
      river.closeRiver();
    }
  }
}
