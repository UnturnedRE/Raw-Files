// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LightingManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class LightingManager : SteamCaller
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 1;
    public static MoonUpdated onMoonUpdated;
    private static LightingManager manager;
    private static uint _cycle;
    private static uint _time;
    private static uint _offset;
    private static float lastUpdate;
    private static bool isCycled;
    private static bool _isFullMoon;

    private static float day
    {
      get
      {
        return (float) LightingManager.time / (float) LightingManager.cycle;
      }
    }

    public static uint cycle
    {
      get
      {
        return LightingManager._cycle;
      }
      set
      {
        LightingManager._offset = Provider.time - (uint) ((double) LightingManager.day * (double) value);
        LightingManager._cycle = value;
        if (!Provider.isServer)
          return;
        LightingManager.manager.updateLighting();
        LightingManager.manager.channel.send("tellLightingCycle", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
        {
          (object) LightingManager.cycle
        });
      }
    }

    public static uint time
    {
      get
      {
        return LightingManager._time;
      }
      set
      {
        value %= LightingManager.cycle;
        LightingManager._offset = Provider.time - value;
        LightingManager._time = value;
        LightingManager.manager.updateLighting();
        LightingManager.manager.channel.send("tellLightingOffset", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
        {
          (object) LightingManager.offset
        });
      }
    }

    public static uint offset
    {
      get
      {
        return LightingManager._offset;
      }
    }

    public static bool isFullMoon
    {
      get
      {
        return LightingManager._isFullMoon;
      }
      set
      {
        if (value == LightingManager.isFullMoon)
          return;
        LightingManager._isFullMoon = value;
        if (LightingManager.onMoonUpdated == null)
          return;
        LightingManager.onMoonUpdated(LightingManager.isFullMoon);
      }
    }

    [SteamCall]
    public void tellLighting(CSteamID steamID, uint serverTime, uint newCycle, uint newOffset, byte moon, byte wind)
    {
      if (!this.channel.checkServer(steamID))
        return;
      Provider.time = serverTime;
      LightingManager._cycle = newCycle;
      LightingManager._offset = newOffset;
      this.updateLighting();
      LevelLighting.moon = moon;
      LightingManager.isCycled = (double) LightingManager.day > (double) LevelLighting.bias;
      LevelLighting.wind = (float) wind * 2f;
      Level.isLoadingLighting = false;
    }

    [SteamCall]
    public void askLighting(CSteamID steamID)
    {
      this.channel.send("tellLighting", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
      {
        (object) Provider.time,
        (object) LightingManager.cycle,
        (object) LightingManager.offset,
        (object) LevelLighting.moon,
        (object) MeasurementTool.angleToByte(LevelLighting.wind)
      });
    }

    [SteamCall]
    public void tellLightingCycle(CSteamID steamID, uint newScale)
    {
      if (!this.channel.checkServer(steamID))
        return;
      LightingManager._offset = Provider.time - (uint) ((double) LightingManager.day * (double) newScale);
      LightingManager._cycle = newScale;
      this.updateLighting();
    }

    [SteamCall]
    public void tellLightingOffset(CSteamID steamID, uint newCycle)
    {
      if (!this.channel.checkServer(steamID))
        return;
      LightingManager._offset = newCycle;
      this.updateLighting();
    }

    [SteamCall]
    public void tellLightingWind(CSteamID steamID, byte newWind)
    {
      if (!this.channel.checkServer(steamID))
        return;
      LevelLighting.wind = (float) newWind;
    }

    private void onClientConnected()
    {
      if (Level.info.type == ELevelType.HORDE)
        return;
      this.channel.send("askLighting", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
    }

    private void updateLighting()
    {
      LightingManager.lastUpdate = Time.realtimeSinceStartup;
      LightingManager._time = (Provider.time - LightingManager.offset) % LightingManager.cycle;
      if (Provider.isServer && (double) Random.value < 0.00999999977648258)
      {
        LevelLighting.wind = (float) Random.Range(0, 360);
        LightingManager.manager.channel.send("tellLightingWind", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
        {
          (object) MeasurementTool.angleToByte(LevelLighting.wind)
        });
      }
      if ((double) LightingManager.day > (double) LevelLighting.bias)
      {
        if (!LightingManager.isCycled)
        {
          LightingManager.isCycled = true;
          if ((int) LevelLighting.moon < (int) LevelLighting.MOON_CYCLES - 1)
          {
            ++LevelLighting.moon;
            LightingManager.isFullMoon = (int) LevelLighting.moon == 2;
          }
          else
          {
            LevelLighting.moon = (byte) 0;
            LightingManager.isFullMoon = false;
          }
        }
      }
      else if (LightingManager.isCycled)
      {
        LightingManager.isCycled = false;
        LightingManager.isFullMoon = false;
      }
      if (Dedicator.isDedicated)
        return;
      LevelLighting.time = LightingManager.day;
    }

    private void onLevelLoaded(int level)
    {
      if (level <= Level.SETUP)
        return;
      LightingManager.onMoonUpdated = (MoonUpdated) null;
      if (Level.info != null && Level.info.type == ELevelType.HORDE)
      {
        LightingManager._cycle = 3600U;
        LightingManager._time = (uint) (((double) LevelLighting.bias + (1.0 - (double) LevelLighting.bias) / 2.0) * (double) LightingManager.cycle);
        LightingManager._offset = 0U;
        LightingManager._isFullMoon = true;
        LevelLighting.wind = (float) Random.Range(0, 360);
        Level.isLoadingLighting = false;
        if (Dedicator.isDedicated)
          return;
        LevelLighting.time = LightingManager.day;
        LevelLighting.moon = (byte) 2;
      }
      else
      {
        LightingManager._cycle = 3600U;
        LightingManager._time = 0U;
        LightingManager._offset = 0U;
        LightingManager._isFullMoon = false;
        LightingManager.isCycled = false;
        LevelLighting.wind = (float) Random.Range(0, 360);
        if (!Provider.isServer)
          return;
        if (!Dedicator.isDedicated)
          LightingManager.load();
        this.updateLighting();
        Level.isLoadingLighting = false;
      }
    }

    private void FixedUpdate()
    {
      if (!Level.isLoaded || Level.info == null || (Level.info.type == ELevelType.HORDE || Level.isEditor) || (double) Time.realtimeSinceStartup - (double) LightingManager.lastUpdate <= 3.0)
        return;
      this.updateLighting();
    }

    private void Start()
    {
      LightingManager.manager = this;
      Level.onLevelLoaded += new LevelLoaded(this.onLevelLoaded);
      Provider.onClientConnected += new Provider.ClientConnected(this.onClientConnected);
    }

    public static void load()
    {
      if (LevelSavedata.fileExists("/Lighting.dat"))
      {
        River river = LevelSavedata.openRiver("/Lighting.dat", true);
        if ((int) river.readByte() > 0)
        {
          LightingManager._cycle = river.readUInt32();
          LightingManager._time = river.readUInt32();
          LightingManager._offset = Provider.time - LightingManager.time;
          return;
        }
      }
      LightingManager._time = (uint) ((double) LightingManager.cycle * (double) LevelLighting.transition);
      LightingManager._offset = Provider.time - LightingManager.time;
    }

    public static void save()
    {
      River river = LevelSavedata.openRiver("/Lighting.dat", false);
      river.writeByte(LightingManager.SAVEDATA_VERSION);
      river.writeUInt32(LightingManager.cycle);
      river.writeUInt32(LightingManager.time);
    }
  }
}
